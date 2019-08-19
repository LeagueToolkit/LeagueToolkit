using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fantome.Libraries.League.Helpers.Structures;
using Fantome.Libraries.League.IO.OBJ;

namespace Fantome.Libraries.League.IO.MapGeometry
{
    public class MGEOObject
    {
        public string Name { get; set; }
        public List<MGEOVertex> Vertices { get; set; } = new List<MGEOVertex>();
        public List<ushort> Indices { get; set; } = new List<ushort>();
        public List<MGEOSubmesh> Submeshes { get; set; } = new List<MGEOSubmesh>();
        public bool Unknown1 { get; set; }
        public R3DBox BoundingBox { get; set; }
        public R3DMatrix44 Transformation { get; set; }
        public byte Unknown2 { get; set; }
        public byte Unknown3 { get; set; } = 0xFF;
        public Vector3 SeparatePointLight { get; set; }
        public List<Vector3> UnknownFloats { get; set; } = new List<Vector3>();
        public string Lightmap { get; set; }
        public ColorRGBAVector4 Color { get; set; }

        internal int _vertexElementGroupID;
        internal int _vertexBufferID;
        internal int _indexBufferID;

        public MGEOObject(string name, OBJFile obj, List<MGEOSubmesh> submeshes)
        {

        }

        public MGEOObject(BinaryReader br, List<MGEOVertexElementGroup> vertexElementGroups, List<long> vertexBufferOffsets, List<ushort[]> indexBuffers, bool useSeparatePointLights, uint version)
        {
            this.Name = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
            uint vertexCount = br.ReadUInt32();
            uint vertexBufferCount = br.ReadUInt32();
            int vertexElementGroup = br.ReadInt32();

            for(int i = 0; i < vertexCount; i++)
            {
                this.Vertices.Add(new MGEOVertex());
            }

            for (int i = 0, currentVertexElementGroup = vertexElementGroup; i < vertexBufferCount; i++, currentVertexElementGroup++)
            {
                int vertexBufferID = br.ReadInt32();
                long returnPosition = br.BaseStream.Position;
                br.BaseStream.Seek(vertexBufferOffsets[vertexBufferID], SeekOrigin.Begin);

                for (int j = 0; j < vertexCount; j++)
                {
                    this.Vertices[j] = MGEOVertex.Combine(this.Vertices[j], new MGEOVertex(br, vertexElementGroups[currentVertexElementGroup].VertexElements));
                }

                br.BaseStream.Seek(returnPosition, SeekOrigin.Begin);
            }

            uint indexCount = br.ReadUInt32();
            int indexBuffer = br.ReadInt32();
            this.Indices.AddRange(indexBuffers[indexBuffer]);

            uint submeshCount = br.ReadUInt32();
            for (int i = 0; i < submeshCount; i++)
            {
                this.Submeshes.Add(new MGEOSubmesh(br, this));
            }

            if(version != 5)
            {
                this.Unknown1 = br.ReadBoolean();
            }

            this.BoundingBox = new R3DBox(br);
            this.Transformation = new R3DMatrix44(br);
            this.Unknown2 = br.ReadByte();

            if(version == 7)
            {
                this.Unknown3 = br.ReadByte();
            }

            if (useSeparatePointLights)
            {
                this.SeparatePointLight = new Vector3(br);
            }
            for (int i = 0; i < 9; i++)
            {
                this.UnknownFloats.Add(new Vector3(br));
            }

            this.Lightmap = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
            this.Color = new ColorRGBAVector4(br);
        }

        public void Write(BinaryWriter bw, bool useSeparatePointLights, uint version)
        {
            bw.Write(this.Name.Length);
            bw.Write(Encoding.ASCII.GetBytes(this.Name));

            bw.Write(this.Vertices.Count);
            bw.Write((uint)1);
            bw.Write(this._vertexElementGroupID);
            bw.Write(this._vertexBufferID); //we only have one vertex buffer

            bw.Write(this.Indices.Count);
            bw.Write(this._indexBufferID);

            bw.Write(this.Submeshes.Count);
            foreach(MGEOSubmesh submesh in this.Submeshes)
            {
                submesh.Write(bw);
            }

            if(version != 5)
            {
                bw.Write(this.Unknown1);
            }

            this.BoundingBox.Write(bw);
            this.Transformation.Write(bw);
            bw.Write(this.Unknown2);

            if(version == 7)
            {
                bw.Write(this.Unknown3);
            }

            if(useSeparatePointLights)
            {
                if(this.SeparatePointLight == null)
                {
                    new Vector3(0, 0, 0).Write(bw);
                }
                else
                {
                    this.SeparatePointLight.Write(bw);
                }
            }

            foreach (Vector3 pointLight in this.UnknownFloats)
            {
                pointLight.Write(bw);
            }
            for(int i = 0; i < 9 - this.UnknownFloats.Count; i++)
            {
                new Vector3(0, 0, 0).Write(bw);
            }

            bw.Write(this.Lightmap.Length);
            bw.Write(Encoding.ASCII.GetBytes(this.Lightmap));
            this.Color.Write(bw);
        }

        public R3DBox CalculateBoundingBox()
        {
            if (this.Vertices == null || this.Vertices.Count == 0)
            {
                return new R3DBox(new Vector3(0, 0, 0), new Vector3(0, 0, 0));
            }
            else
            {
                Vector3 min = new Vector3(this.Vertices[0].Position);
                Vector3 max = new Vector3(this.Vertices[0].Position);

                foreach (MGEOVertex vertex in this.Vertices)
                {
                    if (min.X > vertex.Position.X) min.X = vertex.Position.X;
                    if (min.Y > vertex.Position.Y) min.Y = vertex.Position.Y;
                    if (min.Z > vertex.Position.Z) min.Z = vertex.Position.Z;
                    if (max.X < vertex.Position.X) max.X = vertex.Position.X;
                    if (max.Y < vertex.Position.Y) max.Y = vertex.Position.Y;
                    if (max.Z < vertex.Position.Z) max.Z = vertex.Position.Z;
                }

                return new R3DBox(min, max);
            }
        }
    }
}
