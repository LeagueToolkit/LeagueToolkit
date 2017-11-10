using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fantome.Libraries.League.Helpers.Structures;

namespace Fantome.Libraries.League.IO.MapGeometry
{
    public class MGEOModel
    {
        public string Name { get; set; }
        public MGEOMeshType Type { get; set; }
        public List<uint> VertexBuffers { get; set; } = new List<uint>();
        public List<MGEOSubmesh> Submeshes { get; set; } = new List<MGEOSubmesh>();
        public R3DBox BoundingBox { get; set; }
        public R3DMatrix44 TransformationMatrix { get; set; }
        public Vector3 Unknown7 { get; set; }
        public R3DMatrix44[] Unknown8 { get; set; } = new R3DMatrix44[3];
        public string Texture { get; set; }
        public ColorRGBAVector4 Color { get; set; }

        public MGEOModel(BinaryReader br, List<uint> vertexBufferOffsets, List<uint> indexBufferOffsets, bool specialHeaderFlag)
        {
            this.Name = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
            uint vertexCount = br.ReadUInt32();
            uint vertexBufferCount = br.ReadUInt32();
            this.Type = (MGEOMeshType)br.ReadUInt32();

            List<MGEOVertex> vertices = new List<MGEOVertex>();
            List<ushort> indices = new List<ushort>();

            for (int i = 0; i < vertexBufferCount; i++)
            {
                if (i == 0)
                {
                    long originalVertexPosition = br.BaseStream.Position + 4;
                    br.BaseStream.Seek(vertexBufferOffsets[(int)br.ReadUInt32()], SeekOrigin.Begin);

                    uint vertexCountInBuffer = br.ReadUInt32() / 32;
                    for (int j = 0; j < vertexCount; j++)
                    {
                        vertices.Add(new MGEOVertex(br));
                    }

                    br.BaseStream.Seek(originalVertexPosition, SeekOrigin.Begin);
                }
                else
                {
                    long originalVertexPosition = br.BaseStream.Position + 4;
                    br.BaseStream.Seek(vertexBufferOffsets[(int)br.ReadUInt32()], SeekOrigin.Begin);

                    uint vertexCountInBuffer = br.ReadUInt32() / 16;
                    for (int j = 0; j < vertexCountInBuffer; j++)
                    {
                        vertices[j].UV1 = new Vector2(br);
                        vertices[j].UV2 = new Vector2(br);
                    }

                    br.BaseStream.Seek(originalVertexPosition, SeekOrigin.Begin);
                }
            }

            uint indexCount = br.ReadUInt32();
            int indexBuffer = br.ReadInt32();
            long originalIndexPosition = br.BaseStream.Position;

            br.BaseStream.Seek(indexBufferOffsets[indexBuffer], SeekOrigin.Begin);
            uint indexCountInBuffer = br.ReadUInt32();
            for (int i = 0; i < indexCount; i++)
            {
                indices.Add(br.ReadUInt16());
            }
            br.BaseStream.Seek(originalIndexPosition, SeekOrigin.Begin);

            uint materialCount = br.ReadUInt32();
            for (int i = 0; i < materialCount; i++)
            {
                this.Submeshes.Add(new MGEOSubmesh(br));

                this.Submeshes[i].Vertices = vertices.GetRange((int)this.Submeshes[i]._startVertex, (int)this.Submeshes[i]._vertexCount);
                this.Submeshes[i].Indices = indices.GetRange((int)this.Submeshes[i]._startIndex, (int)this.Submeshes[i]._indexCount);
            }

            this.BoundingBox = new R3DBox(br);
            this.TransformationMatrix = new R3DMatrix44(br);
            byte padding = br.ReadByte();

            if (specialHeaderFlag)
            {
                this.Unknown7 = new Vector3(br); // No idea what this Vector might be
            }
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

    public enum MGEOMeshType : uint
    {
        Model = 0,
        DoubleBufferSimpleGeo = 2,
        Ground = 4,
        SimpleGeo2 = 5,
    }
}
