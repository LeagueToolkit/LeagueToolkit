using System.IO;

namespace Fantome.Libraries.League.IO.LightGrid
{
    public class LightGridSun
    {
        public float OpcaityOfLightOnCharacters { get; set; } //What the fuck did I just write
        public float Unknown1 { get; set; }
        public float Unknown2 { get; set; }
        public float Unknown3 { get; set; }
        public float Unknown4 { get; set; }
        public float Unknown5 { get; set; }
        public float Unknown6 { get; set; }
        public float Unknown7 { get; set; }
        public float Unknown8 { get; set; }
        public float Unknown9 { get; set; }
        public float Unknown10 { get; set; }
        public float Unknown11 { get; set; }
        public float Unknown12 { get; set; }

        public LightGridSun(BinaryReader br)
        {
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
            this.Unknown11 = br.ReadSingle();
            this.Unknown12 = br.ReadSingle();
        }
        public void Write(BinaryWriter bw)
        {
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
