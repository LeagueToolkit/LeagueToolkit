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
        public byte[] XXHash { get; private set; }
        /// <summary>
        /// Compressed Size of <see cref="Data"/>
        /// </summary>
        public uint CompressedSize { get; private set; }
        /// <summary>
        /// Uncompressed Size of <see cref="Data"/>
        /// </summary>
        public uint UncompressedSize { get; private set; }
        /// <summary>
        /// Type of this <see cref="WADEntry"/>
        /// </summary>
        public EntryType Type { get; private set; }
        /// <summary>
        /// Whether this <see cref="WADEntry"/> is contained in a <see cref="WADFile"/> more than one time
        /// </summary>
        public bool IsDuplicated { get; private set; }
        public ushort Unknown1 { get; set; }
        /// <summary>
        /// SHA256 Checksum of <see cref="Data"/>
        /// </summary>
        /// <remarks>
        /// Will be <see cref="null"/> if the version of the <see cref="WADFile"/> which it was read from is not 2.0
        /// Only first 8 bytes of the original 32 byte checksum
        /// </remarks>
        public byte[] SHA256 { get; private set; }

        /// <summary>
        /// Uncompressed Data of this <see cref="WADEntry"/>
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// File to load instead of this <see cref="WADEntry"/>
        /// </summary>
        /// <remarks>Will be <see cref="null"/> if <see cref="Type"/> isnt <c>EntryType.String</c></remarks>
        public string FileRedirection { get; private set; }

        /// <summary>
        /// Offset to the <see cref="WADEntry"/> data
        /// </summary>
        private uint _dataOffset { get; set; }

        private readonly WADFile _wad;

        /// <summary>
        /// Reads a <see cref="WADEntry"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        /// <param name="major">Major version of the <see cref="WADFile"/> which is being read</param>
        /// <param name="minor">Minor version of the <see cref="WADFile"/> which is being read</param>
        public WADEntry(WADFile wad, BinaryReader br, byte major, byte minor)
        {
            this._wad = wad;

            this.XXHash = br.ReadBytes(8);
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
        }

        /// <summary>
        /// Reads the data of this <see cref="WADEntry"/>
        /// </summary>
        /// <remarks>The type of data read depends on <see cref="Type"/></remarks>
        public void ReadData()
        {
            if (this.Type == EntryType.String)
            {
                byte[] fileRedirectionBuffer = new byte[this.UncompressedSize];
                this._wad._dataBuffer.Seek(this._dataOffset + 4, SeekOrigin.Begin);
                this._wad._dataBuffer.Read(fileRedirectionBuffer, 0, (int)this.UncompressedSize);
                this.FileRedirection = Encoding.ASCII.GetString(fileRedirectionBuffer);
            }
            else if (this.Type == EntryType.Compressed)
            {
                byte[] dataBuffer = new byte[this.CompressedSize];
                this._wad._dataBuffer.Seek(this._dataOffset, SeekOrigin.Begin);
                this._wad._dataBuffer.Read(dataBuffer, 0, (int)this.CompressedSize);
                this.Data = Compression.DecompressGZip(dataBuffer);
            }
            else if (this.Type == EntryType.Uncompressed)
            {
                byte[] dataBuffer = new byte[this.CompressedSize];
                this._wad._dataBuffer.Seek(this._dataOffset, SeekOrigin.Begin);
                this._wad._dataBuffer.Read(dataBuffer, 0, (int)this.UncompressedSize);
                this.Data = dataBuffer;
            }
        }

        public void Extract(string directoryLocation, bool identifyFile)
        {
            if (!Directory.Exists(directoryLocation))
            {
                Directory.CreateDirectory(directoryLocation);
            }

            if (this.Data == null)
            {
                ReadData();
            }

            if (identifyFile)
            {
                File.WriteAllBytes(string.Format("{0}//{1}.{2}", directoryLocation, Utilities.ByteArrayToHex(this.XXHash), this.GetEntryExtension()), this.Data);
            }
            else
            {
                File.WriteAllBytes(string.Format("{0}//{1}", directoryLocation, Utilities.ByteArrayToHex(this.XXHash)), this.Data);
            }
        }

        public ExtensionType GetEntryExtensionType()
        {
            if (this.Type != EntryType.String)
            {
                byte[] id = new byte[8];
                if (this.Data == null)
                {
                    ReadData();
                }
                Buffer.BlockCopy(this.Data, 0, id, 0, 8);

                if (id[0] == 'r' && id[1] == '3' && id[2] == 'd' && id[3] == '2')
                {
                    if (id[4] == 'M' && id[5] == 'e' && id[6] == 's' && id[7] == 'h')
                    {
                        return ExtensionType.SCB;
                    }
                    else if (id[4] == 's' && id[5] == 'k' && id[6] == 'l' && id[7] == 't')
                    {
                        return ExtensionType.SKL;
                    }
                    else if (id[4] == 'a' && id[5] == 'n' && id[6] == 'm' && id[7] == 'd')
                    {
                        return ExtensionType.ANM;
                    }
                    else if (id[4] == 'c' && id[5] == 'a' && id[6] == 'n' && id[7] == 'm')
                    {
                        return ExtensionType.ANM;
                    }
                }
                else if (id[0] == 'B' && id[1] == 'K' && id[2] == 'H' && id[3] == 'D')
                {
                    return ExtensionType.BNK;
                }
                else if (id[0] == 0x33 && id[1] == 0x22 && id[2] == 0x11 && id[3] == 0x00)
                {
                    return ExtensionType.SKN;
                }
                else if (id[0] == 'D' && id[1] == 'D' && id[2] == 'S' && id[3] == 0x20)
                {
                    return ExtensionType.DDS;
                }
                else if (id[0] == 'P' && id[1] == 'R' && id[2] == 'O' && id[3] == 'P')
                {
                    return ExtensionType.BIN;
                }
                else if (id[0] == '[' && id[1] == 'O' && id[2] == 'b' && id[3] == 'j')
                {
                    return ExtensionType.SCO;
                }
                else if (BitConverter.ToInt32(id.Take(4).ToArray(), 0) == this.Data.Length)
                {
                    return ExtensionType.SKL;
                }
            }

            return ExtensionType.Unknown;
        }

        public string GetEntryExtension()
        {
            ExtensionType extensionType = GetEntryExtensionType();
            switch (extensionType)
            {
                case ExtensionType.ANM:
                    return "anm";
                case ExtensionType.BIN:
                    return "bin";
                case ExtensionType.BNK:
                    return "bnk";
                case ExtensionType.DDS:
                    return "dds";
                case ExtensionType.SCB:
                    return "scb";
                case ExtensionType.SCO:
                    return "sco";
                case ExtensionType.SKL:
                    return "skl";
                case ExtensionType.SKN:
                    return "skn";
                default:
                    return "";
            }
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
        String
    }

    public enum ExtensionType
    {
        Unknown,
        ANM,
        BIN,
        BNK,
        DDS,
        SCB,
        SCO,
        SKL,
        SKN
    }
}
