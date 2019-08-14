using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.MapGeometry
{
    public class MGEOFile
    {
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

                List<MGEOVertexElementGroup> vertexElementGroups = new List<MGEOVertexElementGroup>();
                uint vertexElementGroupCount = br.ReadUInt32();
                for(int i = 0; i < vertexElementGroupCount; i++)
                {
                    vertexElementGroups.Add(new MGEOVertexElementGroup(br));
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
                    this.Objects.Add(new MGEOObject(br, vertexElementGroups, vertexBufferOffsets, indexBuffers, useSeparatePointLights, version));
                }

                this.BucketGeometry = new MGEOBucketGeometry(br);
            }
        }

        public void Write(string fileLocation, uint version)
        {
            Write(File.Create(fileLocation), version);
        }

        public void Write(Stream stream, uint version)
        {
            if(version != 6 && version != 7)
            {
                throw new Exception("Unsupported version");
            }

            using (BinaryWriter bw = new BinaryWriter(stream))
            {
                bw.Write(Encoding.ASCII.GetBytes("OEGM"));
                bw.Write(version);

                bool usesSeparatePointLights = false;
                if(version != 7)
                {
                    usesSeparatePointLights = UsesSeparatePointLights();
                    bw.Write(usesSeparatePointLights);
                }

                List<MGEOVertexElementGroup> vertexElementGroups = GenerateVertexElementGroups();
                bw.Write(vertexElementGroups.Count);
                foreach(MGEOVertexElementGroup vertexElementGroup in vertexElementGroups)
                {
                    vertexElementGroup.Write(bw);
                }

                List<float[]> vertexBuffers = GenerateVertexBuffers(vertexElementGroups);
                bw.Write(vertexBuffers.Count);
                foreach(float[] vertexBuffer in vertexBuffers)
                {
                    bw.Write(vertexBuffer.Length * 4);

                    for(int i = 0; i < vertexBuffer.Length; i++)
                    {
                        bw.Write(vertexBuffer[i]);
                    }
                }

                List<ushort[]> indexBuffers = GenerateIndexBuffers();
                bw.Write(indexBuffers.Count);
                foreach(ushort[] indexBuffer in indexBuffers)
                {
                    bw.Write(indexBuffer.Length * 2);

                    for (int i = 0; i < indexBuffer.Length; i++)
                    {
                        bw.Write(indexBuffer[i]);
                    }
                }

                bw.Write(this.Objects.Count);
                foreach(MGEOObject model in this.Objects)
                {
                    model.Write(bw, usesSeparatePointLights, version);
                }

                this.BucketGeometry.Write(bw);
            }
        }

        private bool UsesSeparatePointLights()
        {
            foreach(MGEOObject model in this.Objects)
            {
                if (model.SeparatePointLight != null)
                {
                    return true;
                }
            }

            return false;
        }

        private List<MGEOVertexElementGroup> GenerateVertexElementGroups()
        {
            List<MGEOVertexElementGroup> vertexElementGroups = new List<MGEOVertexElementGroup>();

            foreach(MGEOObject model in this.Objects)
            {
                MGEOVertexElementGroup vertexElementGroup = new MGEOVertexElementGroup(model.Vertices[0]);

                if(!vertexElementGroups.Contains(vertexElementGroup))
                {
                    vertexElementGroups.Add(vertexElementGroup);
                }

                model._vertexElementGroupID = vertexElementGroups.IndexOf(vertexElementGroup);
            }

            return vertexElementGroups;
        }

        private List<float[]> GenerateVertexBuffers(List<MGEOVertexElementGroup> vertexElementGroups)
        {
            List<float[]> vertexBuffers = new List<float[]>();
            int vertexBufferID = 0;

            foreach(MGEOObject model in this.Objects)
            {
                uint vertexSize = vertexElementGroups[model._vertexElementGroupID].GetVertexSize();
                float[] vertexBuffer = new float[(vertexSize / 4) * model.Vertices.Count];

                for(int i = 0; i < model.Vertices.Count; i++)
                {
                    float[] vertexArray = model.Vertices[i].ToFloatArray(vertexSize);
                    Array.Copy(vertexArray, 0, vertexBuffer, (vertexSize / 4) * i, vertexArray.Length);
                }

                vertexBuffers.Add(vertexBuffer);
                model._vertexBufferID = vertexBufferID;
                vertexBufferID++;
            }

            return vertexBuffers;
        }

        private List<ushort[]> GenerateIndexBuffers()
        {
            List<ushort[]> indexBuffers = new List<ushort[]>();
            int indexBufferID = 0;

            foreach(MGEOObject model in this.Objects)
            {
                indexBuffers.Add(model.Indices.ToArray());

                model._indexBufferID = indexBufferID;
                indexBufferID++;
            }

            return indexBuffers;
        }
    }
}
