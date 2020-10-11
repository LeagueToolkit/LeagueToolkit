using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.IO.PropertyBin
{
    public class BinTreeObject : IBinTreeParent
    {
        public uint MetaClassHash { get; private set; }
        public uint PathHash { get; private set; }

        public List<BinTreeProperty> Properties { get; private set; } = new();

        internal BinTreeObject(uint metaClassHash)
        {
            this.MetaClassHash = metaClassHash;
        }

        internal static BinTreeObject ReadHeader(BinaryReader br)
        {
            uint metaClassHash = br.ReadUInt32();

            return new BinTreeObject(metaClassHash);
        }

        internal void ReadData(BinaryReader br)
        {
            uint size = br.ReadUInt32();
            this.PathHash = br.ReadUInt32();

            ushort propertyCount = br.ReadUInt16();
            for (int i = 0; i < propertyCount; i++)
            {
                this.Properties.Add(BinTreeProperty.Read(br, this, null));
            }
        }

        internal void WriteHeader(BinaryWriter bw)
        {
            bw.Write(this.MetaClassHash);
        }
        internal void WriteContent(BinaryWriter bw)
        {
            bw.Write(GetSize());
            bw.Write(this.PathHash);

            bw.Write((ushort)this.Properties.Count);
            foreach (BinTreeProperty property in this.Properties)
            {
                property.Write(bw, true);
            }
        }

        private int GetSize()
        {
            int size = 4 + 2;
            foreach (BinTreeProperty property in this.Properties)
            {
                size += property.GetSize(true);
            }

            return size;
        }
    }
}
