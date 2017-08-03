using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.WAD
{
    [DebuggerDisplay("[ Type: {EntryType}, Duplicated: {IsDuplicated}, {BitConverter.ToString(XXHash)}, {Name} ]")]
    public class WADEntry
    {
        public string Name { get; private set; }
        public byte[] XXHash { get; private set; }
        private UInt32 Offset { get; set; }
        public UInt32 CompressedSize { get; private set; }
        public UInt32 UncompressedSize { get; private set; }
        public EntryType Type { get; private set; }
        public bool IsDuplicated { get; private set; }
        private byte Unknown1 { get; set; }
        private byte Unknown2 { get; set; }
        public byte[] SHA256 { get; private set; }
        public byte[] Data { get; private set; }

        public WADEntry(BinaryReader br, byte Major, byte Minor)
        {
            this.XXHash = br.ReadBytes(8);
            this.Offset = br.ReadUInt32();
            this.CompressedSize = br.ReadUInt32();
            this.UncompressedSize = br.ReadUInt32();
            this.Type = (EntryType)br.ReadByte();
            this.IsDuplicated = br.ReadBoolean();
            this.Unknown1 = br.ReadByte();
            this.Unknown2 = br.ReadByte();
            if (Major == 2 && Minor == 0)
                this.SHA256 = br.ReadBytes(8);
        }

        public void ReadData(BinaryReader br)
        {
            br.BaseStream.Seek((int)this.Offset, SeekOrigin.Begin);
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

    public enum EntryType : Byte
    {
        Uncompressed,
        Compressed,
        String
    }
}
