using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.WAD
{
    public class WADEntry
    {
        public string Name { get; private set; }
        public byte[] XXHash { get; private set; }
        public uint CompressedSize { get; private set; }
        public uint UncompressedSize { get; private set; }
        public EntryType Type { get; private set; }
        public bool IsDuplicated { get; private set; }
        public byte Unknown1 { get; set; }
        public byte Unknown2 { get; set; }
        public byte[] SHA256 { get; private set; }
        public byte[] Data { get; private set; }
        private uint _dataOffset { get; set; }

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

    public enum EntryType : byte
    {
        Uncompressed,
        Compressed,
        String
    }
}
