using LeagueToolkit.Helpers.Exceptions;
using LeagueToolkit.Helpers.Structures.BucketGrid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LeagueToolkit.IO.MapGeometry
{
    public class MapGeometry
    {
        public string UnknownString1 { get; set; } = string.Empty;
        public string UnknownString2 { get; set; } = string.Empty;
        public List<MapGeometryModel> Models { get; set; } = new();
        public BucketGrid BucketGrid { get; set; }

        public MapGeometry(string fileLocation) : this(File.OpenRead(fileLocation)) { }
        public MapGeometry(Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream))
            {
                string magic = Encoding.ASCII.GetString(br.ReadBytes(4));
                if (magic != "OEGM")
                {
                    throw new InvalidFileSignatureException();
                }

                uint version = br.ReadUInt32();
                if (version != 5 && version != 6 && version != 7 && version != 9 && version != 11)
                {
                    throw new UnsupportedFileVersionException();
                }

                bool useSeparatePointLights = false;
                if (version < 7)
                {
                    useSeparatePointLights = br.ReadBoolean();
                }

                if (version >= 9)
                {
                    this.UnknownString1 = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));

                    if (version >= 11)
                    {
                        this.UnknownString2 = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
                    }
                }

                List<MapGeometryVertexElementGroup> vertexElementGroups = new();
                uint vertexElementGroupCount = br.ReadUInt32();
                for (int i = 0; i < vertexElementGroupCount; i++)
                {
                    vertexElementGroups.Add(new MapGeometryVertexElementGroup(br));
                }

                uint vertexBufferCount = br.ReadUInt32();
                List<long> vertexBufferOffsets = new();
                for (int i = 0; i < vertexBufferCount; i++)
                {
                    uint bufferSize = br.ReadUInt32();

                    vertexBufferOffsets.Add(br.BaseStream.Position);
                    br.BaseStream.Seek(bufferSize, SeekOrigin.Current);
                }

                uint indexBufferCount = br.ReadUInt32();
                List<ushort[]> indexBuffers = new();
                for (int i = 0; i < indexBufferCount; i++)
                {
                    uint bufferSize = br.ReadUInt32();
                    ushort[] indexBuffer = new ushort[bufferSize / 2];

                    for (int j = 0; j < bufferSize / 2; j++)
                    {
                        indexBuffer[j] = br.ReadUInt16();
                    }

                    indexBuffers.Add(indexBuffer);
                }

                uint modelCount = br.ReadUInt32();
                for (int i = 0; i < modelCount; i++)
                {
                    this.Models.Add(new MapGeometryModel(br, vertexElementGroups, vertexBufferOffsets, indexBuffers, useSeparatePointLights, version));
                }

                this.BucketGrid = new BucketGrid(br);
            }
        }

        public void Write(string fileLocation, uint version)
        {
            Write(File.Create(fileLocation), version);
        }
        public void Write(Stream stream, uint version, bool leaveOpen = false)
        {
            if (version != 5 && version != 6 && version != 7 && version != 9 && version != 11)
            {
                throw new Exception("Unsupported version");
            }

            using (BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8, leaveOpen))
            {
                bw.Write(Encoding.ASCII.GetBytes("OEGM"));
                bw.Write(version);

                bool usesSeparatePointLights = false;
                if (version < 7)
                {
                    usesSeparatePointLights = UsesSeparatePointLights();
                    bw.Write(usesSeparatePointLights);
                }

                if (version >= 9)
                {
                    bw.Write(this.UnknownString1.Length);
                    bw.Write(Encoding.ASCII.GetBytes(this.UnknownString1));

                    if (version >= 11)
                    {
                        bw.Write(this.UnknownString2.Length);
                        bw.Write(Encoding.ASCII.GetBytes(this.UnknownString2));
                    }
                }

                List<MapGeometryVertexElementGroup> vertexElementGroups = GenerateVertexElementGroups();
                bw.Write(vertexElementGroups.Count);
                foreach (MapGeometryVertexElementGroup vertexElementGroup in vertexElementGroups)
                {
                    vertexElementGroup.Write(bw);
                }

                List<byte[]> vertexBuffers = GenerateVertexBuffers(vertexElementGroups);
                bw.Write(vertexBuffers.Count);
                foreach (byte[] vertexBuffer in vertexBuffers)
                {
                    bw.Write(vertexBuffer.Length);
                    bw.Write(vertexBuffer);
                }

                List<ushort[]> indexBuffers = GenerateIndexBuffers();
                bw.Write(indexBuffers.Count);
                foreach (ushort[] indexBuffer in indexBuffers)
                {
                    bw.Write(indexBuffer.Length * 2);

                    for (int i = 0; i < indexBuffer.Length; i++)
                    {
                        bw.Write(indexBuffer[i]);
                    }
                }

                bw.Write(this.Models.Count);
                foreach (MapGeometryModel model in this.Models)
                {
                    model.Write(bw, usesSeparatePointLights, version);
                }

                this.BucketGrid.Write(bw);
            }
        }

        public void AddModel(MapGeometryModel model)
        {
            this.Models.Add(model);
        }

        private bool UsesSeparatePointLights()
        {
            foreach (MapGeometryModel model in this.Models)
            {
                if (model.SeparatePointLight != null)
                {
                    return true;
                }
            }

            return false;
        }

        private List<MapGeometryVertexElementGroup> GenerateVertexElementGroups()
        {
            List<MapGeometryVertexElementGroup> vertexElementGroups = new List<MapGeometryVertexElementGroup>();

            foreach (MapGeometryModel model in this.Models)
            {
                MapGeometryVertexElementGroup vertexElementGroup = new MapGeometryVertexElementGroup(model.Vertices[0]);

                if (!vertexElementGroups.Contains(vertexElementGroup))
                {
                    vertexElementGroups.Add(vertexElementGroup);
                }

                model._vertexElementGroupID = vertexElementGroups.IndexOf(vertexElementGroup);
            }

            return vertexElementGroups;
        }
        private List<byte[]> GenerateVertexBuffers(List<MapGeometryVertexElementGroup> vertexElementGroups)
        {
            List<byte[]> vertexBuffers = new List<byte[]>();
            int vertexBufferID = 0;

            foreach (MapGeometryModel model in this.Models)
            {
                int vertexSize = vertexElementGroups[model._vertexElementGroupID].GetVertexSize();
                byte[] vertexBuffer = new byte[vertexSize * model.Vertices.Count];

                for (int i = 0; i < model.Vertices.Count; i++)
                {
                    byte[] vertexElementsBuffer = model.Vertices[i].ToArray(vertexSize);
                    Buffer.BlockCopy(vertexElementsBuffer, 0, vertexBuffer, i * vertexSize, vertexElementsBuffer.Length);
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

            foreach (MapGeometryModel model in this.Models)
            {
                indexBuffers.Add(model.Indices.ToArray());

                model._indexBufferID = indexBufferID;
                indexBufferID++;
            }

            return indexBuffers;
        }
    }
}
