using Fantome.Libraries.League.Helpers;
using Fantome.Libraries.League.Helpers.Compression;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Fantome.Libraries.League.IO.WAD
{
    /// <summary>
    /// Represents an entry in a <see cref="WADFile"/>
    /// </summary> 
    public class WADEntry
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
        /// Whether this <see cref="WADEntry"/> is contained in a <see cref="WADFile"/> more than one time
        /// </summary>
        public bool IsDuplicated { get; private set; }

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
        public byte[] SHA256 { get; internal set; }

        /// <summary>
        /// File to load instead of this <see cref="WADEntry"/>
        /// </summary>
        /// <remarks>Will be <see cref="null"/> if <see cref="Type"/> isn't <c>EntryType.String</c></remarks>
        public string FileRedirection { get; private set; }

        /// <summary>
        /// Offset to the <see cref="WADEntry"/> data
        /// </summary>
        internal uint _dataOffset { get; set; }

        private readonly WADFile _wad;

        /// <summary>
        /// Initializes a new <see cref="WADEntry"/>
        /// </summary>
        /// <param name="xxHash">The XXHash of this <see cref="WADEntry"/></param>
        /// <param name="type">The <see cref="EntryType"/> of this <see cref="WADEntry"/></param>
        public WADEntry(ulong xxHash, EntryType type)
        {
            this.XXHash = xxHash;

            if (type != EntryType.FileRedirection)
            {
                this.Type = type;
            }
            else
            {
                throw new Exception("Invalid EntryType. To create a file redirection Entry use the correct constructor");
            }
        }

        /// <summary>
        /// Initializes a new File Redirection <see cref="WADEntry"/>
        /// </summary>
        /// <param name="xxHash">The XXHash of this <see cref="WADEntry"/></param>
        /// <param name="fileRedirection">The file the game should load instead of this one</param>
        public WADEntry(ulong xxHash, string fileRedirection)
        {
            this.XXHash = xxHash;
            this.FileRedirection = fileRedirection;
            this.Type = EntryType.FileRedirection;
        }

        /// <summary>
        /// Reads a <see cref="WADEntry"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        /// <param name="major">Major version of the <see cref="WADFile"/> which is being read</param>
        /// <param name="minor">Minor version of the <see cref="WADFile"/> which is being read</param>
        public WADEntry(WADFile wad, BinaryReader br, byte major, byte minor)
        {
            this._wad = wad;
            this.XXHash = br.ReadUInt64();
            this._dataOffset = br.ReadUInt32();
            this.CompressedSize = br.ReadUInt32();
            this.UncompressedSize = br.ReadUInt32();
            this.Type = (EntryType)br.ReadByte();
            this.IsDuplicated = br.ReadBoolean();
            this.Unknown1 = br.ReadUInt16();
            if (major == 2 && minor == 0)
            {
                this.SHA256 = br.ReadBytes(8);
            }

            if (Type == EntryType.FileRedirection)
            {
                long currentPosition = br.BaseStream.Position;
                br.BaseStream.Seek(_dataOffset, SeekOrigin.Begin);
                this.FileRedirection = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
                br.BaseStream.Seek(currentPosition, SeekOrigin.Begin);
            }
        }

        /// <summary>
        /// Replaces this <see cref="WADEntry"/>'s data
        /// </summary>
        /// <param name="data"></param>
        public void SetData(byte[] data)
        {
            long originalPosition = this._wad._stream.Position;
            byte[] writeData = this.Type == EntryType.Compressed ? Compression.CompressGZip(data) : data;

            this._dataOffset = (uint)originalPosition;
            this.CompressedSize = (uint)writeData.Length;
            this.UncompressedSize = this.Type == EntryType.Compressed ? BitConverter.ToUInt32(writeData, writeData.Length - 4) : (uint)writeData.Length;

            if (this.Type != EntryType.FileRedirection)
            {
                this._wad._stream.Write(data, 0, data.Length);
            }
            else
            {
                this._wad._stream.Write(BitConverter.GetBytes(writeData.Length), 0, 4);
                this._wad._stream.Write(writeData, 0, writeData.Length);
            }

            this._wad._stream.Seek(originalPosition, SeekOrigin.Begin);
        }

        /// <summary>
        /// Returns the Data from this <see cref="WADEntry"/>.
        /// </summary>
        public byte[] GetContent(bool decompress)
        {
            byte[] dataBuffer = new byte[this.CompressedSize];
            _wad._stream.Seek(this._dataOffset, SeekOrigin.Begin);
            _wad._stream.Read(dataBuffer, 0, (int)this.CompressedSize);
            if (this.Type == EntryType.Compressed && decompress)
            {
                return Compression.DecompressGZip(dataBuffer);
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
        public void Write(BinaryWriter bw)
        {
            bw.Write(this.XXHash);
            bw.Write(this._dataOffset);
            bw.Write(this.CompressedSize);
            bw.Write(this.UncompressedSize);
            bw.Write((byte)this.Type);
            bw.Write(this.IsDuplicated);
            bw.Write(this.Unknown1);
            bw.Write(this.SHA256);
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
        FileRedirection
    }
}
