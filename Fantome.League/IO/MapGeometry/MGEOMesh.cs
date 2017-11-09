using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fantome.Libraries.League.Helpers.Structures;

namespace Fantome.Libraries.League.IO.MapGeometry
{
    public class MGEOMesh
    {
        public string Name { get; set; }
        public uint Unknown2 { get; set; }
        public List<uint> VertexBuffers { get; set; } = new List<uint>(); //Possibly the index of the vertex and index buffers ?
        public List<MGEOMaterial> Materials { get; set; } = new List<MGEOMaterial>();
        public R3DBox BoundingBox { get; set; }
        public R3DMatrix44 TransformationMatrix { get; set; }
        public Vector3 Unknown7 { get; set; }
        public R3DMatrix44[] Unknown8 { get; set; } = new R3DMatrix44[3];
        public string Texture { get; set; }
        public ColorRGBAVector4 Color { get; set; }
        public List<byte[]> Vertices { get; set; } = new List<byte[]>();
        public List<ushort> Indices { get; set; } = new List<ushort>();

        public MGEOMesh(List<byte[]> vertexBuffers, List<List<ushort>> indexBuffers, BinaryReader br, bool specialHeaderFlag)
        {
            this.Name = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
            uint vertexCount = br.ReadUInt32();
            uint vertexBufferCount = br.ReadUInt32();
            this.Unknown2 = br.ReadUInt32();

            // Most meshes have 2 vertex buffer with the first one having vertex positions and most likely normals,
            // still havent figured out what the second type is
            for (int i = 0; i < vertexBufferCount; i++)
            {
                this.VertexBuffers.Add(br.ReadUInt32());
                this.Vertices.Add(vertexBuffers[(int)this.VertexBuffers[i]]);
            }

            uint indexCount = br.ReadUInt32();
            int indexBuffer = br.ReadInt32();
            this.Indices.AddRange(indexBuffers[indexBuffer]);

            // This material structure is really similiar to SKN Submesh structure but only that there is 
            // one more uint at the start of it which might be the flags of the submesh 
            uint materialCount = br.ReadUInt32();
            for (int i = 0; i < materialCount; i++)
            {
                this.Materials.Add(new MGEOMaterial(br));
            }

            this.BoundingBox = new R3DBox(br);
            this.TransformationMatrix = new R3DMatrix44(br);
            byte padding = br.ReadByte(); // This is most likely just padding, could be flags or something though

            if (specialHeaderFlag)
            {
                this.Unknown7 = new Vector3(br); // No idea what this Vector might be
            }

            // These are most likely 3 3x3 Matrices, no idea what data they contains though
            for (int i = 0; i < 3; i++)
            {
                this.Unknown8[i] = new R3DMatrix44(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), 0,
                                                   br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), 0,
                                                   br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), 0,
                                                   0, 0, 0, 1);
            }

            this.Texture = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
            this.Color = new ColorRGBAVector4(br);
        }
    }
}
