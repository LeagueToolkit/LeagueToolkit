using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Fantome.Libraries.League.Helpers.Structures;

namespace Fantome.Libraries.League.IO.WorldGeometry
{
    /// <summary>
    /// Represents a <see cref="WGEOModel"/> inside of a <see cref="WGEOFile"/>
    /// </summary>
    public class WGEOModel
    {
        /// <summary>
        /// Texture Path of this <see cref="WGEOModel"/>
        /// </summary>
        public string Texture { get; set; }
        /// <summary>
        /// Material of this <see cref="WGEOModel"/>
        /// </summary>
        public string Material { get; set; }
        /// <summary>
        /// Bounding Sphere of this <see cref="WGEOModel"/>
        /// </summary>
        public R3DSphere Sphere { get; private set; }
        /// <summary>
        /// Axis Aligned Bounding Box of this <see cref="WGEOModel"/>
        /// </summary>
        public R3DBox BoundingBox { get; private set; }
        /// <summary>
        /// Vertices of this <see cref="WGEOModel"/>
        /// </summary>
        public List<WGEOVertex> Vertices { get; set; } = new List<WGEOVertex>();
        /// <summary>
        /// Indices of this <see cref="WGEOModel"/>
        /// </summary>
        public List<ushort> Indices { get; set; } = new List<ushort>();

        /// <summary>
        /// Initializes an empty <see cref="WGEOModel"/>
        /// </summary>
        public WGEOModel() { }

        /// <summary>
        /// Initializes a new <see cref="WGEOModel"/>
        /// </summary>
        /// <param name="texture">Texture Path of this <see cref="WGEOModel"/></param>
        /// <param name="material">Material of this <see cref="WGEOModel"/></param>
        /// <param name="vertices">Vertices of this <see cref="WGEOModel"/></param>
        /// <param name="indices">Indices of this <see cref="WGEOModel"/></param>
        public WGEOModel(string texture, string material, List<WGEOVertex> vertices, List<ushort> indices)
        {
            this.Texture = texture;
            this.Material = material;
            this.Vertices = vertices;
            this.Indices = indices;
            this.BoundingBox = CalculateBoundingBox();
            this.Sphere = CalculateSphere();
        }

        /// <summary>
        /// Initializes a new <see cref="WGEOModel"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        public WGEOModel(BinaryReader br)
        {
            this.Texture = Encoding.ASCII.GetString(br.ReadBytes(260)).Replace("\0", "");
            this.Material = Encoding.ASCII.GetString(br.ReadBytes(64));
            this.Material = this.Material.Remove(this.Material.IndexOf("\0"));
            this.Sphere = new R3DSphere(br);
            this.BoundingBox = new R3DBox(br);

            uint vertexCount = br.ReadUInt32();
            uint indexCount = br.ReadUInt32();
            for (int i = 0; i < vertexCount; i++)
            {
                this.Vertices.Add(new WGEOVertex(br));
            }
            for (int i = 0; i < indexCount; i++)
            {
                this.Indices.Add(br.ReadUInt16());
            }
        }

        /// <summary>
        /// Writes this <see cref="WGEOModel"/> into the specified <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        public void Write(BinaryWriter bw)
        {
            bw.Write(Encoding.ASCII.GetBytes(this.Texture.PadRight(260, '\u0000')));
            bw.Write(Encoding.ASCII.GetBytes(this.Material.PadRight(64, '\u0000')));

            Tuple<R3DSphere, R3DBox> boundingGeometry = CalculateBoundingGeometry();
            boundingGeometry.Item1.Write(bw);
            boundingGeometry.Item2.Write(bw);

            bw.Write(this.Vertices.Count);
            bw.Write(this.Indices.Count);
            foreach (WGEOVertex vertex in this.Vertices)
            {
                vertex.Write(bw);
            }
            foreach (ushort index in this.Indices)
            {
                bw.Write(index);
            }
        }

        public Tuple<R3DSphere, R3DBox> CalculateBoundingGeometry()
        {
            R3DBox box = CalculateBoundingBox();
            R3DSphere sphere = CalculateSphere(box);
            return new Tuple<R3DSphere, R3DBox>(sphere, box);
        }

        /// <summary>
        /// Calculates the Axis Aligned Bounding Box of this <see cref="WGEOModel"/>
        /// </summary>
        public R3DBox CalculateBoundingBox()
        {
            if(this.Vertices == null || this.Vertices.Count == 0)
            {
                return new R3DBox(new Vector3(0, 0, 0), new Vector3(0, 0, 0));
            }
            else
            {
                Vector3 min = this.Vertices[0].Position;
                Vector3 max = this.Vertices[0].Position;

                foreach (WGEOVertex vertex in this.Vertices)
                {
                    if (min.X > vertex.Position.X) min.X = vertex.Position.X;
                    if (min.Y > vertex.Position.Y) min.Y = vertex.Position.Y;
                    if (min.Z > vertex.Position.Z) min.Z = vertex.Position.Z;
                    if (max.X < vertex.Position.X) max.X = vertex.Position.X;
                    if (max.Y < vertex.Position.Y) max.Y = vertex.Position.Y;
                    if (max.Z < vertex.Position.Z) max.Z = vertex.Position.Z;
                }

                return new R3DBox(min, max);
            }
        }

        /// <summary>
        /// Calculates the Bounding Sphere of this <see cref="WGEOModel"/>
        /// </summary>
        public R3DSphere CalculateSphere()
        {
            R3DBox box = CalculateBoundingBox();
            Vector3 centralPoint = new Vector3(0.5f * (this.BoundingBox.Max.X + this.BoundingBox.Min.X),
                0.5f * (this.BoundingBox.Max.Y + this.BoundingBox.Min.Y),
                0.5f * (this.BoundingBox.Max.Z + this.BoundingBox.Min.Z));

            return new R3DSphere(centralPoint, Vector3.Distance(centralPoint, box.Max));
        }

        /// <summary>
        /// Calculates the Bounding Sphere of this <see cref="WGEOModel"/> from the specified <see cref="R3DBox"/>
        /// </summary>
        /// <param name="box"><see cref="R3DBox"/> to use for calculation</param>
        public R3DSphere CalculateSphere(R3DBox box)
        {
            Vector3 centralPoint = new Vector3(0.5f * (this.BoundingBox.Max.X + this.BoundingBox.Min.X),
                0.5f * (this.BoundingBox.Max.Y + this.BoundingBox.Min.Y),
                0.5f * (this.BoundingBox.Max.Z + this.BoundingBox.Min.Z));

            return new R3DSphere(centralPoint, Vector3.Distance(centralPoint, box.Max));
        }
    }
}
