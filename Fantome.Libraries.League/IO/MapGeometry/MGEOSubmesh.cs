﻿using System;
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
        public MGEOModel Parent { get; internal set; }
        public uint Hash { get; internal set; } = 0;
        public string Material { get; set; }
        public uint StartIndex { get; set; }
        public uint IndexCount { get; set; }
        public uint StartVertex { get; set; }
        public uint VertexCount { get; set; }

        public MGEOSubmesh(string material, uint startIndex, uint indexCount, uint startVertex, uint vertexCount)
        {
            this.Material = material;
            this.StartIndex = startIndex;
            this.IndexCount = indexCount;
            this.StartVertex = startVertex;
            this.VertexCount = vertexCount;
        }
        public MGEOSubmesh(BinaryReader br, MGEOModel parent)
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

        public Tuple<List<ushort>, List<MGEOVertex>> GetData(bool normalize = true)
        {
            return new Tuple<List<ushort>, List<MGEOVertex>>(GetIndices(normalize), GetVertices());
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
        public List<MGEOVertex> GetVertices()
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
