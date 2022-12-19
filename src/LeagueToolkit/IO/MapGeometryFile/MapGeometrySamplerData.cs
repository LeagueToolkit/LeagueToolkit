using LeagueToolkit.Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace LeagueToolkit.IO.MapGeometryFile
{
    /// <summary>
    /// Contains information for an arbitrary environment terrain mesh sampler
    /// </summary>
    public struct MapGeometrySamplerData
    {
        /// <summary>
        /// The path of the texture to use for this sampler
        /// </summary>
        public string Texture;

        /// <summary>
        /// The scale of UVs being used to sample from <see cref="Texture"/>
        /// </summary>
        public Vector2 Scale;

        /// <summary>
        /// The offset of UVs being used to sample from <see cref="Texture"/>
        /// </summary>
        /// <remarks>
        /// Applied after scaling
        /// </remarks>
        public Vector2 Bias;

        public MapGeometrySamplerData()
        {
            this.Texture = string.Empty;
            this.Scale = Vector2.Zero;
            this.Bias = Vector2.Zero;
        }

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
