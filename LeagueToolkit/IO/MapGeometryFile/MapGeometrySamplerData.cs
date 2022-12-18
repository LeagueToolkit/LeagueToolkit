using LeagueToolkit.Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace LeagueToolkit.IO.MapGeometryFile
{
    public struct MapGeometrySamplerData
    {
        public string Texture;
        public Vector2 Scale;
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
