using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.SCB
{
    [DebuggerDisplay("[ {Material} ]")]
    public class SCBFace
    {
        public UInt32[] Indices { get; private set; } = new UInt32[3];
        public string Material { get; private set; }
        public Vector2[] UV { get; private set; } = new Vector2[3];

        public SCBFace(UInt32[] Indices, string Material, Vector2[] UV)
        {
            this.Indices = Indices;
            this.Material = Material;
            this.UV = UV;
        }
        public SCBFace(BinaryReader br)
        {
            for (int i = 0; i < 3; i++)
            {
                this.Indices[i] = br.ReadUInt32();
            }
            this.Material = Encoding.ASCII.GetString(br.ReadBytes(64)).Replace("\0", "");
            float[] uvs = new float[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
            for (int i = 0; i < 3; i++)
            {
                this.UV[i] = new Vector2(uvs[i], uvs[i + 3]);
            }
        }

        public void Write(BinaryWriter bw)
        {
            for (int i = 0; i < 3; i++)
            {
                bw.Write(this.Indices[i]);
            }
            bw.Write(this.Material.PadRight(64, '\u0000').ToCharArray());

            for (int i = 0; i < 3; i++)
            {
                bw.Write(this.UV[i].X);
            }
            for (int i = 0; i < 3; i++)
            {
                bw.Write(this.UV[i].Y);
            }
        }
    }
}
