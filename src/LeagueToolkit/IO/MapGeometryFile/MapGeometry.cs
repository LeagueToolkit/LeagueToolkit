using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance.Buffers;
using LeagueToolkit.Core.Memory;
using LeagueToolkit.Helpers.Exceptions;
using LeagueToolkit.Helpers.Extensions;
using LeagueToolkit.Helpers.Structures.BucketGrid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace LeagueToolkit.IO.MapGeometryFile
{
    /// <summary>
    /// Contains the evironment asset geometry
    /// </summary>
    public sealed class MapGeometry : IDisposable
    {
        /// <value>
        /// Represents the baked terrain samplers used for this environment asset
        /// </value>
        public MapGeometryBakedTerrainSamplers BakedTerrainSamplers { get; private set; }

        /// <value>
        /// Represents the meshes used by this environment asset
        /// </value>
        public IReadOnlyList<MapGeometryModel> Meshes => this._meshes;
        private readonly List<MapGeometryModel> _meshes = new();

        /// <value>
        /// Represents the bucketed scene graph for this environment asset
        /// </value>
        public BucketGrid BucketGrid { get; private set; }

        /// <value>
        /// Represents the planar reflectors used by this environment asset
        /// </value>
        public IReadOnlyList<MapGeometryPlanarReflector> PlanarReflectors => this._planarReflectors;
        private readonly List<MapGeometryPlanarReflector> _planarReflectors = new();

        /// <summary>
        /// Creates a new <see cref="MapGeometry"/> instance by reading it from <paramref name="fileLocation"/>
        /// </summary>
        /// <param name="fileLocation">The file to read from</param>
        public MapGeometry(string fileLocation) : this(File.OpenRead(fileLocation)) { }

        /// <summary>
        /// Creates a new <see cref="MapGeometry"/> instance by reading it from <paramref name="stream"/>
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read from</param>
        /// <exception cref="InvalidFileSignatureException">The header magic signature is invalid</exception>
        /// <exception cref="UnsupportedFileVersionException">The version of the <see cref="MapGeometry"/> file is not supported</exception>
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

            List<VertexBufferDescription> vertexElementGroups = new();
            uint vertexElementGroupCount = br.ReadUInt32();
            for (int i = 0; i < vertexElementGroupCount; i++)
            {
                vertexElementGroups.Add(VertexBufferDescription.ReadFromMapGeometry(br));
            }

            uint vertexBufferCount = br.ReadUInt32();
            List<long> vertexBufferOffsets = new();
            for (int i = 0; i < vertexBufferCount; i++)
            {
                if (version >= 13)
                {
                    MapGeometryVisibilityFlags visibilityFlags = (MapGeometryVisibilityFlags)br.ReadByte();
                }

                uint bufferSize = br.ReadUInt32();

                vertexBufferOffsets.Add(br.BaseStream.Position);
                br.BaseStream.Seek(bufferSize, SeekOrigin.Current);
            }

            uint indexBufferCount = br.ReadUInt32();
            List<MemoryOwner<ushort>> indexBuffers = new();
            for (int i = 0; i < indexBufferCount; i++)
            {
                if (version >= 13)
                {
                    MapGeometryVisibilityFlags visibilityFlags = (MapGeometryVisibilityFlags)br.ReadByte();
                }

                int bufferSize = br.ReadInt32();
                MemoryOwner<ushort> indexBuffer = MemoryOwner<ushort>.Allocate(bufferSize / 2);

                for (int j = 0; j < bufferSize / 2; j++)
                {
                    indexBuffer.Span[j] = br.ReadUInt16();
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

        /// <summary>
        /// Writes this <see cref="MapGeometry"/> instance into <paramref name="fileLocation"/> with the requested <paramref name="version"/>
        /// </summary>
        /// <param name="fileLocation">The file to write into</param>
        /// <param name="version">The version of the written <see cref="MapGeometry"/> file</param>
        public void Write(string fileLocation, uint version) => Write(File.Create(fileLocation), version);

        /// <summary>
        /// Writes this <see cref="MapGeometry"/> instance into <paramref name="stream"/>
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to write to</param>
        /// <param name="version">The version of the written <see cref="MapGeometry"/> file</param>
        /// <param name="leaveOpen">Whether the internal reader should leave <paramref name="stream"/> opened</param>
        /// <exception cref="ArgumentException"></exception>
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

            List<VertexBufferDescription> vertexElementGroups = GenerateVertexBufferDescriptions();
            bw.Write(vertexElementGroups.Count);
            foreach (VertexBufferDescription vertexElementGroup in vertexElementGroups)
            {
                vertexElementGroup.WriteToMapGeometry(bw);
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

        // TODO: Instanced Vertex Buffers
        private List<VertexBufferDescription> GenerateVertexBufferDescriptions()
        {
            List<VertexBufferDescription> descriptions = new();

            foreach (MapGeometryModel mesh in this.Meshes)
            {
                // Find base descriptor index, if it doesn't exist, create it
                IEnumerable<VertexBufferDescription> meshDescriptions = mesh.VertexData.Buffers.Select(
                    buffer => buffer.Description
                );
                int baseDescriptionId = descriptions.IndexOf(meshDescriptions);
                if (baseDescriptionId == -1)
                {
                    baseDescriptionId = descriptions.Count;

                    descriptions.AddRange(meshDescriptions);
                }

                mesh._baseDescriptionId = baseDescriptionId;
            }

            return descriptions;
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

        // TODO: Vertex Buffer instancing
        private void WriteVertexBuffers(
            BinaryWriter bw,
            List<VertexBufferDescription> vertexElementGroups,
            uint version
        )
        {
            // Write count of buffers
            bw.Write(this.Meshes.Count);

            int currentBaseVertexBufferId = 0;
            foreach (MapGeometryModel mesh in this.Meshes)
            {
                InstancedVertexBuffer vertexData = mesh.VertexData;

                // Write buffer layer mask
                if (version >= 13)
                {
                    bw.Write((byte)mesh.VisibilityFlags);
                }

                // Write buffer size
                bw.Write(vertexData.Buffers.Sum(vertexBuffer => vertexBuffer.View.Length));

                // Write buffer data
                foreach (VertexBuffer vertexBuffer in vertexData.Buffers)
                {
                    bw.Write(vertexBuffer.View.Span);
                }

                mesh._vertexBufferIds = Enumerable.Range(currentBaseVertexBufferId, vertexData.Buffers.Count).ToArray();

                currentBaseVertexBufferId += vertexData.Buffers.Count;
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
                    bw.Write((byte)mesh.VisibilityFlags);
                }

                // Write size of buffer and the buffer itself
                bw.Write(indexBuffer.Length);
                bw.Write(indexBuffer);

                mesh._indexBufferId = currentIndexBufferId;
                currentIndexBufferId++;
            }
        }

        public void Dispose()
        {
            if (this._meshes is null)
            {
                return;
            }

            foreach (MapGeometryModel mesh in this._meshes)
            {
                mesh?.Dispose();
            }

            GC.SuppressFinalize(this);
        }
    }
}
