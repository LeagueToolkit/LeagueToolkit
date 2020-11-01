using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueToolkit.Helpers.Structures;

namespace LeagueToolkit.IO.MapGeometry
{
    public class MapGeometrySubmesh
    {
        public MapGeometryModel Parent { get; internal set; }
        public uint Hash { get; internal set; } = 0;
        public string Material { get; set; }
        public uint StartIndex { get; private set; }
        public uint IndexCount { get; private set; }
        public uint StartVertex { get; private set; }
        public uint VertexCount { get; private set; }

        public MapGeometrySubmesh(string material, uint startIndex, uint indexCount, uint startVertex, uint vertexCount)
        {
            this.Material = material;
            this.StartIndex = startIndex;
            this.IndexCount = indexCount;
            this.StartVertex = startVertex;
            this.VertexCount = vertexCount;
        }
        public MapGeometrySubmesh(BinaryReader br, MapGeometryModel parent)
        {
            this.Parent = parent;
            this.Hash = br.ReadUInt32();
            this.Material = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
            this.StartIndex = br.ReadUInt32();
            this.IndexCount = br.ReadUInt32();
            this.StartVertex = br.ReadUInt32(); //MinVertex
            this.VertexCount = br.ReadUInt32() + 1; //MaxVertex

            if (this.StartVertex != 0)
            {
                this.StartVertex--;
            }
        }

        public (List<ushort>, List<MapGeometryVertex>) GetData(bool normalize = true)
        {
            return (GetIndices(normalize), GetVertices());
        }
        public List<ushort> GetIndices(bool normalize = true)
        {
            List<ushort> indices = this.Parent.Indices.GetRange((int)this.StartIndex, (int)this.IndexCount);

            if(normalize)
            {
                ushort minIndex = indices.Min();

                return indices.Select(x => x -= minIndex).ToList();
            }
            else
            {
                return indices;
            }
        }
        public List<MapGeometryVertex> GetVertices()
        {
            return this.Parent.Vertices.GetRange((int)this.StartVertex, (int)(this.VertexCount - this.StartVertex));
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(this.Hash);
            bw.Write(this.Material.Length);
            bw.Write(Encoding.ASCII.GetBytes(this.Material));
            bw.Write(this.StartIndex);
            bw.Write(this.IndexCount);
            bw.Write((this.StartVertex == 0) ? this.StartVertex : this.StartVertex + 1); //edit later
            bw.Write(this.VertexCount - 1); //edit later
        }
    }
}
