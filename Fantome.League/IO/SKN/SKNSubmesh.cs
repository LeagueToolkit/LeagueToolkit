using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.SKN
{
    public class SKNSubmesh
    {
        public string Name { get; private set; }
        public uint StartVertex { get; private set; }
        public uint VertexCount { get; private set; }
        public uint StartIndex { get; private set; }
        public uint IndexCount { get; private set; }

        public SKNSubmesh(string name, uint startVertex, uint vertexCount, uint startIndex, uint indexCount)
        {
            this.Name = name;
            this.StartVertex = vertexCount;
            this.VertexCount = vertexCount;
            this.StartIndex = startIndex;
            this.IndexCount = indexCount;
        }

        public SKNSubmesh(BinaryReader br)
        {
            this.Name = Encoding.ASCII.GetString(br.ReadBytes(64)).Replace("\0", "");
            uint startVertex = br.ReadUInt32();
            uint vertexCount = br.ReadUInt32();
            uint startIndex = br.ReadUInt32();
            uint indexCount = br.ReadUInt32();
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(this.Name.PadRight(64, '\u0000').ToCharArray());
            bw.Write(this.StartVertex);
            bw.Write(this.VertexCount);
            bw.Write(this.StartIndex);
            bw.Write(this.IndexCount);
        }
    }
}
