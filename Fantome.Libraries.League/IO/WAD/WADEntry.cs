using Fantome.Libraries.League.Helpers.Compression;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Fantome.Libraries.League.IO.WAD
{
    /// <summary>
    /// Represents an entry in a <see cref="WADFile"/>
    /// </summary> 
    public class WADEntry : IComparable<WADEntry>
    {
        /// <summary>
        /// Hash of the <see cref="Name"/> of this <see cref="WADEntry"/>
        /// </summary>
        public ulong XXHash { get; private set; }

        /// <summary>
        /// Compressed Size of <see cref="Data"/>
        /// </summary>
        public uint CompressedSize { get; internal set; }

        /// <summary>
        /// Uncompressed Size of <see cref="Data"/>
        /// </summary>
        public uint UncompressedSize { get; internal set; }

        /// <summary>
        /// Type of this <see cref="WADEntry"/>
        /// </summary>
        public EntryType Type { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public ushort Unknown1 { get; set; }

        /// <summary>
        /// SHA256 Checksum of <see cref="Data"/>
        /// </summary>
        /// <remarks>
        /// Will be <see cref="null"/> if the version of the <see cref="WADFile"/> which it was read from is not 2.0
        /// Only first 8 bytes of the original 32 byte checksum
        /// </remarks>
        public byte[] SHA { get; internal set; }

        private string _fileRedirection;

        /// <summary>
        /// File to load instead of this <see cref="WADEntry"/>
        /// </summary>
        /// <remarks>Will be <see cref="null"/> if <see cref="Type"/> isn't <see cref="EntryType.FileRedirection"/></remarks>
        public string FileRedirection
        {
            get => this._fileRedirection;
            set
            {
                if (this.Type == EntryType.FileRedirection)
                {
                    this._fileRedirection = value;
                    this._newData = new byte[value.Length + 4];
                    Buffer.BlockCopy(BitConverter.GetBytes(value.Length), 0, this._newData, 0, 4);
                    Buffer.BlockCopy(Encoding.ASCII.GetBytes(value), 0, this._newData, 4, value.Length);
                    this.CompressedSize = (uint)this._newData.Length;
                    this.UncompressedSize = this.CompressedSize;
                    this.SHA = new byte[8];
                }
                else
                {
                    throw new Exception("The current entry is not a FileRedirection entry.");
                }
            }
        }

        /// <summary>
        /// Offset to the <see cref="WADEntry"/> data
        /// </summary>
        internal uint _dataOffset;

        /// <summary>
        /// Whether this <see cref="WADEntry"/> is contained in a <see cref="WADFile"/> more than one time
        /// </summary>
        internal bool _isDuplicated;

        /// <summary>
        /// New data replacing original data of this <see cref="WADEntry"/>.
        /// </summary>
        internal byte[] _newData;

        internal readonly WADFile _wad;

        /// <summary>
        /// Initializes a new <see cref="WADEntry"/>
        /// </summary>
        /// <param name="wad"><see cref="WADFile"/> this new entry belongs to.</param>
        /// <param name="xxHash">The XXHash of this new entry.</param>
        /// <param name="data">Data of this new entry.</param>
        /// <param name="compressedEntry">Whether the data needs to be ZStandard compressed inside WAD</param>
        public WADEntry(WADFile wad, ulong xxHash, byte[] data, bool compressedEntry)
        {
            this._wad = wad;
            this.XXHash = xxHash;
            this.Type = compressedEntry ? (wad._major == 3 ? EntryType.ZStandardCompressed : EntryType.Compressed) : EntryType.Uncompressed;
            this.EditData(data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wad"></param>
        /// <param name="xxHash"></param>
        /// <param name="fileRedirection"></param>
        public WADEntry(WADFile wad, ulong xxHash, string fileRedirection)
        {
            this._wad = wad;
            this.XXHash = xxHash;
            this.Type = EntryType.FileRedirection;  
            this.FileRedirection = fileRedirection;
        }

        /// <summary>
        /// Reads a <see cref="WADEntry"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="wad"><see cref="WADFile"/> this new entry belongs to.</param>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        /// <param name="major">Major version of the <see cref="WADFile"/> which is being read</param>
        public WADEntry(WADFile wad, BinaryReader br, byte major)
        {
            this._wad = wad;
            this.XXHash = br.ReadUInt64();
            this._dataOffset = br.ReadUInt32();
            this.CompressedSize = br.ReadUInt32();
            this.UncompressedSize = br.ReadUInt32();
            this.Type = (EntryType)br.ReadByte();
            this._isDuplicated = br.ReadBoolean();
            this.Unknown1 = br.ReadUInt16();
            if (major >= 2)
            {
                this.SHA = br.ReadBytes(8);
            }

            if (this.Type == EntryType.FileRedirection)
            {
                long currentPosition = br.BaseStream.Position;
                br.BaseStream.Seek(this._dataOffset, SeekOrigin.Begin);
                this._fileRedirection = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
                br.BaseStream.Seek(currentPosition, SeekOrigin.Begin);
            }
        }

        /// <summary>
        /// Replaces this <see cref="WADEntry"/>'s data
        /// </summary>
        /// <param name="data"></param>
        public void EditData(byte[] data)
        {
            if (this.Type == EntryType.FileRedirection)
            {
                throw new Exception("You cannot edit the data of a FileRedirection Entry");
            }
            else if (this.Type == EntryType.Compressed)
            {
                this._newData = Compression.CompressGZip(data);
            }
            else if (this.Type == EntryType.ZStandardCompressed)
            {
                this._newData = Compression.CompressZStandard(data);
            }
            else
            {
                this._newData = data;
            }

            this.CompressedSize = (uint)this._newData.Length;
            this.UncompressedSize = (uint)data.Length;
            using (SHA256 sha256 = SHA256.Create())
            {
                this.SHA = sha256.ComputeHash(this._newData).Take(8).ToArray();
            }
        }

        /// <summary>
        /// Replaces this <see cref="WADEntry"/>'s file redirection data
        /// </summary>
        /// <param name="stringData"></param>
        public void EditData(string stringData)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write(stringData.Length);
                    bw.Write(Encoding.ASCII.GetBytes(stringData));
                }
                this._newData = ms.ToArray();
            }
            this.CompressedSize = (uint)this._newData.Length;
            this.UncompressedSize = this.CompressedSize;
            this.SHA = new byte[8];
        }

        /// <summary>
        /// Returns the Data from this <see cref="WADEntry"/>.
        /// </summary>
        public byte[] GetContent(bool decompress)
        {
            byte[] dataBuffer = this._newData;
            if (dataBuffer == null)
            {
                dataBuffer = new byte[this.CompressedSize];
                this._wad._stream.Seek(this._dataOffset, SeekOrigin.Begin);
                this._wad._stream.Read(dataBuffer, 0, (int)this.CompressedSize);
            }
            if (this.Type == EntryType.Compressed && decompress)
            {
                return Compression.DecompressGZip(dataBuffer);
            }
            else if (this.Type == EntryType.ZStandardCompressed && decompress)
            {
                return Compression.DecompressZStandard(dataBuffer);
            }
            else
            {
                return dataBuffer;
            }
        }

        /// <summary>
        /// Writes this <see cref="WADEntry"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        /// <param name="major">Which Version this <see cref="WADEntry"/> should be saved as</param>
        public void Write(BinaryWriter bw, uint major)
        {
            bw.Write(this.XXHash);
            bw.Write(this._dataOffset);
            bw.Write(this.CompressedSize);
            bw.Write(this.UncompressedSize);
            bw.Write((byte)this.Type);
            bw.Write(this._isDuplicated);
            bw.Write(this.Unknown1);
            if(major >= 2)
            {
                bw.Write(this.SHA);
            }
        }

        /// <summary>
        /// Compares two <see cref="WADEntry"/> by looking at the <see cref="XXHash"/> value.
        /// </summary>
        /// <param name="other">Other <see cref="WADEntry"/> to compare the current one to.</param>
        public int CompareTo(WADEntry other)
        {
            return this.XXHash.CompareTo(other.XXHash);
        }
    }

    /// <summary>
    /// Type of a <see cref="WADEntry"/>
    /// </summary>
    public enum EntryType : byte
    {
        /// <summary>
        /// The Data of the <see cref="WADEntry"/> is uncompressed
        /// </summary>
        Uncompressed,
        /// <summary>
        /// The Data of the <see cref="WADEntry"/> is compressed with GZip
        /// </summary>
        Compressed,
        /// <summary>
        /// The Data of this <see cref="WADEntry"/> is a file redirection
        /// </summary>
        FileRedirection,
        /// <summary>
        /// The Data of this <see cref="WADEntry"/> is compressed with ZStandard
        /// </summary>
        ZStandardCompressed
    }
}
