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
        public string BakedTerrainPrimarySampler { get; set; } = string.Empty;
        public string BakedTerrainSecondarySampler { get; set; } = string.Empty;
        public List<MapGeometryModel> Models { get; set; } = new();
        public BucketGrid BucketGrid { get; set; }
        public List<MapGeometryPlanarReflector> PlanarReflectors { get; set; } = new();

        public MapGeometry(string fileLocation) : this(File.OpenRead(fileLocation)) { }

        public MapGeometry(Stream stream)
        {
            using BinaryReader br = new(stream);

            string magic = Encoding.ASCII.GetString(br.ReadBytes(4));
            if (magic != "OEGM")
            {
                throw new InvalidFileSignatureException();
            }

            uint version = br.ReadUInt32();
            if (version is not (5 or 6 or 7 or 9 or 11 or 12 or 13))
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
                this.BakedTerrainPrimarySampler = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));

                if (version >= 11)
                {
                    this.BakedTerrainSecondarySampler = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
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
                if (version >= 13)
                {
                    MapGeometryLayer layers = (MapGeometryLayer)br.ReadByte();
                }

                uint bufferSize = br.ReadUInt32();

                vertexBufferOffsets.Add(br.BaseStream.Position);
                br.BaseStream.Seek(bufferSize, SeekOrigin.Current);
            }

            uint indexBufferCount = br.ReadUInt32();
            List<ushort[]> indexBuffers = new();
            for (int i = 0; i < indexBufferCount; i++)
            {
                if (version >= 13)
                {
                    MapGeometryLayer layers = (MapGeometryLayer)br.ReadByte();
                }

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
                this.Models.Add(
                    new(br, vertexElementGroups, vertexBufferOffsets, indexBuffers, useSeparatePointLights, version)
                );
            }

            this.BucketGrid = new(br);

            if (version >= 13)
            {
                uint planarReflectorCount = br.ReadUInt32();
                for (int i = 0; i < planarReflectorCount; i++)
                {
                    this.PlanarReflectors.Add(new(br));
                }
            }
        }

        public void Write(string fileLocation, uint version)
        {
            Write(File.Create(fileLocation), version);
        }

        public void Write(Stream stream, uint version, bool leaveOpen = false)
        {
            if (version is not (5 or 6 or 7 or 9 or 11 or 12 or 13))
            {
                throw new Exception("Unsupported version");
            }

            using BinaryWriter bw = new(stream, Encoding.UTF8, leaveOpen);

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
                bw.Write(this.BakedTerrainPrimarySampler.Length);
                bw.Write(Encoding.ASCII.GetBytes(this.BakedTerrainPrimarySampler));

                if (version >= 11)
                {
                    bw.Write(this.BakedTerrainSecondarySampler.Length);
                    bw.Write(Encoding.ASCII.GetBytes(this.BakedTerrainSecondarySampler));
                }
            }

            List<MapGeometryVertexElementGroup> vertexElementGroups = GenerateVertexElementGroups();
            bw.Write(vertexElementGroups.Count);
            foreach (MapGeometryVertexElementGroup vertexElementGroup in vertexElementGroups)
            {
                vertexElementGroup.Write(bw);
            }

            WriteVertexBuffers(bw, vertexElementGroups, version);

            List<(MapGeometryLayer, ushort[])> indexBuffers = GenerateIndexBuffers();
            bw.Write(indexBuffers.Count);
            foreach ((MapGeometryLayer layers, ushort[] indexBuffer) in indexBuffers)
            {
                if (version >= 13)
                {
                    bw.Write((byte)layers);
                }

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

            if (version >= 13)
            {
                bw.Write(this.PlanarReflectors.Count);
                foreach (MapGeometryPlanarReflector planarReflector in this.PlanarReflectors)
                {
                    planarReflector.Write(bw);
                }
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
            List<MapGeometryVertexElementGroup> vertexElementGroups = new();

            foreach (MapGeometryModel model in this.Models)
            {
                MapGeometryVertexElementGroup vertexElementGroup = new(model.Vertices[0]);

                if (!vertexElementGroups.Contains(vertexElementGroup))
                {
                    vertexElementGroups.Add(vertexElementGroup);
                }

                model._vertexElementGroupId = vertexElementGroups.IndexOf(vertexElementGroup);
            }

            return vertexElementGroups;
        }

        private void WriteVertexBuffers(
            BinaryWriter bw,
            List<MapGeometryVertexElementGroup> vertexElementGroups,
            uint version
        )
        {
            // Write count of buffers
            bw.Write(this.Models.Count);

            int currentVertexBufferId = 0;
            foreach (MapGeometryModel model in this.Models)
            {
                int vertexSize = vertexElementGroups[model._vertexElementGroupId].GetVertexSize();
                int vertexBufferSize = model.Vertices.Count * vertexSize;

                // Write buffer layer mask
                if (version >= 13)
                {
                    bw.Write((byte)model.LayerMask);
                }

                // Write buffer size
                bw.Write(vertexBufferSize);

                // Write buffer data
                foreach (MapGeometryVertex vertex in model.Vertices)
                {
                    vertex.Write(bw);
                }

                model._vertexBufferId = currentVertexBufferId;

                currentVertexBufferId++;
            }
        }

        private List<(MapGeometryLayer, ushort[])> GenerateIndexBuffers()
        {
            List<(MapGeometryLayer, ushort[])> indexBuffers = new();
            int indexBufferId = 0;

            foreach (MapGeometryModel model in this.Models)
            {
                indexBuffers.Add((model.LayerMask, model.Indices.ToArray()));

                model._indexBufferId = indexBufferId;
                indexBufferId++;
            }

            return indexBuffers;
        }
    }
}
