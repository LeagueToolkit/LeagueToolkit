using System.Collections.Generic;
using System.IO;
using System.Text;
using LeagueToolkit.IO.PropertyBin.Properties;

namespace LeagueToolkit.IO.PropertyBin
{
    public class BinTreePatchObject : IBinNestedProvider
    {
        public uint PathHash { get; }

        public List<(BinTreeProperty, string)> Properties { get; } = new();

        public BinTreePatchObject(uint pathHash)
        {
            this.PathHash = pathHash;
        }

        public void Write(BinaryWriter bw)
        {
            foreach ((BinTreeProperty property, string name) in Properties)
            {
                if (property is BinTreeNested nested)
                {
                    foreach ((string namePart, BinTreeProperty p) in nested.GetObjects())
                    {
                        WriteProperty(bw, $"{name}.{namePart}", p);
                    }
                }
                else
                {
                    WriteProperty(bw, name, property);
                }
            }
        }

        private void WriteProperty(BinaryWriter bw, string name, BinTreeProperty property)
        {
            bw.Write(PathHash);
            bw.Write(property.GetSize(false) + name.Length + 3); // size of entry
            bw.Write((byte)BinUtilities.PackType(property.Type));
            bw.Write((ushort)name.Length);
            bw.Write(Encoding.ASCII.GetBytes(name));
            property.Write(bw, false);
        }
    }

    public interface IBinNestedProvider : IBinTreeParent
    {
        public List<(BinTreeProperty, string)> Properties { get; }
    }
}
