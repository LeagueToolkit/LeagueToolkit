using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Fantome.Libraries.League.IO.SimpleSkin
{
    /// <summary>
    /// Represents a model inside of a <see cref="SKNFile"/>
    /// </summary>
    public class SKNSubmesh
    {
        /// <summary>
        /// Name of this <see cref="SKNSubmesh"/>
        /// </summary>
        public string Name { get; private set; }
        internal uint _startVertex;
        internal uint _vertexCount;
        internal uint _startIndex;
        internal uint _indexCount;

        /// <summary>
        /// Indices of this <see cref="SKNSubmesh"/>
        /// </summary>
        public List<ushort> Indices { get; internal set; } = new List<ushort>();

        /// <summary>
        /// Vertices of this <see cref="SKNSubmesh"/>
        /// </summary>
        public List<SKNVertex> Vertices { get; internal set; } = new List<SKNVertex>();
        internal SKNFile _skn;

        /// <summary>
        /// Initializes a new <see cref="SKNSubmesh"/>
        /// </summary>
        /// <param name="name">Name of this <see cref="SKNSubmesh"/></param>
        /// <param name="indices">Indices of this <see cref="SKNSubmesh"/></param>
        /// <param name="vertices">Vertices of this <see cref="SKNSubmesh"/></param>
        public SKNSubmesh(string name, List<ushort> indices, List<SKNVertex> vertices)
        {
            this.Name = name;
            this.Indices = indices;
            this.Vertices = vertices;
        }

        /// <summary>
        /// Initializes a new <see cref="SKNSubmesh"/> with the spcified <see cref="SKNFile"/>
        /// </summary>
        /// <param name="name">Name of this <see cref="SKNSubmesh"/></param>
        /// <param name="indices">Indices of this <see cref="SKNSubmesh"/></param>
        /// <param name="vertices">Vertices of this <see cref="SKNSubmesh"/></param>
        /// <param name="skn"><see cref="SKNFile"/> this <see cref="SKNSubmesh"/> belongs to</param>
        public SKNSubmesh(string name, List<ushort> indices, List<SKNVertex> vertices, SKNFile skn)
        {
            this.Name = name;
            this.Indices = indices;
            this.Vertices = vertices;
            this._skn = skn;
        }

        /// <summary>
        /// Initializes a new <see cref="SKNSubmesh"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="skn"><see cref="SKNFile"/> this <see cref="SKNSubmesh"/> belongs to</param>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        public SKNSubmesh(SKNFile skn, BinaryReader br)
        {
            this._skn = skn;
            this.Name = Encoding.ASCII.GetString(br.ReadBytes(64)).Replace("\0", "");
            this._startVertex = br.ReadUInt32();
            this._vertexCount = br.ReadUInt32();
            this._startIndex = br.ReadUInt32();
            this._indexCount = br.ReadUInt32();
        }

        /// <summary>
        /// Assigns the specified <see cref="SKNFile"/> to this <see cref="SKNSubmesh"/> 
        /// </summary>
        /// <param name="skn"><see cref="SKNFile"/> this <see cref="SKNSubmesh"/> belongs to</param>
        public void AssignSimpleSkin(SKNFile skn)
        {
            this._skn = skn;
        }

        /// <summary>
        /// Gets the Normalized Indices of this <see cref="SKNSubmesh"/>
        /// </summary>
        public List<ushort> GetNormalizedIndices()
        {
            ushort min = this.Indices.Min();
            return this.Indices.Select(x => x -= min).ToList();
        }

        /// <summary>
        /// Calculates an AABB Bounding Box of this <see cref="SKNSubmesh"/>
        /// </summary>
        public R3DBox CalculateBoundingBox()
        {
            if (this.Vertices.Count == 0)
            {
                return new R3DBox(new Vector3(0, 0, 0), new Vector3(0, 0, 0));
            }
            else
            {
                Vector3 min = this.Vertices[0].Position;
                Vector3 max = this.Vertices[0].Position;

                foreach (SKNVertex vertex in this.Vertices)
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
        /// Calculates a Bounding Sphere of this <see cref="SKNSubmesh"/>
        /// </summary>
        public R3DSphere CalculateBoundingSphere()
        {
            R3DBox box = CalculateBoundingBox();
            Vector3 centralPoint = CalculateCentralPoint();

            return new R3DSphere(centralPoint, Vector3.Distance(centralPoint, box.Max));
        }

        /// <summary>
        /// Calculates a Bounding Sphere from a <see cref="R3DBox"/> of this <see cref="SKNSubmesh"/>
        /// </summary>
        /// <param name="box"><see cref="R3DBox"/> of this <see cref="SKNSubmesh"/></param>
        public R3DSphere CalculateBoundingSphere(R3DBox box)
        {
            Vector3 centralPoint = CalculateCentralPoint();

            return new R3DSphere(centralPoint, Vector3.Distance(centralPoint, box.Max));
        }

        /// <summary>
        /// Calculates a Bounding Sphere from a <see cref="R3DBox"/> and a Central Point of this <see cref="SKNSubmesh"/>
        /// </summary>
        /// <param name="box"><see cref="R3DBox"/> of this <see cref="SKNSubmesh"/></param>
        /// <param name="centralPoint">Position of the <see cref="R3DSphere"/></param>
        public R3DSphere CalculateBoundingSphere(R3DBox box, Vector3 centralPoint)
        {
            return new R3DSphere(centralPoint, Vector3.Distance(centralPoint, box.Max));
        }

        /// <summary>
        /// Calculates the Central Point of this <see cref="SKNSubmesh"/>
        /// </summary>
        public Vector3 CalculateCentralPoint()
        {
            R3DBox box = CalculateBoundingBox();

            return new Vector3(0.5f * (box.Min.X + box.Max.X),
                0.5f * (box.Min.Y + box.Max.Y),
                0.5f * (box.Min.Z + box.Max.Z));
        }

        /// <summary>
        /// Calculates the Central Point of this <see cref="SKNSubmesh"/> from a <see cref="R3DBox"/> of this <see cref="SKNSubmesh"/>
        /// </summary>
        /// <param name="box"><see cref="R3DBox"/> of this <see cref="SKNSubmesh"/></param>
        public Vector3 CalculateCentralPoint(R3DBox box)
        {
            return new Vector3(0.5f * (box.Min.X + box.Max.X),
                0.5f * (box.Min.Y + box.Max.Y),
                0.5f * (box.Min.Z + box.Max.Z));
        }

        /// <summary>
        /// Writes this <see cref="SKNSubmesh"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        public void Write(BinaryWriter bw)
        {
            if (this._skn == null)
            {
                throw new Exception("This submesh is not linked to an SKN File");
            }
            else
            {
                bw.Write(this.Name.PadRight(64, '\u0000').ToCharArray());

                int vertexOffset = 0;
                int indexOffset = 0;
                int submeshIndex = this._skn.Submeshes.IndexOf(this);

                if (this._skn.Submeshes.Count != 1)
                {
                    foreach (SKNSubmesh submesh in this._skn.Submeshes.GetRange(0, submeshIndex))
                    {
                        vertexOffset += submesh.Vertices.Count;
                        indexOffset += submesh.Indices.Count;
                    }
                }

                bw.Write(vertexOffset);
                bw.Write(this.Vertices.Count);
                bw.Write(indexOffset);
                bw.Write(this.Indices.Count);
            }
        }
    }
}
