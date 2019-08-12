using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fantome.Libraries.League.Helpers.Structures;

namespace Fantome.Libraries.League.IO.MapGeometry
{
    public class MGEOObject
    {
        public string Name { get; set; }
        public List<MGEOSubmesh> Submeshes { get; set; } = new List<MGEOSubmesh>();
        public R3DBox BoundingBox { get; set; }
        public R3DMatrix44 TransformationMatrix { get; set; }
        public Vector3 PointLight { get; set; }
        public R3DMatrix44[] Unknown8 { get; set; } = new R3DMatrix44[3];
        public string Lightmap { get; set; }
        public ColorRGBAVector4 Color { get; set; }

        public MGEOObject(BinaryReader br, List<MGEOVertexElementGroup> vertexElementGroups, List<long> vertexBuffers, List<ushort[]> indexBuffers, bool useSeparatePointLights, uint version)
        {
            this.Name = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
            uint vertexCount = br.ReadUInt32();
            uint vertexBufferCount = br.ReadUInt32();
            int vertexElementGroup = br.ReadInt32();

            int[] vertexBufferIDs;
            vertexBufferIDs = new int[vertexBufferCount];
            for(int i = 0; i < vertexBufferCount; i++)
            {
                vertexBufferIDs[i] = br.ReadInt32();
            }

            uint indexCount = br.ReadUInt32();
            int indexBuffer = br.ReadInt32();

            uint submeshCount = br.ReadUInt32();
            for (int i = 0; i < submeshCount; i++)
            {
                List<long> vertexBufferOffsetsSubmesh = new List<long>();
                for(int j = 0; j < vertexBufferCount; j++)
                {
                    vertexBufferOffsetsSubmesh.Add(vertexBuffers[vertexBufferIDs[j]]);
                }

                this.Submeshes.Add(new MGEOSubmesh(br, vertexBufferOffsetsSubmesh, indexBuffers[indexBuffer], vertexElementGroups, vertexElementGroup));
            }

            bool unknown1 = false;
            if(version != 5)
            {
                unknown1 = br.ReadBoolean();
            }

            this.BoundingBox = new R3DBox(br);
            this.TransformationMatrix = new R3DMatrix44(br);

            byte unknown2 = br.ReadByte();
            byte unknown3 = 0;
            if(version == 7)
            {
                unknown3 = br.ReadByte();
            }

            if (useSeparatePointLights)
            {
                this.PointLight = new Vector3(br);
            }
            for (int i = 0; i < 3; i++)
            {
                this.Unknown8[i] = new R3DMatrix44(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), 0,
                                                   br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), 0,
                                                   br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), 0,
                                                   0, 0, 0, 1);
            }

            this.Lightmap = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
            this.Color = new ColorRGBAVector4(br);

            if(vertexBufferCount > 1)
            {
                
            }
        }
    }
}
