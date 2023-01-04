using LeagueToolkit.Helpers.Extensions;
using System.IO;
using System.Numerics;
using System.Text;

namespace LeagueToolkit.IO.MapGeometryFile
{
    /// <summary>
    /// Represents an environment terrain mesh sampler
    /// </summary>
    public struct MapGeometrySamplerData
    {
        /// <summary>Gets the texture path</summary>
        public string Texture;

        /// <summary>Gets the UV scale being used to sample from <see cref="Texture"/></summary>
        public Vector2 Scale;

        /// <summary>Gets the UV offset being used to sample from <see cref="Texture"/></summary>
        /// <remarks>Applied after scaling</remarks>
        public Vector2 Bias;

        /// <summary>Creates a new <see cref="MapGeometrySamplerData"/> object</summary>
        public MapGeometrySamplerData()
        {
            this.Texture = string.Empty;
            this.Scale = Vector2.Zero;
            this.Bias = Vector2.Zero;
        }

        /// <summary>Creates a new <see cref="MapGeometrySamplerData"/> object with the specified parameters</summary>
        public MapGeometrySamplerData(string texture, Vector2 scale, Vector2 bias)
        {
            this.Texture = texture;
            this.Scale = scale;
            this.Bias = bias;
        }

        internal static MapGeometrySamplerData Read(BinaryReader br)
        {
            return new()
            {
                Texture = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32())),
                Scale = br.ReadVector2(),
                Bias = br.ReadVector2()
            };
        }

        internal void Write(BinaryWriter bw)
        {
            bw.Write(this.Texture.Length);
            bw.Write(Encoding.ASCII.GetBytes(this.Texture ?? string.Empty));
            bw.WriteVector2(this.Scale);
            bw.WriteVector2(this.Bias);
        }
    }
}
