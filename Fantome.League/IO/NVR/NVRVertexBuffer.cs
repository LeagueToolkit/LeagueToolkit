using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fantome.Libraries.League.IO.NVR
{
    public class NVRVertexBuffer
    {
        public int Length { get; private set; }
        public long Offset { get; private set; }

        //Used for write stuff
        public NVRVertexType Type { get; private set; }
        public List<NVRVertex> Vertices { get; private set; } = new List<NVRVertex>();

        public NVRVertexBuffer(BinaryReader br)
        {
            this.Length = br.ReadInt32();
            this.Offset = br.BaseStream.Position;
            br.BaseStream.Seek(this.Length, SeekOrigin.Current);
        }

        public void Write(BinaryWriter bw)
        {
            int bufferLength = this.Vertices[0].GetSize() * this.Vertices.Count;
            bw.Write(bufferLength);
            foreach (NVRVertex vertex in this.Vertices)
            {
                vertex.Write(bw);
            }
        }

        public NVRVertexBuffer(NVRVertexType type)
        {
            this.Type = type;
        }
    }
}
