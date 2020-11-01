using LeagueToolkit.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LeagueToolkit.IO.SimpleSkinFile
{
    public class SimpleSkinSubmesh
    {
        public string Name { get; set; }
        public List<SimpleSkinVertex> Vertices { get; set; }
        public List<ushort> Indices { get; set; }

        internal uint _startVertex;
        internal uint _vertexCount;
        internal uint _startIndex;
        internal uint _indexCount;

        public SimpleSkinSubmesh(string name, List<ushort> indices, List<SimpleSkinVertex> vertices)
        {
            this.Name = name;
            this.Indices = indices;
            this.Vertices = vertices;
        }
        public SimpleSkinSubmesh(BinaryReader br)
        {
            this.Name = Encoding.ASCII.GetString(br.ReadBytes(64)).Replace("\0", "");
            this._startVertex = br.ReadUInt32();
            this._vertexCount = br.ReadUInt32();
            this._startIndex = br.ReadUInt32();
            this._indexCount = br.ReadUInt32();
        }

        public void Write(BinaryWriter bw, uint startVertex, uint startIndex)
        {
            bw.Write(Encoding.ASCII.GetBytes(this.Name.PadRight(64, '\u0000')));
            bw.Write(startVertex);
            bw.Write(this.Vertices.Count);
            bw.Write(startIndex);
            bw.Write(this.Indices.Count);
        }
    }
}
