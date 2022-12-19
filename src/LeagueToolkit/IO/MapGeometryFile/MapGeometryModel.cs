using CommunityToolkit.HighPerformance.Buffers;
using LeagueToolkit.Helpers.Extensions;
using LeagueToolkit.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace LeagueToolkit.IO.MapGeometryFile
{
    public sealed class MapGeometryModel : IDisposable
    {
        /// <summary>
        /// The name of this mesh
        /// </summary>
        /// <remarks>
        /// This feature is supported only if <c>version &lt; 12</c>
        /// </remarks>
        public string Name { get; private set; }

        /// <summary>
        /// A read-only view into the serialized vertex buffer
        /// </summary>
        public ReadOnlySpan<MapGeometryVertex> Vertices => this._vertices.Span;

        /// <summary>
        /// A read-only view into the index buffer
        /// </summary>
        public ReadOnlySpan<ushort> Indices => this._indices.Span;

        private readonly MemoryOwner<MapGeometryVertex> _vertices;
        private readonly MemoryOwner<ushort> _indices;

        public IReadOnlyList<MapGeometrySubmesh> Submeshes => this._submeshes;
        private readonly List<MapGeometrySubmesh> _submeshes = new();

        /// <summary>
        /// Tells the game to flip the normals of this mesh
        /// </summary>
        public bool FlipNormals { get; private set; }

        public Box BoundingBox { get; private set; }
        public Matrix4x4 Transform { get; private set; }

        /// <summary>
        /// Tells the game on which "Environment Quality" settings this mesh should be rendered
        /// </summary>
        public MapGeometryEnvironmentQualityFilter EnvironmentQualityFilter { get; private set; } =
            MapGeometryEnvironmentQualityFilter.AllQualities;

        /// <summary>
        /// Tells the game on which Visibility Flags this mesh should be rendered
        /// </summary>
        public MapGeometryVisibilityFlags VisibilityFlags { get; private set; } = MapGeometryVisibilityFlags.AllLayers;
        public MapGeometryMeshRenderFlags RenderFlags { get; private set; }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// This feature is supported only if <c>version &lt; 7</c>
        /// </remarks>
        public Vector3? PointLight { get; private set; }

        /// <summary>
        /// For more information about generating light probes,
        /// see <see href="https://docs.unity3d.com/Manual/LightProbes-TechnicalInformation.html">Unity - Light Probes</see>
        /// </summary>
        /// <remarks>
        /// This feature is supported only if <c>version &lt; 9</c>
        /// <br>Since version 9, terrain meshes use baked light instead</br>
        /// </remarks>
        public IReadOnlyList<Vector3> LightProbes => this._lightProbes;
        private readonly Vector3[] _lightProbes;

        /// <summary>
        /// Information for the "STATIONARY_LIGHT" sampler
        /// </summary>
        /// <remarks>
        /// Usually contains a diffuse texture
        /// </remarks>
        public MapGeometrySamplerData StationaryLight { get; private set; }

        /// <summary>
        /// Information for the "BAKED_LIGHT" sampler
        /// </summary>
        /// <remarks>
        /// Usually contains a lightmap texture (baked from scene point lights)
        /// </remarks>
        public MapGeometrySamplerData BakedLight { get; private set; }

        /// <summary>
        /// Information for the "BAKED_PAINT" sampler
        /// </summary>
        /// <remarks>
        /// Usually contains a texture with baked diffuse and lightmap data
        /// </remarks>
        public MapGeometrySamplerData BakedPaint { get; private set; }

        public const uint MAX_SUBMESH_COUNT = 64;

        internal int _vertexElementGroupId;
        internal int _vertexBufferId;
        internal int _indexBufferId;

        internal MapGeometryModel(
            int id,
            MemoryOwner<MapGeometryVertex> vertices,
            MemoryOwner<ushort> indices,
            IEnumerable<MapGeometrySubmesh> submeshes,
            Matrix4x4 transform,
            bool flipNormals,
            MapGeometryEnvironmentQualityFilter environmentQualityFilter,
            MapGeometryVisibilityFlags visibilityFlags,
            MapGeometryMeshRenderFlags renderFlags,
            MapGeometrySamplerData stationaryLight,
            MapGeometrySamplerData bakedLight,
            MapGeometrySamplerData bakedPaint
        )
        {
            this.Name = CreateName(id);
            this._vertices = vertices;
            this._indices = indices;
            this._submeshes = new(submeshes);
            this.Transform = transform;
            this.FlipNormals = flipNormals;
            this.EnvironmentQualityFilter = environmentQualityFilter;
            this.VisibilityFlags = visibilityFlags;
            this.RenderFlags = renderFlags;
            this.StationaryLight = stationaryLight;
            this.BakedLight = bakedLight;
            this.BakedPaint = bakedPaint;

            this.BoundingBox = Box.FromVertices(TakeVertexPositions());

            IEnumerable<Vector3> TakeVertexPositions()
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    yield return vertices.Span[i].Position ?? Vector3.Zero;
                }
            }
        }

        internal MapGeometryModel(
            BinaryReader br,
            int id,
            List<MapGeometryVertexElementGroup> vertexElementGroups,
            List<long> vertexBufferOffsets,
            List<MemoryOwner<ushort>> indexBuffers,
            bool useSeparatePointLights,
            uint version
        )
        {
            this.Name = version <= 11 ? Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32())) : CreateName(id);

            int vertexCount = br.ReadInt32();
            uint vertexBufferCount = br.ReadUInt32();
            int baseVertexElementGroup = br.ReadInt32();

            this._vertices = MemoryOwner<MapGeometryVertex>.Allocate(vertexCount, AllocationMode.Clear);
            for (int i = 0; i < vertexBufferCount; i++)
            {
                int vertexBufferId = br.ReadInt32();
                long returnPosition = br.BaseStream.Position;
                br.BaseStream.Seek(vertexBufferOffsets[vertexBufferId], SeekOrigin.Begin);

                for (int j = 0; j < vertexCount; j++)
                {
                    MapGeometryVertex.ReadAndCombineElements(
                        ref this._vertices.Span[j],
                        vertexElementGroups[baseVertexElementGroup + i],
                        br
                    );
                }

                br.BaseStream.Seek(returnPosition, SeekOrigin.Begin);
            }

            uint indexCount = br.ReadUInt32();
            int indexBufferId = br.ReadInt32();
            this._indices = indexBuffers[indexBufferId];

            if (version >= 13)
            {
                this.VisibilityFlags = (MapGeometryVisibilityFlags)br.ReadByte();
            }

            uint submeshCount = br.ReadUInt32();
            for (int i = 0; i < submeshCount; i++)
            {
                this._submeshes.Add(new(br));
            }

            if (version != 5)
            {
                this.FlipNormals = br.ReadBoolean();
            }

            this.BoundingBox = br.ReadBox();
            this.Transform = br.ReadMatrix4x4RowMajor();
            this.EnvironmentQualityFilter = (MapGeometryEnvironmentQualityFilter)br.ReadByte();

            if (version >= 7 && version <= 12)
            {
                this.VisibilityFlags = (MapGeometryVisibilityFlags)br.ReadByte();
            }

            if (version >= 11)
            {
                this.RenderFlags = (MapGeometryMeshRenderFlags)br.ReadByte();
            }

            if (useSeparatePointLights && (version < 7))
            {
                this.PointLight = br.ReadVector3();
            }

            if (version < 9)
            {
                this._lightProbes = new Vector3[9];
                for (int i = 0; i < 9; i++)
                {
                    this._lightProbes[i] = br.ReadVector3();
                }

                this.StationaryLight = MapGeometrySamplerData.Read(br);
            }
            else
            {
                this.StationaryLight = MapGeometrySamplerData.Read(br);
                this.BakedLight = MapGeometrySamplerData.Read(br);

                if (version >= 12)
                {
                    this.BakedPaint = MapGeometrySamplerData.Read(br);
                }
            }
        }

        internal void Write(BinaryWriter bw, bool useSeparatePointLights, uint version)
        {
            if (version <= 11)
            {
                bw.Write(this.Name.Length);
                bw.Write(Encoding.ASCII.GetBytes(this.Name));
            }

            bw.Write(this._vertices.Length);
            bw.Write((uint)1);
            bw.Write(this._vertexElementGroupId);
            bw.Write(this._vertexBufferId); //we only have one vertex buffer

            bw.Write(this._indices.Length);
            bw.Write(this._indexBufferId);

            if (version >= 13)
            {
                bw.Write((byte)this.VisibilityFlags);
            }

            bw.Write(this._submeshes.Count);
            foreach (MapGeometrySubmesh submesh in this._submeshes)
            {
                submesh.Write(bw);
            }

            if (version != 5)
            {
                bw.Write(this.FlipNormals);
            }

            bw.WriteBox(this.BoundingBox);
            bw.WriteMatrix4x4RowMajor(this.Transform);
            bw.Write((byte)this.EnvironmentQualityFilter);

            if (version >= 7 && version <= 12)
            {
                bw.Write((byte)this.VisibilityFlags);
            }

            if (version >= 11)
            {
                bw.Write((byte)this.RenderFlags);
            }

            if (version < 9)
            {
                if (useSeparatePointLights)
                {
                    bw.WriteVector3(this.PointLight ?? Vector3.Zero);
                }

                foreach (Vector3 pointLight in this.LightProbes)
                {
                    bw.WriteVector3(pointLight);
                }
                for (int i = 0; i < 9 - this.LightProbes.Count; i++)
                {
                    bw.WriteVector3(Vector3.Zero);
                }

                this.StationaryLight.Write(bw);
            }
            else
            {
                this.StationaryLight.Write(bw);
                this.BakedLight.Write(bw);

                if (version >= 12)
                {
                    this.BakedPaint.Write(bw);
                }
            }
        }

        public static string CreateName(int id)
        {
            // League assigns this name to the meshes automatically during reading
            return $"MapGeo_Instance_{id}";
        }

        public void Dispose()
        {
            this._indices?.Dispose();
            this._vertices?.Dispose();

            GC.SuppressFinalize(this);
        }
    }

    [Flags]
    public enum MapGeometryVisibilityFlags : byte
    {
        NoLayer = 0,
        Layer1 = 1 << 0,
        Layer2 = 1 << 1,
        Layer3 = 1 << 2,
        Layer4 = 1 << 3,
        Layer5 = 1 << 4,
        Layer6 = 1 << 5,
        Layer7 = 1 << 6,
        Layer8 = 1 << 7,
        AllLayers = Layer1 | Layer2 | Layer3 | Layer4 | Layer5 | Layer6 | Layer7 | Layer8
    }

    [Flags]
    public enum MapGeometryEnvironmentQualityFilter : byte
    {
        VeryLow = 1 << 0,
        Low = 1 << 1,
        Medium = 1 << 2,
        High = 1 << 3,
        VeryHigh = 1 << 4,

        AllQualities = VeryLow | Low | Medium | High | VeryHigh
    }

    [Flags]
    public enum MapGeometryMeshRenderFlags : byte
    {
        /// <summary>
        /// Mesh will have a higher render priority which causes it to be rendered
        /// on top of certain meshes such as particles with the following properties:
        /// <code>miscRenderFlags: u8 = 1 || isGroundLayer: flag = true || useNavmeshMask: flag = true</code>
        /// </summary>
        HighRenderPriority = 1 << 0,
        UnknownConstructDistortionBuffer = 1 << 1,

        /// <summary>
        /// Mesh will be rendered only if "Hide Eye Candy" option is unchecked
        /// </summary>
        RenderOnlyIfEyeCandyOn = 1 << 2, // (meshTypeFlags & 4) == 0 || envSettingsFlags)

        /// <summary>
        /// Mesh will be rendered only if "Hide Eye Candy" option is checked
        /// </summary>
        RenderOnlyIfEyeCandyOff = 1 << 3 // ((meshTypeFlags & 8) == 0 || envSettingsFlags != 1)
    }
}
