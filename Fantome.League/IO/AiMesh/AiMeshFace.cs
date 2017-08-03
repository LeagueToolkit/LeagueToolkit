using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.Diagnostics;
using System.IO;

namespace Fantome.Libraries.League.IO.AiMesh
{
    [DebuggerDisplay("[ {Indices[0]}, {Indices[1]},  {Indices[2]} ]")]
    public class AiMeshFace
    {
        public Vector3[] Vertices = new Vector3[3];
        public UInt16[] Indices = new UInt16[3]; //Some of these are often [0xFF] and there is a way to retrieve them correctly

        public AiMeshFace(BinaryReader br)
        {
            for (int i = 0; i < 3; i++)
            {
                this.Vertices[i] = new Vector3(br);
            }
            for (int i = 0; i < 3; i++)
            {
                this.Indices[i] = br.ReadUInt16();
            }
        }

        public void Write(BinaryWriter bw)
        {
            for (int i = 0; i < 3; i++)
            {
                this.Vertices[i].Write(bw);
            }
            for (int i = 0; i < 3; i++)
            {
                bw.Write(this.Indices[i]);
            }
        }
    }
}
