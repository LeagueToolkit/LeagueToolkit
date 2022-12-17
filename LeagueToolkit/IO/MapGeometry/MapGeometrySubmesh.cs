using System.IO;
using System.Text;

namespace LeagueToolkit.IO.MapGeometry
{
    public class MapGeometrySubmesh
    {
        public uint Hash { get; private set; }
        public string Material { get; private set; }
        public int StartIndex { get; private set; }
        public int IndexCount { get; private set; }
        public int StartVertex { get; private set; }
        public int VertexCount { get; private set; }

        public MapGeometrySubmesh(string material, int startIndex, int indexCount, int startVertex, int vertexCount)
        {
            this.Material = material;
            this.StartIndex = startIndex;
            this.IndexCount = indexCount;
            this.StartVertex = startVertex;
            this.VertexCount = vertexCount;
        }

        public MapGeometrySubmesh(BinaryReader br)
        {
            this.Hash = br.ReadUInt32();
            this.Material = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
            this.StartIndex = br.ReadInt32();
            this.IndexCount = br.ReadInt32();
            this.StartVertex = br.ReadInt32(); //MinVertex
            this.VertexCount = br.ReadInt32() + 1; //MaxVertex

            if (this.StartVertex != 0)
            {
                this.StartVertex--;
            }
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(this.Hash);
            bw.Write(this.Material.Length);
            bw.Write(Encoding.ASCII.GetBytes(this.Material));
            bw.Write(this.StartIndex);
            bw.Write(this.IndexCount);
            bw.Write((this.StartVertex == 0) ? this.StartVertex : this.StartVertex + 1);
            bw.Write(this.VertexCount - 1);
        }
    }
}
