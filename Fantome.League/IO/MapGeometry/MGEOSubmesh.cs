using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fantome.Libraries.League.Helpers.Structures;

namespace Fantome.Libraries.League.IO.MapGeometry
{
    public class MGEOSubmesh
    {
        private uint _unknown1 { get; set; }
        public string Name { get; set; }
        internal uint _startIndex;
        internal uint _indexCount;
        internal uint _startVertex;
        internal uint _vertexCount;
        public List<MGEOVertex> Vertices { get; set; } = new List<MGEOVertex>();
        public List<ushort> Indices { get; set; } = new List<ushort>();

        public MGEOSubmesh(BinaryReader br)
        {
            this._unknown1 = br.ReadUInt32();
            this.Name = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
            this._startIndex = br.ReadUInt32();
            this._indexCount = br.ReadUInt32();
            this._startVertex = br.ReadUInt32();
            this._vertexCount = br.ReadUInt32();
        }
    }
}
