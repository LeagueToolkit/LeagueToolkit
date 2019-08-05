using System.IO;

namespace Fantome.League.IO.BLND
{
    public class BLNDAnimation
    {
        public string Name { get; private set; }
        public uint Hash { get; private set; }
        private uint _dataOffset;

        public BLNDAnimation(BinaryReader br)
        {
            this.Hash = br.ReadUInt32();
            _dataOffset = (uint)br.BaseStream.Position + br.ReadUInt32();
        }

        public void ReadData(BinaryReader br)
        {
            this.Name = br.ReadString(4);
        }
    }
}
