using CommunityToolkit.Diagnostics;
using LeagueToolkit.Helpers.Exceptions;
using LeagueToolkit.Helpers.Structures.BucketGrid;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace LeagueToolkit.IO.MapGeometry
{
    public class MapGeometry
    {
        public MapGeometryBakedTerrainSamplers BakedTerrainSamplers { get; private set; }

        public IReadOnlyList<MapGeometryModel> Meshes => this._meshes;
        private readonly List<MapGeometryModel> _meshes = new();

        public BucketGrid BucketGrid { get; set; }

        public IReadOnlyList<MapGeometryPlanarReflector> PlanarReflectors => this._planarReflectors;
        private readonly List<MapGeometryPlanarReflector> _planarReflectors = new();

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

            ReadBakedTerrainSamplers(br, version);

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
                this._meshes.Add(
                    new(br, i, vertexElementGroups, vertexBufferOffsets, indexBuffers, useSeparatePointLights, version)
                );
            }

            this.BucketGrid = new(br);

            if (version >= 13)
            {
                uint planarReflectorCount = br.ReadUInt32();
                for (int i = 0; i < planarReflectorCount; i++)
                {
                    this._planarReflectors.Add(new(br));
                }
            }
        }

        internal MapGeometry(
            MapGeometryBakedTerrainSamplers bakedTerrainSamplers,
            IEnumerable<MapGeometryModel> meshes,
            BucketGrid bucketGrid,
            IEnumerable<MapGeometryPlanarReflector> planarReflectors
        )
        {
            Guard.IsNotNull(meshes, nameof(meshes));
            Guard.IsNotNull(bucketGrid, nameof(bucketGrid));
            Guard.IsNotNull(planarReflectors, nameof(planarReflectors));

            this.BakedTerrainSamplers = bakedTerrainSamplers;
            this._meshes = new(meshes);
            this.BucketGrid = bucketGrid;
            this._planarReflectors = new(planarReflectors);
        }

        internal void ReadBakedTerrainSamplers(BinaryReader br, uint version)
        {
            MapGeometryBakedTerrainSamplers bakedTerrainSamplers = new();
            if (version >= 9)
            {
                bakedTerrainSamplers.Primary = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));

                if (version >= 11)
                {
                    bakedTerrainSamplers.Secondary = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
                }
            }

            this.BakedTerrainSamplers = bakedTerrainSamplers;
        }

        public void Write(string fileLocation, uint version)
        {
            Write(File.Create(fileLocation), version);
        }

        public void Write(Stream stream, uint version, bool leaveOpen = false)
        {
            if (version is not (5 or 6 or 7 or 9 or 11 or 12 or 13))
            {
                throw new ArgumentException($"Unsupported version: {version}", nameof(version));
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

            WriteBakedTerrainSamplers(bw, version);

            List<MapGeometryVertexElementGroup> vertexElementGroups = GenerateVertexElementGroups();
            bw.Write(vertexElementGroups.Count);
            foreach (MapGeometryVertexElementGroup vertexElementGroup in vertexElementGroups)
            {
                vertexElementGroup.Write(bw);
            }

            WriteVertexBuffers(bw, vertexElementGroups, version);
            WriteIndexBuffers(bw, version);

            bw.Write(this._meshes.Count);
            foreach (MapGeometryModel model in this._meshes)
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

        private bool UsesSeparatePointLights()
        {
            foreach (MapGeometryModel mesh in this.Meshes)
            {
                if (mesh.PointLight is not null)
                {
                    return true;
                }
            }

            return false;
        }

        private List<MapGeometryVertexElementGroup> GenerateVertexElementGroups()
        {
            List<MapGeometryVertexElementGroup> vertexElementGroups = new();

            foreach (MapGeometryModel mesh in this.Meshes)
            {
                // TODO: Create method which verifies that each vertex is of the same format
                //       and throws if it detects an inconsistency
                MapGeometryVertexElementGroup vertexElementGroup = new(mesh.Vertices[0]);

                if (!vertexElementGroups.Contains(vertexElementGroup))
                {
                    vertexElementGroups.Add(vertexElementGroup);
                }

                mesh._vertexElementGroupId = vertexElementGroups.IndexOf(vertexElementGroup);
            }

            return vertexElementGroups;
        }

        private void WriteBakedTerrainSamplers(BinaryWriter bw, uint version)
        {
            if (version >= 9)
            {
                WriteSampler(this.BakedTerrainSamplers.Primary);

                if (version >= 11)
                {
                    WriteSampler(this.BakedTerrainSamplers.Secondary);
                }
            }

            void WriteSampler(string sampler)
            {
                if (string.IsNullOrEmpty(sampler))
                {
                    bw.Write(0); // Length
                }
                else
                {
                    bw.Write(sampler.Length);
                    bw.Write(Encoding.ASCII.GetBytes(sampler));
                }
            }
        }

        private void WriteVertexBuffers(
            BinaryWriter bw,
            List<MapGeometryVertexElementGroup> vertexElementGroups,
            uint version
        )
        {
            // Write count of buffers
            bw.Write(this.Meshes.Count);

            int currentVertexBufferId = 0;
            foreach (MapGeometryModel mesh in this.Meshes)
            {
                int vertexSize = vertexElementGroups[mesh._vertexElementGroupId].GetVertexSize();
                int vertexBufferSize = mesh.Vertices.Length * vertexSize;

                // Write buffer layer mask
                if (version >= 13)
                {
                    bw.Write((byte)mesh.LayerMask);
                }

                // Write buffer size
                bw.Write(vertexBufferSize);

                // Write buffer data
                for (int currentVertex = 0; currentVertex < mesh.Vertices.Length; currentVertex++)
                {
                    mesh.Vertices[currentVertex].Write(bw);
                }

                mesh._vertexBufferId = currentVertexBufferId;

                currentVertexBufferId++;
            }
        }

        private void WriteIndexBuffers(BinaryWriter bw, uint version)
        {
            // Write count of buffers
            bw.Write(this.Meshes.Count);

            int currentIndexBufferId = 0;
            foreach (MapGeometryModel mesh in this.Meshes)
            {
                // Marshal Indices array of mesh into a buffer for writing into the stream
                ReadOnlySpan<byte> indexBuffer = MemoryMarshal.AsBytes(mesh.Indices);

                // Write buffer layer mask
                if (version >= 13)
                {
                    bw.Write((byte)mesh.LayerMask);
                }

                // Write size of buffer and the buffer itself
                bw.Write(indexBuffer.Length);
                bw.Write(indexBuffer);

                mesh._indexBufferId = currentIndexBufferId;
                currentIndexBufferId++;
            }
        }
    }
}
