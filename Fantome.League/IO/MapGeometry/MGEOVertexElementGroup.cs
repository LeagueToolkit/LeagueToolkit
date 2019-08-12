using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.IO.MapGeometry
{
    public class MGEOVertexElementGroup
    {
        public MGEOVertexElementGroupUsage Usage { get; private set; }
        public List<MGEOVertexElement> VertexElements { get; private set; } = new List<MGEOVertexElement>();

        public MGEOVertexElementGroup(BinaryReader br)
        {
            this.Usage = (MGEOVertexElementGroupUsage)br.ReadUInt32();

            uint vertexElementCount = br.ReadUInt32();
            for(int i = 0; i < vertexElementCount; i++)
            {
                this.VertexElements.Add(new MGEOVertexElement(br));
            }

            br.BaseStream.Seek(8 * (15 - vertexElementCount), SeekOrigin.Current);
        }
    }

    public enum MGEOVertexElementGroupUsage : uint
    {
        Static,
        Dynamic,
        Stream
    }
}
