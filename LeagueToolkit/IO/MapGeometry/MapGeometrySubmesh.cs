using System.IO;
using System.Text;

namespace LeagueToolkit.IO.MapGeometry
{
    public struct MapGeometrySubmesh
    {
        /// <summary>
        /// This is the default material name for a <see cref="MapGeometrySubmesh"/>
        /// unless a specific one is provided
        /// </summary>
        public const string MISSING_MATERIAL = "-missing@environment-";

        public uint Hash { get; private set; }
        public string Material { get; private set; }

        public int StartIndex { get; private set; }
        public int IndexCount { get; private set; }

        public int VertexCount => this.MaxVertex - this.MinVertex + 1;
        public int MinVertex { get; private set; }
        public int MaxVertex { get; private set; }

        internal MapGeometrySubmesh(string material, int startIndex, int indexCount, int minVertex, int maxVertex)
        {
            this.Material = material ?? MISSING_MATERIAL;
            this.StartIndex = startIndex;
            this.IndexCount = indexCount;
            this.MinVertex = minVertex;
            this.MaxVertex = maxVertex;
        }

        internal MapGeometrySubmesh(BinaryReader br)
        {
            this.Hash = br.ReadUInt32();
            this.Material = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
            this.StartIndex = br.ReadInt32();
            this.IndexCount = br.ReadInt32();
            this.MinVertex = br.ReadInt32();
            this.MaxVertex = br.ReadInt32();
        }

        internal void Write(BinaryWriter bw)
        {
            bw.Write(this.Hash);
            bw.Write(this.Material.Length);
            bw.Write(Encoding.ASCII.GetBytes(this.Material));
            bw.Write(this.StartIndex);
            bw.Write(this.IndexCount);
            bw.Write(this.MinVertex);
            bw.Write(this.MaxVertex);
        }
    }
}
