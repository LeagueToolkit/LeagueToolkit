using LeagueToolkit.Helpers.Extensions;
using System.IO;

namespace LeagueToolkit.Core.Mesh
{
    public readonly struct SkinnedMeshRange
    {
        public string Material { get; }
        public int StartVertex { get; }
        public int VertexCount { get; }
        public int StartIndex { get; }
        public int IndexCount { get; }

        public SkinnedMeshRange(string material, int startVertex, int vertexCount, int startIndex, int indexCount)
        {
            this.Material = material;
            this.StartVertex = startVertex;
            this.VertexCount = vertexCount;
            this.StartIndex = startIndex;
            this.IndexCount = indexCount;
        }

        internal static SkinnedMeshRange ReadFromSimpleSkin(BinaryReader br)
        {
            string material = br.ReadPaddedString(64);
            int startVertex = br.ReadInt32();
            int vertexCount = br.ReadInt32();
            int startIndex = br.ReadInt32();
            int indexCount = br.ReadInt32();

            return new(material, startVertex, vertexCount, startIndex, indexCount);
        }

        internal void WriteToSimpleSkin(BinaryWriter bw)
        {
            bw.WritePaddedString(this.Material, 64);
            bw.Write(this.StartVertex);
            bw.Write(this.VertexCount);
            bw.Write(this.StartIndex);
            bw.Write(this.IndexCount);
        }
    }
}
