using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.MapGeometry
{
    public class MGEOFile
    {
        public List<MGEOVertexElementGroup> VertexElementGroups = new List<MGEOVertexElementGroup>();
        public List<MGEOObject> Objects { get; set; } = new List<MGEOObject>();
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
                if (version != 6 && version != 7)
                {
                    throw new Exception("Version: " + version + " of Map Geometry is not supported");
                }

                bool useSeparatePointLights = false;
                if (version != 7)
                {
                    useSeparatePointLights = br.ReadBoolean();
                }

                uint vertexElementGroupCount = br.ReadUInt32();
                for(int i = 0; i < vertexElementGroupCount; i++)
                {
                    this.VertexElementGroups.Add(new MGEOVertexElementGroup(br));
                }

                uint vertexBufferCount = br.ReadUInt32();
                List<long> vertexBufferOffsets = new List<long>();
                for(int i = 0; i < vertexBufferCount; i++)
                {
                    uint bufferSize = br.ReadUInt32();

                    vertexBufferOffsets.Add(br.BaseStream.Position);
                    br.BaseStream.Seek(bufferSize, SeekOrigin.Current);
                }

                uint indexBufferCount = br.ReadUInt32();
                List<ushort[]> indexBuffers = new List<ushort[]>();
                for(int i = 0; i < indexBufferCount; i++)
                {
                    ushort[] indexBuffer;

                    uint bufferSize = br.ReadUInt32();
                    indexBuffer = new ushort[bufferSize / 2];

                    for(int j = 0; j < bufferSize / 2; j++)
                    {
                        indexBuffer[j] = br.ReadUInt16();
                    }

                    indexBuffers.Add(indexBuffer);
                }

                uint objectCount = br.ReadUInt32();
                for(int i = 0; i < objectCount; i++)
                {
                    this.Objects.Add(new MGEOObject(br, this.VertexElementGroups, vertexBufferOffsets, indexBuffers, useSeparatePointLights, version));
                }

                this.BucketGeometry = new MGEOBucketGeometry(br);
            }
        }
    }
}
