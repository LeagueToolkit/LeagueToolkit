using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.FX
{
    [DebuggerDisplay("[ {Texture}, Link Type: {LinkType}, Blend Type: {BlendType} ]")]
    public class FXWeaponStreakInfo
    {
        public Int32 LinkType { get; private set; }
        public Int32 BlendType { get; private set; }
        public float TrailsPerSecond { get; private set; }
        public float TrailCount { get; private set; }
        public float StartAlpha { get; private set; }
        public float EndAlpha { get; private set; }
        public float AlphaDecay { get; private set; }
        public Int32 TextureMapMode { get; private set; }
        public string Texture { get; private set; }
        public FXTimeGradient ColorOverTime { get; private set; }
        public FXTimeGradient WidthOverTime { get; private set; }

        public FXWeaponStreakInfo(BinaryReader br)
        {
            this.LinkType = br.ReadInt32();
            this.BlendType = br.ReadInt32();
            this.TrailsPerSecond = br.ReadSingle();
            this.TrailCount = br.ReadSingle();
            this.StartAlpha = br.ReadSingle();
            this.EndAlpha = br.ReadSingle();
            this.AlphaDecay = br.ReadSingle();
            this.TextureMapMode = br.ReadInt32();


            this.Texture = Encoding.ASCII.GetString(br.ReadBytes(64));
            char TextureIndexOf = this.Texture.Contains("\0") ? '\u0000' : '?';
            this.Texture = this.Texture.Remove(this.Texture.IndexOf(TextureIndexOf));

            this.ColorOverTime = new FXTimeGradient(br);
            this.WidthOverTime = new FXTimeGradient(br);
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(this.LinkType);
            bw.Write(this.BlendType);
            bw.Write(this.TrailsPerSecond);
            bw.Write(this.TrailCount);
            bw.Write(this.StartAlpha);
            bw.Write(this.EndAlpha);
            bw.Write(this.AlphaDecay);
            bw.Write(this.TextureMapMode);
            bw.Write(this.Texture.PadRight(64, '\u0000').ToCharArray());
            ColorOverTime.Write(bw);
            WidthOverTime.Write(bw);
        }
    }
}
