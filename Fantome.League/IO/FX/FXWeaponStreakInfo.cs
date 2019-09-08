using Fantome.Libraries.League.Helpers.Structures;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.FX
{
    public class FXWeaponStreakInfo
    {
        public int LinkType { get; private set; }
        public int BlendType { get; private set; }
        public float TrailsPerSecond { get; private set; }
        public float TrailCount { get; private set; }
        public float StartAlpha { get; private set; }
        public float EndAlpha { get; private set; }
        public float AlphaDecay { get; private set; }
        public int TextureMapMode { get; private set; }
        public string Texture { get; private set; }
        public TimeGradient ColorOverTime { get; private set; }
        public TimeGradient WidthOverTime { get; private set; }

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
            this.Texture = this.Texture.Remove(this.Texture.IndexOf(this.Texture.Contains("\0") ? '\u0000' : '?'));

            this.ColorOverTime = new TimeGradient(br);
            this.WidthOverTime = new TimeGradient(br);
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
            this.ColorOverTime.Write(bw);
            this.WidthOverTime.Write(bw);
        }
    }
}
