using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Fantome.Libraries.League.IO.MapGeometry
{
    public class MGEOFile
    {
        public List<MGEOMesh> Meshes { get; set; } = new List<MGEOMesh>();
        public MGEOBucketGeometry BucketGeometry { get; set; }

        public MGEOFile(string fileLocation) : this(File.OpenRead(fileLocation)) { }

        public MGEOFile(Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream))
            {
                string magic = Encoding.ASCII.GetString(br.ReadBytes(4));
                if (magic != "OEGM")
                {
                    throw new Exception("This is not a valid Map Geometry File");
                }

                uint version = br.ReadUInt32();
                if (version != 5)
                {
                    throw new Exception("Version: " + version + " of Map Geometry is not supported");
                }

                bool unknownFlag = br.ReadByte() == 1;

                List<uint> vertexBufferTypes = new List<uint>();
                uint vertexBufferTypesCount = (br.ReadUInt32() << 7) / 4;
                for (int i = 0; i < vertexBufferTypesCount; i++)
                {
                    vertexBufferTypes.Add(br.ReadUInt32());
                }

                /*List<List<MGEOVertex>> vertexBuffers = new List<List<MGEOVertex>>();
                uint vertexBufferCount = br.ReadUInt32();
                for (int i = 0; i < vertexBufferCount; i++)
                {
                    List<MGEOVertex> vertices = new List<MGEOVertex>();
                    uint vertexCount = br.ReadUInt32() / 24;
                    for (int j = 0; j < vertexCount; j++)
                    {
                        vertices.Add(new MGEOVertex(br));
                    }

                    vertexBuffers.Add(vertices);
                }*/

                List<byte[]> vertexBuffers = new List<byte[]>();
                uint vertexBufferCount = br.ReadUInt32();
                for (int i = 0; i < vertexBufferCount; i++)
                {
                    vertexBuffers.Add(br.ReadBytes((int)br.ReadUInt32()));
                }

                List<List<ushort>> indexBuffers = new List<List<ushort>>();
                uint indexBufferCount = br.ReadUInt32();
                for (int i = 0; i < indexBufferCount; i++)
                {
                    List<ushort> indices = new List<ushort>();
                    uint indexCount = br.ReadUInt32() / 2;
                    for (int j = 0; j < indexCount; j++)
                    {
                        indices.Add(br.ReadUInt16());
                    }

                    indexBuffers.Add(indices);
                }

                uint meshCount = br.ReadUInt32();
                for (int i = 0; i < meshCount; i++)
                {
                    this.Meshes.Add(new MGEOMesh(vertexBuffers, indexBuffers, br, unknownFlag));
                }

                File.WriteAllText("kek.txt", JsonConvert.SerializeObject(this.Meshes, Formatting.Indented));

                this.BucketGeometry = new MGEOBucketGeometry(br);
            }
        }
    }
}
