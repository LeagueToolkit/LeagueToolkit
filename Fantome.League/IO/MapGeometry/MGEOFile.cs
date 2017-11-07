using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fantome.Libraries.League.IO.MapGeometry
{
    public class MGEOFile
    {
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

                uint unknownSectionSize = br.ReadUInt32() << 7;
                byte[] unknownSection = br.ReadBytes((int)unknownSectionSize);

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

                List<MGEOMesh> meshes = new List<MGEOMesh>();
                uint materialCount = br.ReadUInt32();
                for (int i = 0; i < materialCount; i++)
                {
                    meshes.Add(new MGEOMesh(br, unknownFlag));
                }
            }
        }
    }
}
