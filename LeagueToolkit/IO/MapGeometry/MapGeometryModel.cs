using LeagueToolkit.Helpers.Extensions;
using LeagueToolkit.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace LeagueToolkit.IO.MapGeometry
{
    public class MapGeometryModel
    {
        public string Name { get; private set; }

        public ReadOnlySpan<MapGeometryVertex> Vertices => this._vertices;
        public ReadOnlySpan<ushort> Indices => this._indices;

        private readonly MapGeometryVertex[] _vertices;
        private readonly ushort[] _indices;

        public ReadOnlyCollection<MapGeometrySubmesh> Submeshes => this._submeshes.AsReadOnly();
        private readonly List<MapGeometrySubmesh> _submeshes = new();

        public bool FlipNormals { get; private set; }

        public Box BoundingBox { get; private set; }
        public Matrix4x4 Transformation { get; private set; }

        public MapGeometryQualityFilter QualityFilter { get; private set; } = MapGeometryQualityFilter.QualityAll;
        public MapGeometryLayer LayerMask { get; private set; } = MapGeometryLayer.AllLayers;
        public MapGeometryMeshRenderFlags MeshRenderFlags { get; private set; }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// This feature is supported only up until version 7
        /// </remarks>
        public Vector3? PointLight { get; private set; }

        /// <summary>
        /// For more information about generating light probes,
        /// see <see href="https://docs.unity3d.com/Manual/LightProbes-TechnicalInformation.html">Unity - Light Probes</see>
        /// </summary>
        /// <remarks>
        /// This feature is supported only up until version 9
        /// <br>Since version 9, terrain meshes use baked light instead</br>
        /// </remarks>
        public ReadOnlyCollection<Vector3> LightProbes => Array.AsReadOnly(this._lightProbes);
        private readonly Vector3[] _lightProbes;

        public MapGeometrySamplerData StationaryLight { get; private set; } = new();

        public MapGeometrySamplerData BakedLight { get; private set; } = new();

        public MapGeometrySamplerData BakedPaint { get; private set; } = new();

        public const uint MAX_SUBMESH_COUNT = 64;

        internal int _vertexElementGroupId;
        internal int _vertexBufferId;
        internal int _indexBufferId;

        public MapGeometryModel() { }

        public MapGeometryModel(
            string name,
            IEnumerable<MapGeometryVertex> vertices,
            ushort[] indices,
            IEnumerable<MapGeometrySubmesh> submeshes
        )
        {
            this.Name = name;
            this._vertices = vertices.ToArray();
            this._indices = indices;
            this._submeshes = new(submeshes);

            this.BoundingBox = Box.FromVertices(vertices.Select(vertex => vertex.Position ?? Vector3.Zero));
        }

        public MapGeometryModel(
            string name,
            IEnumerable<MapGeometryVertex> vertices,
            ushort[] indices,
            IEnumerable<MapGeometrySubmesh> submeshes,
            MapGeometryLayer layer
        ) : this(name, vertices, indices, submeshes)
        {
            this.LayerMask = layer;
        }

        public MapGeometryModel(
            string name,
            IEnumerable<MapGeometryVertex> vertices,
            ushort[] indices,
            IEnumerable<MapGeometrySubmesh> submeshes,
            Matrix4x4 transformation
        ) : this(name, vertices, indices, submeshes)
        {
            this.Transformation = transformation;
        }

        public MapGeometryModel(
            string name,
            IEnumerable<MapGeometryVertex> vertices,
            ushort[] indices,
            IEnumerable<MapGeometrySubmesh> submeshes,
            MapGeometryLayer layer,
            Matrix4x4 transformation
        ) : this(name, vertices, indices, submeshes)
        {
            this.LayerMask = layer;
            this.Transformation = transformation;
        }

        public MapGeometryModel(
            string name,
            IEnumerable<MapGeometryVertex> vertices,
            ushort[] indices,
            IEnumerable<MapGeometrySubmesh> submeshes,
            MapGeometrySamplerData stationaryLight
        ) : this(name, vertices, indices, submeshes)
        {
            this.StationaryLight = stationaryLight;
        }

        internal MapGeometryModel(
            BinaryReader br,
            List<MapGeometryVertexElementGroup> vertexElementGroups,
            List<long> vertexBufferOffsets,
            List<ushort[]> indexBuffers,
            bool useSeparatePointLights,
            uint version
        )
        {
            if (version <= 11)
            {
                this.Name = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
            }

            int vertexCount = br.ReadInt32();
            uint vertexBufferCount = br.ReadUInt32();
            int baseVertexElementGroup = br.ReadInt32();

            // Pre-allocate mesh vertex buffer
            this._vertices = new MapGeometryVertex[vertexCount];
            for (int i = 0; i < vertexCount; i++)
            {
                this._vertices[i] = new();
            }
            for (int i = 0; i < vertexBufferCount; i++)
            {
                int vertexBufferId = br.ReadInt32();
                long returnPosition = br.BaseStream.Position;
                br.BaseStream.Seek(vertexBufferOffsets[vertexBufferId], SeekOrigin.Begin);

                for (int j = 0; j < vertexCount; j++)
                {
                    MapGeometryVertex.ReadAndCombineElements(
                        this._vertices[j],
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
                this.LayerMask = (MapGeometryLayer)br.ReadByte();
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
            this.Transformation = br.ReadMatrix4x4RowMajor();
            this.QualityFilter = (MapGeometryQualityFilter)br.ReadByte();

            if (version >= 7 && version <= 12)
            {
                this.LayerMask = (MapGeometryLayer)br.ReadByte();
            }

            if (version >= 11)
            {
                this.MeshRenderFlags = (MapGeometryMeshRenderFlags)br.ReadByte();
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
                bw.Write((byte)this.LayerMask);
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
            bw.WriteMatrix4x4RowMajor(this.Transformation);
            bw.Write((byte)this.QualityFilter);

            if (version >= 7 && version <= 12)
            {
                bw.Write((byte)this.LayerMask);
            }

            if (version >= 11)
            {
                bw.Write((byte)this.MeshRenderFlags);
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
    }

    [Flags]
    public enum MapGeometryLayer : byte
    {
        NoLayer = 0,
        Layer1 = 1,
        Layer2 = 2,
        Layer3 = 4,
        Layer4 = 8,
        Layer5 = 16,
        Layer6 = 32,
        Layer7 = 64,
        Layer8 = 128,
        AllLayers = 255
    }

    [Flags]
    public enum MapGeometryQualityFilter : byte
    {
        Quality0 = 1,
        Quality1 = 2,
        Quality2 = 4,
        Quality3 = 8,
        Quality4 = 16,

        QualityAll = Quality0 | Quality1 | Quality2 | Quality3 | Quality4
    }

    [Flags]
    public enum MapGeometryMeshRenderFlags : byte
    {
        /// <summary>
        /// Mesh will have a higher render priority which causes it to be rendered
        /// on top of certain meshes such as particles with the following properties:
        /// <code>miscRenderFlags: u8 = 1 || isGroundLayer: flag = true || useNavmeshMask: flag = true</code>
        /// </summary>
        HighRenderPriority = 1,
        UnknownConstructDistortionBuffer = 2,

        /// <summary>
        /// Mesh will be rendered only if "Hide Eye Candy" option is unchecked
        /// </summary>
        RenderOnlyIfEyeCandyOn = 4, // (meshTypeFlags & 4) == 0 || envSettingsFlags)

        /// <summary>
        /// Mesh will be rendered only if "Hide Eye Candy" option is checked
        /// </summary>
        RenderOnlyIfEyeCandyOff = 8 // ((meshTypeFlags & 8) == 0 || envSettingsFlags != 1)
    }
}
