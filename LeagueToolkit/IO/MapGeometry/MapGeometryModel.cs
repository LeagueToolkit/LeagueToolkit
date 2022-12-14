using LeagueToolkit.Helpers.Extensions;
using LeagueToolkit.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.IO;
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
        
        public string StationaryLightTexture { get; set; } = string.Empty;
        public Vector2 StationaryLightScale { get; set; } = new();
        public Vector2 StationaryLightBias { get; set; } = new();

        public string BakedLightTexture { get; set; } = string.Empty;
        public Vector2 BakedLightScale { get; set; } = new();
        public Vector2 BakedLightBias { get; set; } = new();

        public string BakedPaintTexture { get; set; } = string.Empty;
        public Vector2 BakedPaintScale { get; set; } = new();
        public Vector2 BakedPaintBias { get; set; } = new();

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

            this.BoundingBox = GetBoundingBox();
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
            string stationaryLight,
            Vector2 stationaryLightScale,
            Vector2 stationaryLightBias
        ) : this(name, vertices, indices, submeshes)
        {
            this.StationaryLightTexture = stationaryLight;
            this.StationaryLightScale = stationaryLightScale;
            this.StationaryLightBias = stationaryLightBias;
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

            uint vertexCount = br.ReadUInt32();
            uint vertexBufferCount = br.ReadUInt32();
            int vertexElementGroup = br.ReadInt32();

            for (int i = 0; i < vertexCount; i++)
            {
                this.Vertices.Add(new());
            }

            for (
                int i = 0, currentVertexElementGroup = vertexElementGroup;
                i < vertexBufferCount;
                i++, currentVertexElementGroup++
            )
            {
                int vertexBufferId = br.ReadInt32();
                long returnPosition = br.BaseStream.Position;
                br.BaseStream.Seek(vertexBufferOffsets[vertexBufferId], SeekOrigin.Begin);

                for (int j = 0; j < vertexCount; j++)
                {
                    this.Vertices[j] = MapGeometryVertex.Combine(
                        this.Vertices[j],
                        new(br, vertexElementGroups[currentVertexElementGroup].VertexElements)
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

                this.StationaryLightTexture = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
                this.StationaryLightScale = br.ReadVector2();
                this.StationaryLightBias = br.ReadVector2();
            }
            else
            {
                this.StationaryLightTexture = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
                this.StationaryLightScale = br.ReadVector2();
                this.StationaryLightBias = br.ReadVector2();

                this.BakedLightTexture = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
                this.BakedLightScale = br.ReadVector2();
                this.BakedLightBias= br.ReadVector2();

                if (version >= 12)
                {
                    this.BakedPaintTexture = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
                    this.BakedPaintScale = br.ReadVector2();
                    this.BakedPaintBias = br.ReadVector2();
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
                    if (this.SeparatePointLight is Vector3 separatePointLight)
                    {
                        bw.WriteVector3(separatePointLight);
                    }
                    else
                    {
                        bw.WriteVector3(Vector3.Zero);
                    }
                }

                foreach (Vector3 pointLight in this.UnknownFloats)
                {
                    bw.WriteVector3(pointLight);
                }
                for (int i = 0; i < 9 - this.UnknownFloats.Count; i++)
                {
                    bw.WriteVector3(Vector3.Zero);
                }

                bw.Write(this.StationaryLightTexture.Length);
                bw.Write(Encoding.ASCII.GetBytes(this.StationaryLightTexture));
                bw.WriteVector2(this.StationaryLightScale);
                bw.WriteVector2(this.StationaryLightBias);
            }
            else
            {
                bw.Write(this.StationaryLightTexture.Length);
                bw.Write(Encoding.ASCII.GetBytes(this.StationaryLightTexture));
                bw.WriteVector2(this.StationaryLightScale);
                bw.WriteVector2(this.StationaryLightBias);

                bw.Write(this.BakedLightTexture.Length);
                bw.Write(Encoding.ASCII.GetBytes(this.BakedLightTexture));
                bw.WriteVector2(this.BakedLightScale);
                bw.WriteVector2(this.BakedLightBias);

                if (version >= 12)
                {
                    bw.Write(this.BakedPaintTexture.Length);
                    bw.Write(Encoding.ASCII.GetBytes(this.BakedPaintTexture));
                    bw.WriteVector2(this.BakedPaintScale);
                    bw.WriteVector2(this.BakedPaintBias);
                }
            }
        }

        public void AssignLightmap(string lightmap, Vector2 stationaryLightScale, Vector2 stationaryLightBias)
        {
            this.StationaryLightTexture = lightmap;
            this.StationaryLightScale = stationaryLightScale;
            this.StationaryLightBias = stationaryLightBias;
        }

        public Box GetBoundingBox()
        {
            if (this.Vertices == null || this.Vertices.Count == 0)
            {
                return new Box(new Vector3(0, 0, 0), new Vector3(0, 0, 0));
            }
            else
            {
                Vector3 min = this.Vertices[0].Position.Value;
                Vector3 max = this.Vertices[0].Position.Value;

                foreach (MapGeometryVertex vertex in this.Vertices)
                {
                    if (min.X > vertex.Position.Value.X)
                    {
                        min.X = vertex.Position.Value.X;
                    }
                    if (min.Y > vertex.Position.Value.Y)
                    {
                        min.Y = vertex.Position.Value.Y;
                    }
                    if (min.Z > vertex.Position.Value.Z)
                    {
                        min.Z = vertex.Position.Value.Z;
                    }
                    if (max.X < vertex.Position.Value.X)
                    {
                        max.X = vertex.Position.Value.X;
                    }
                    if (max.Y < vertex.Position.Value.Y)
                    {
                        max.Y = vertex.Position.Value.Y;
                    }
                    if (max.Z < vertex.Position.Value.Z)
                    {
                        max.Z = vertex.Position.Value.Z;
                    }
                }

                return new Box(min, max);
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
