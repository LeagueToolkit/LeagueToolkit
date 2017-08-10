using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.WAD
{
    /// <summary>
    /// Represents an entry in a <see cref="WADFile"/>
    /// </summary> 
    public class WADEntry
    {
        /// <summary>
        /// Name of this <see cref="WADEntry"/>
        /// </summary>
        /// <remarks>Will be <see cref="null"/> if <see cref="Type"/> is not <c>EntryType.String</c></remarks>
        public string Name { get; private set; }
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
        public byte Unknown1 { get; set; }
        public byte Unknown2 { get; set; }
        /// <summary>
        /// SHA256 Checksum of <see cref="Data"/>
        /// </summary>
        /// <remarks>
        /// Will be <see cref="null"/> if the version of the <see cref="WADFile"/> which it was read from is not 2.0
        /// Only first 8 bytes of the original 32 byte checksum
        /// </remarks>
        public byte[] SHA256 { get; private set; }
        /// <summary>
        /// Data of this <see cref="WADEntry"/>
        /// </summary>
        /// <remarks>
        /// The conent it contains is compressed with GZip\n
        /// Will be <see cref="null"/> if <see cref="Type"/> is <c>EntryType.String</c> and the data is <see cref="Name"/>
        /// </remarks>
        public byte[] Data { get; private set; }
        /// <summary>
        /// Offset to the <see cref="WADEntry"/> data
        /// </summary>
        private uint _dataOffset { get; set; }

        /// <summary>
        /// Reads a <see cref="WADEntry"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        /// <param name="major">Major version of the <see cref="WADFile"/> which is being read</param>
        /// <param name="minor">Minor version of the <see cref="WADFile"/> which is being read</param>
        public WADEntry(BinaryReader br, byte major, byte minor)
        {
            this.XXHash = br.ReadBytes(8);
            this._dataOffset = br.ReadUInt32();
            this.CompressedSize = br.ReadUInt32();
            this.UncompressedSize = br.ReadUInt32();
            this.Type = (EntryType)br.ReadByte();
            this.IsDuplicated = br.ReadBoolean();
            this.Unknown1 = br.ReadByte();
            this.Unknown2 = br.ReadByte();
            if (major == 2 && minor == 0)
                this.SHA256 = br.ReadBytes(8);
        }

        /// <summary>
        /// Reads the data of this <see cref="WADEntry"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        /// <remarks>The type of data read depends on <see cref="Type"/></remarks>
        public void ReadData(BinaryReader br)
        {
            br.BaseStream.Seek(this._dataOffset, SeekOrigin.Begin);
            if (this.Type == EntryType.String)
            {
                this.Name = Encoding.ASCII.GetString(br.ReadBytes((int)br.ReadUInt32()));
            }
            else
            {
                this.Data = br.ReadBytes((int)this.CompressedSize);
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
        /// The Data of this <see cref="WADEntry"/> is a string and is its Name
        /// </summary>
        String
    }
}
