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
        public uint Unknown { get; set; }
        public string Name { get; set; }
        public List<MGEOVertex> Vertices { get; set; } = new List<MGEOVertex>();
        public List<ushort> Indices { get; set; } = new List<ushort>();

        public MGEOSubmesh(BinaryReader br, List<long> vertexBufferOffsets, ushort[] indexBuffer, List<MGEOVertexElementGroup> vertexElementGroups, int vertexElementGroup)
        {
            this.Unknown = br.ReadUInt32();
            this.Name = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
            int startIndex = br.ReadInt32();
            uint indexCount = br.ReadUInt32();
            uint startVertex = br.ReadUInt32();
            uint endVertex = br.ReadUInt32() + 1;

            if (startVertex != 0)
            {
                startVertex--;
            }

            for (int i = startIndex; i < indexCount; i++)
            {
                this.Indices.Add(indexBuffer[i]);
            }

            for (int i = 0; i < endVertex - startVertex; i++)
            {
                this.Vertices.Add(new MGEOVertex());
            }

            long returnPosition = br.BaseStream.Position;
            for(int i = 0, currentVertexElementGroup = vertexElementGroup; i < vertexBufferOffsets.Count; i++, currentVertexElementGroup++)
            {
                br.BaseStream.Seek(vertexBufferOffsets[i], SeekOrigin.Begin);

                for (int j = 0; j < endVertex - startVertex; j++)
                {
                    this.Vertices[j] = MGEOVertex.Combine(this.Vertices[j], new MGEOVertex(br, vertexElementGroups[currentVertexElementGroup].VertexElements));
                }
            }

            br.BaseStream.Seek(returnPosition, SeekOrigin.Begin);
        }
    }
}
