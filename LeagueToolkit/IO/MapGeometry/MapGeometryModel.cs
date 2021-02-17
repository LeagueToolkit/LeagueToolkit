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
        public List<MapGeometryVertex> Vertices { get; set; } = new List<MapGeometryVertex>();
        public List<ushort> Indices { get; set; } = new List<ushort>();
        public List<MapGeometrySubmesh> Submeshes { get; set; } = new List<MapGeometrySubmesh>();
        public bool FlipNormals { get; set; }
        public R3DBox BoundingBox { get; set; }
        public R3DMatrix44 Transformation { get; set; } = R3DMatrix44.IdentityR3DMatrix44();
        public MapGeometryModelFlags Flags { get; set; } = MapGeometryModelFlags.GenericObject;
        public MapGeometryLayer Layer { get; set; } = MapGeometryLayer.AllLayers;
        public byte UnknownByte { get; set; }
        public Vector3? SeparatePointLight { get; set; }
        public List<Vector3> UnknownFloats { get; set; } = new List<Vector3>();
        public string Lightmap { get; set; } = string.Empty;
        public string BakedPaintTexture { get; set; } = string.Empty;
        public Color Color { get; set; } = new Color(0, 0, 0, 1);
        public Color BakedPaintColor { get; set; } = new Color(0, 0, 0, 1);

        internal int _vertexElementGroupID;
        internal int _vertexBufferID;
        internal int _indexBufferID;

        public MapGeometryModel() { }
        public MapGeometryModel(string name, List<MapGeometryVertex> vertices, List<ushort> indices, List<MapGeometrySubmesh> submeshes)
        {
            this.Name = name;
            this.Vertices = vertices;
            this.Indices = indices;
            this.Submeshes = submeshes;

            foreach(MapGeometrySubmesh submesh in submeshes)
            {
                submesh.Parent = this;
            }

            this.BoundingBox = GetBoundingBox();
        }
        public MapGeometryModel(string name, List<MapGeometryVertex> vertices, List<ushort> indices, List<MapGeometrySubmesh> submeshes, MapGeometryLayer layer)
            : this(name, vertices, indices, submeshes)
        {
            this.Layer = layer;
        }
        public MapGeometryModel(string name, List<MapGeometryVertex> vertices, List<ushort> indices, List<MapGeometrySubmesh> submeshes, R3DMatrix44 transformation)
            : this(name, vertices, indices, submeshes)
        {
            this.Transformation = transformation;
        }
        public MapGeometryModel(string name, List<MapGeometryVertex> vertices, List<ushort> indices, List<MapGeometrySubmesh> submeshes, MapGeometryLayer layer, R3DMatrix44 transformation)
            : this(name, vertices, indices, submeshes)
        {
            this.Layer = layer;
            this.Transformation = transformation;
        }
        public MapGeometryModel(string name, List<MapGeometryVertex> vertices, List<ushort> indices, List<MapGeometrySubmesh> submeshes, string lightmap, Color color)
            : this(name, vertices, indices, submeshes)
        {
            this.Lightmap = lightmap;
            this.Color = color;
        }
        public MapGeometryModel(BinaryReader br, List<MapGeometryVertexElementGroup> vertexElementGroups, List<long> vertexBufferOffsets, List<ushort[]> indexBuffers, bool useSeparatePointLights, uint version)
        {
            this.Name = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
            uint vertexCount = br.ReadUInt32();
            uint vertexBufferCount = br.ReadUInt32();
            int vertexElementGroup = br.ReadInt32();

            for (int i = 0; i < vertexCount; i++)
            {
                this.Vertices.Add(new MapGeometryVertex());
            }

            for (int i = 0, currentVertexElementGroup = vertexElementGroup; i < vertexBufferCount; i++, currentVertexElementGroup++)
            {
                int vertexBufferID = br.ReadInt32();
                long returnPosition = br.BaseStream.Position;
                br.BaseStream.Seek(vertexBufferOffsets[vertexBufferID], SeekOrigin.Begin);

                for (int j = 0; j < vertexCount; j++)
                {
                    this.Vertices[j] = MapGeometryVertex.Combine(this.Vertices[j], new MapGeometryVertex(br, vertexElementGroups[currentVertexElementGroup].VertexElements));
                }

                br.BaseStream.Seek(returnPosition, SeekOrigin.Begin);
            }

            uint indexCount = br.ReadUInt32();
            int indexBuffer = br.ReadInt32();
            this.Indices.AddRange(indexBuffers[indexBuffer]);

            uint submeshCount = br.ReadUInt32();
            for (int i = 0; i < submeshCount; i++)
            {
                this.Submeshes.Add(new MapGeometrySubmesh(br, this));
            }

            if (version != 5)
            {
                this.FlipNormals = br.ReadBoolean();
            }

            this.BoundingBox = new R3DBox(br);
            this.Transformation = new R3DMatrix44(br);
            this.Flags = (MapGeometryModelFlags)br.ReadByte();

            if (version >= 7)
            {
                this.Layer = (MapGeometryLayer)br.ReadByte();

                if(version >= 11)
                {
                    this.UnknownByte = br.ReadByte();
                }
            }

            if (useSeparatePointLights && (version < 7))
            {
                this.SeparatePointLight = br.ReadVector3();
            }

            if(version < 9)
            {
                for (int i = 0; i < 9; i++)
                {
                    this.UnknownFloats.Add(br.ReadVector3());
                }

                this.Lightmap = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
                this.Color = br.ReadColor(ColorFormat.RgbaF32);
            }
            else if(version >= 9)
            {
                this.Lightmap = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
                this.Color = br.ReadColor(ColorFormat.RgbaF32);

                this.BakedPaintTexture = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
                this.BakedPaintColor = br.ReadColor(ColorFormat.RgbaF32);
            }
        }

        public void Write(BinaryWriter bw, bool useSeparatePointLights, uint version)
        {
            bw.Write(this.Name.Length);
            bw.Write(Encoding.ASCII.GetBytes(this.Name));

            bw.Write(this.Vertices.Count);
            bw.Write((uint)1);
            bw.Write(this._vertexElementGroupID);
            bw.Write(this._vertexBufferID); //we only have one vertex buffer

            bw.Write(this.Indices.Count);
            bw.Write(this._indexBufferID);

            bw.Write(this.Submeshes.Count);
            foreach (MapGeometrySubmesh submesh in this.Submeshes)
            {
                submesh.Write(bw);
            }

            if (version != 5)
            {
                bw.Write(this.FlipNormals);
            }

            this.BoundingBox.Write(bw);
            this.Transformation.Write(bw);
            bw.Write((byte)this.Flags);

            if (version >= 7)
            {
                bw.Write((byte)this.Layer);

                if(version >= 11)
                {
                    bw.Write(this.UnknownByte);
                }
            }

            if(version < 9)
            {
                if(useSeparatePointLights)
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

                bw.Write(this.Lightmap.Length);
                bw.Write(Encoding.ASCII.GetBytes(this.Lightmap));
                bw.WriteColor(this.Color, ColorFormat.RgbaF32);
            }
            else if(version >= 9)
            {
                bw.Write(this.Lightmap.Length);
                if (this.Lightmap.Length != 0) { bw.Write(Encoding.ASCII.GetBytes(this.Lightmap)); }
                bw.WriteColor(this.Color, ColorFormat.RgbaF32);

                bw.Write(this.BakedPaintTexture.Length);
                if (this.BakedPaintTexture.Length != 0) { bw.Write(Encoding.ASCII.GetBytes(this.BakedPaintTexture)); }
                bw.WriteColor(this.BakedPaintColor, ColorFormat.RgbaF32);
            }
        }

        public void AssignLightmap(string lightmap, Color color)
        {
            this.Lightmap = lightmap;
            this.Color = color;
        }

        public R3DBox GetBoundingBox()
        {
            if (this.Vertices == null || this.Vertices.Count == 0)
            {
                return new R3DBox(new Vector3(0, 0, 0), new Vector3(0, 0, 0));
            }
            else
            {
                Vector3 min = this.Vertices[0].Position.Value;
                Vector3 max = this.Vertices[0].Position.Value;

                foreach (MapGeometryVertex vertex in this.Vertices)
                {
                    if (min.X > vertex.Position.Value.X) min.X = vertex.Position.Value.X;
                    if (min.Y > vertex.Position.Value.Y) min.Y = vertex.Position.Value.Y;
                    if (min.Z > vertex.Position.Value.Z) min.Z = vertex.Position.Value.Z;
                    if (max.X < vertex.Position.Value.X) max.X = vertex.Position.Value.X;
                    if (max.Y < vertex.Position.Value.Y) max.Y = vertex.Position.Value.Y;
                    if (max.Z < vertex.Position.Value.Z) max.Z = vertex.Position.Value.Z;
                }

                return new R3DBox(min, max);
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
    public enum MapGeometryModelFlags : byte
    {
        UnknownTransparency = 1,
        UnknownLightning = 2,
        Unknown3 = 4,
        Unknown4 = 8,
        UnknownConst1 = 16,

        GenericObject = UnknownTransparency | UnknownLightning | Unknown3 | Unknown4 | UnknownConst1
    }
}
