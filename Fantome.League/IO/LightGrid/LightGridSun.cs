using Fantome.Libraries.League.Helpers.Structures;
using System.IO;

namespace Fantome.Libraries.League.IO.LightGrid
{
    public class LightGridSun
    {
        public Vector2 Position;
        public float OpcaityOfLightOnCharacters; //What the fuck did I just write
        public float Unknown1;
        public float Unknown2;
        public float Unknown3;
        public float Unknown4;
        public float Unknown5;
        public float Unknown6;
        public float Unknown7;
        public float Unknown8;
        public float Unknown9;
        public float Unknown10;
        public float Unknown11;
        public float Unknown12;
        public LightGridSun(BinaryReader br)
        {
            this.Position = new Vector2(br);
            this.OpcaityOfLightOnCharacters = br.ReadSingle();
            this.Unknown1 = br.ReadSingle();
            this.Unknown2 = br.ReadSingle();
            this.Unknown3 = br.ReadSingle();
            this.Unknown4 = br.ReadSingle();
            this.Unknown5 = br.ReadSingle();
            this.Unknown6 = br.ReadSingle();
            this.Unknown7 = br.ReadSingle();
            this.Unknown8 = br.ReadSingle();
            this.Unknown9 = br.ReadSingle();
            this.Unknown10 = br.ReadSingle();
            this.Unknown12 = br.ReadSingle();
            this.Unknown11 = br.ReadSingle();
        }
        public void Write(BinaryWriter bw)
        {
            this.Position.Write(bw);
            bw.Write(this.OpcaityOfLightOnCharacters);
            bw.Write(this.Unknown1);
            bw.Write(this.Unknown2);
            bw.Write(this.Unknown3);
            bw.Write(this.Unknown4);
            bw.Write(this.Unknown5);
            bw.Write(this.Unknown6);
            bw.Write(this.Unknown7);
            bw.Write(this.Unknown8);
            bw.Write(this.Unknown9);
            bw.Write(this.Unknown10);
            bw.Write(this.Unknown11);
            bw.Write(this.Unknown12);
        }
    }
}
