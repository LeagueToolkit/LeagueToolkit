using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.SKN
{
    public class SKNSubmesh
    {
        public string Name { get; private set; }
        internal uint _startVertex;
        internal uint _vertexCount;
        internal uint _startIndex;
        internal uint _indexCount;
        public List<ushort> Indices { get; internal set; } = new List<ushort>();
        public List<SKNVertex> Vertices { get; internal set; } = new List<SKNVertex>();
        internal SKNFile _skn;

        public SKNSubmesh(string name, List<ushort> indices, List<SKNVertex> vertices)
        {
            this.Name = name;
            this.Indices = indices;
            this.Vertices = vertices;
        }

        public SKNSubmesh(string name, List<ushort> indices, List<SKNVertex> vertices, SKNFile skn)
        {
            this.Name = name;
            this.Indices = indices;
            this.Vertices = vertices;
            this._skn = skn;
        }

        public SKNSubmesh(SKNFile skn, BinaryReader br)
        {
            this._skn = skn;
            this.Name = Encoding.ASCII.GetString(br.ReadBytes(64)).Replace("\0", "");
            this._startVertex = br.ReadUInt32();
            this._vertexCount = br.ReadUInt32();
            this._startIndex = br.ReadUInt32();
            this._indexCount = br.ReadUInt32();
        }

        public void AssignSimpleSkin(SKNFile skn)
        {
            this._skn = skn;
        }

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
