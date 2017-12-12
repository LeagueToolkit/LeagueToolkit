using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.LeagueSoundbank
{
    public class LeagueSoundbankEntry
    {
        public string Name { get; set; }
        public byte[] Data { get; set; }

        internal uint _dataOffset;

        public LeagueSoundbankEntry(BinaryReader br)
        {
            uint metaOffset = br.ReadUInt32();
            long returnPosition = br.BaseStream.Position;
            br.BaseStream.Seek(metaOffset, SeekOrigin.Begin);

            uint offset = br.ReadUInt32();
            uint dataSize = br.ReadUInt32();
            this.Name = Encoding.Unicode.GetString(br.ReadBytes(br.ReadInt32() * 2));

            br.BaseStream.Seek(offset, SeekOrigin.Begin);
            this.Data = br.ReadBytes((int)dataSize);
            br.BaseStream.Seek(returnPosition, SeekOrigin.Begin);
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(this._dataOffset);
            bw.Write(this.Data.Length);
            bw.Write(this.Name.Length * 2);
            bw.Write(Encoding.Unicode.GetBytes(this.Name));
            bw.Write(new byte[28 - this.Name.Length * 2]);
        }
    }
}
