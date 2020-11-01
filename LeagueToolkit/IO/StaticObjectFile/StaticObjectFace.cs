using LeagueToolkit.Helpers.Structures;
using System;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Text;

namespace LeagueToolkit.IO.StaticObjectFile
{
    internal class StaticObjectFace
    {
        public uint[] Indices { get; private set; }
        public string Material { get; private set; }
        public Vector2[] UVs { get; private set; }

        public StaticObjectFace(uint[] indices, string material, Vector2[] uvs)
        {
            this.Indices = indices;
            this.Material = material;
            this.UVs = uvs;
        }
        public StaticObjectFace(BinaryReader br)
        {
            this.Indices = new uint[] { br.ReadUInt32(), br.ReadUInt32(), br.ReadUInt32() };
            this.Material = Encoding.ASCII.GetString(br.ReadBytes(64)).Replace("\0", "");

            float[] uvs = new float[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
            this.UVs = new Vector2[]
            {
                new Vector2(uvs[0], uvs[3]),
                new Vector2(uvs[1], uvs[4]),
                new Vector2(uvs[2], uvs[5])
            };
        }
        public StaticObjectFace(StreamReader sr)
        {
            string[] input = sr.ReadLine().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            this.Indices = new uint[] { uint.Parse(input[1]), uint.Parse(input[2]), uint.Parse(input[3]) };
            this.Material = input[4];
            this.UVs = new Vector2[]
            {
                new Vector2(float.Parse(input[5], CultureInfo.InvariantCulture), float.Parse(input[6], CultureInfo.InvariantCulture)),
                new Vector2(float.Parse(input[7], CultureInfo.InvariantCulture), float.Parse(input[8], CultureInfo.InvariantCulture)),
                new Vector2(float.Parse(input[9], CultureInfo.InvariantCulture), float.Parse(input[10], CultureInfo.InvariantCulture)),
            };
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
                bw.Write(this.UVs[i].X);
            }
            for (int i = 0; i < 3; i++)
            {
                bw.Write(this.UVs[i].Y);
            }
        }
        public void Write(StreamWriter sw)
        {
            string indices = string.Format("{0} {1} {2}", this.Indices[0], this.Indices[1], this.Indices[2]);
            string uvs = string.Format("{0} {1} {2} {3} {4} {5}",
                this.UVs[0].X, this.UVs[1].X, this.UVs[2].X, 
                this.UVs[0].Y, this.UVs[1].Y, this.UVs[2].Y);

            sw.WriteLine(string.Format("3 {0} {1} {2}", indices, this.Material, uvs));
        }
    }
}
