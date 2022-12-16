using LeagueToolkit.Helpers.Extensions;
using LeagueToolkit.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace LeagueToolkit.IO.MapGeometry
{
    public class MapGeometryModel
    {
        public string Name { get; set; }

        public List<MapGeometryVertex> Vertices { get; set; } = new();
        public List<ushort> Indices { get; set; } = new();
        public List<MapGeometrySubmesh> Submeshes { get; set; } = new();

        public bool FlipNormals { get; set; }

        public Box BoundingBox { get; set; }
        public Matrix4x4 Transformation { get; set; } = Matrix4x4.Identity;

        public MapGeometryQualityFilter QualityFilter { get; set; } = MapGeometryQualityFilter.QualityAll;
        public MapGeometryLayer Layer { get; set; } = MapGeometryLayer.AllLayers;
        public MapGeometryMeshRenderFlags MeshRenderFlags { get; set; }

        public Vector3? SeparatePointLight { get; set; }
        public List<Vector3> UnknownFloats { get; set; } = new();

        public MapGeometrySamplerData StationaryLight { get; set; } = new();

        public MapGeometrySamplerData BakedLight { get; set; } = new();

        public MapGeometrySamplerData BakedPaint { get; set; } = new();

        public const uint MAX_SUBMESH_COUNT = 64;

        internal int _vertexElementGroupId;
        internal int _vertexBufferId;
        internal int _indexBufferId;

        public MapGeometryModel() { }

        public MapGeometryModel(
            string name,
            List<MapGeometryVertex> vertices,
            List<ushort> indices,
            List<MapGeometrySubmesh> submeshes
        )
        {
            this.Name = name;
            this.Vertices = vertices;
            this.Indices = indices;
            this.Submeshes = submeshes;

            foreach (MapGeometrySubmesh submesh in submeshes)
            {
                submesh.Parent = this;
            }

            this.BoundingBox = Box.FromVertices(vertices.Select(vertex => vertex.Position ?? Vector3.Zero));
        }

        public MapGeometryModel(
            string name,
            List<MapGeometryVertex> vertices,
            List<ushort> indices,
            List<MapGeometrySubmesh> submeshes,
            MapGeometryLayer layer
        ) : this(name, vertices, indices, submeshes)
        {
            this.Layer = layer;
        }

        public MapGeometryModel(
            string name,
            List<MapGeometryVertex> vertices,
            List<ushort> indices,
            List<MapGeometrySubmesh> submeshes,
            Matrix4x4 transformation
        ) : this(name, vertices, indices, submeshes)
        {
            this.Transformation = transformation;
        }

        public MapGeometryModel(
            string name,
            List<MapGeometryVertex> vertices,
            List<ushort> indices,
            List<MapGeometrySubmesh> submeshes,
            MapGeometryLayer layer,
            Matrix4x4 transformation
        ) : this(name, vertices, indices, submeshes)
        {
            this.Layer = layer;
            this.Transformation = transformation;
        }

        public MapGeometryModel(
            string name,
            List<MapGeometryVertex> vertices,
            List<ushort> indices,
            List<MapGeometrySubmesh> submeshes,
            MapGeometrySamplerData stationaryLight
        ) : this(name, vertices, indices, submeshes)
        {
            this.StationaryLight = stationaryLight;
        }

        public MapGeometryModel(
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

            //if(vertexBufferCount != 1)
            //{
            //
            //}

            // Pre-allocate mesh vertex list
            this.Vertices = new(vertexCount);
            for (int i = 0; i < vertexCount; i++)
            {
                this.Vertices.Add(new());
            }
            for (int i = 0; i < vertexBufferCount; i++)
            {
                int vertexBufferId = br.ReadInt32();
                long returnPosition = br.BaseStream.Position;
                br.BaseStream.Seek(vertexBufferOffsets[vertexBufferId], SeekOrigin.Begin);

                for (int j = 0; j < vertexCount; j++)
                {
                    MapGeometryVertex.ReadAndCombineElements(
                        this.Vertices[j],
                        vertexElementGroups[baseVertexElementGroup + i],
                        br
                    );
                }

                br.BaseStream.Seek(returnPosition, SeekOrigin.Begin);
            }

            uint indexCount = br.ReadUInt32();
            int indexBufferId = br.ReadInt32();
            this.Indices.AddRange(indexBuffers[indexBufferId]);

            if (version >= 13)
            {
                this.Layer = (MapGeometryLayer)br.ReadByte();
            }

            uint submeshCount = br.ReadUInt32();
            for (int i = 0; i < submeshCount; i++)
            {
                this.Submeshes.Add(new(br, this));
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
                this.Layer = (MapGeometryLayer)br.ReadByte();
            }

            if (version >= 11)
            {
                this.MeshRenderFlags = (MapGeometryMeshRenderFlags)br.ReadByte();
            }

            if (useSeparatePointLights && (version < 7))
            {
                this.SeparatePointLight = br.ReadVector3();
            }

            if (version < 9)
            {
                for (int i = 0; i < 9; i++)
                {
                    this.UnknownFloats.Add(br.ReadVector3());
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

        public void Write(BinaryWriter bw, bool useSeparatePointLights, uint version)
        {
            if (version <= 11)
            {
                bw.Write(this.Name.Length);
                bw.Write(Encoding.ASCII.GetBytes(this.Name));
            }

            bw.Write(this.Vertices.Count);
            bw.Write((uint)1);
            bw.Write(this._vertexElementGroupId);
            bw.Write(this._vertexBufferId); //we only have one vertex buffer

            bw.Write(this.Indices.Count);
            bw.Write(this._indexBufferId);

            if (version >= 13)
            {
                bw.Write((byte)this.Layer);
            }

            bw.Write(this.Submeshes.Count);
            foreach (MapGeometrySubmesh submesh in this.Submeshes)
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
                bw.Write((byte)this.Layer);
            }

            if (version >= 11)
            {
                bw.Write((byte)this.MeshRenderFlags);
            }

            if (version < 9)
            {
                if (useSeparatePointLights)
                {
                    bw.WriteVector3(this.SeparatePointLight ?? Vector3.Zero);
                }

                foreach (Vector3 pointLight in this.UnknownFloats)
                {
                    bw.WriteVector3(pointLight);
                }
                for (int i = 0; i < 9 - this.UnknownFloats.Count; i++)
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
