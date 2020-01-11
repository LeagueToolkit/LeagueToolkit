using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fantome.Libraries.League.Helpers.Structures;

namespace Fantome.Libraries.League.IO.MapGeometry
{
    public class MGEOSubmesh
    {
        public MGEOObject Parent { get; internal set; }
        public uint Hash { get; internal set; } = 0;
        public string Material { get; set; }
        public uint StartIndex { get; set; }
        public uint IndexCount { get; set; }
        public uint MinVertex { get; set; }
        public uint MaxVertex { get; set; }

        public MGEOSubmesh(string material, uint startIndex, uint indexCount, uint minVertex, uint maxVertex)
        {
            this.Material = material;
            this.StartIndex = startIndex;
            this.IndexCount = indexCount;
            this.MinVertex = minVertex;
            this.MaxVertex = maxVertex;
        }
        public MGEOSubmesh(BinaryReader br, MGEOObject parent)
        {
            this.Parent = parent;
            this.Hash = br.ReadUInt32();
            this.Material = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
            this.StartIndex = br.ReadUInt32();
            this.IndexCount = br.ReadUInt32();
            this.MinVertex = br.ReadUInt32();
            this.MaxVertex = br.ReadUInt32() + 1;

            if (this.MinVertex != 0)
            {
                this.MinVertex--;
            }
        }

        public Tuple<List<ushort>, List<MGEOVertex>> GetData()
        {
            return new Tuple<List<ushort>, List<MGEOVertex>>(GetIndices(), GetVertices());
        }
        public List<ushort> GetIndices()
        {
            return this.Parent.Indices.GetRange((int)this.StartIndex, (int)this.IndexCount);
        }
        public List<MGEOVertex> GetVertices()
        {
            return this.Parent.Vertices.GetRange((int)this.MinVertex, (int)(this.MaxVertex - this.MinVertex));
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(this.Hash);
            bw.Write(this.Material.Length);
            bw.Write(Encoding.ASCII.GetBytes(this.Material));
            bw.Write(this.StartIndex);
            bw.Write(this.IndexCount);
            bw.Write((this.MinVertex == 0) ? this.MinVertex : this.MinVertex + 1); //edit later
            bw.Write(this.MaxVertex - 1); //edit later
        }
    }
}
