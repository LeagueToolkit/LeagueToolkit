using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LeagueToolkit.Helpers.Structures;
using LeagueToolkit.Helpers.Extensions;
using System.Numerics;

namespace LeagueToolkit.IO.NVR
{
    public class SimpleEnvironmentMaterial
    {
        public string Name { get; private set; }
        public SimpleEnvironmentMaterialType Type { get; private set; }
        public SimpleEnvironmentMaterialFlags Flags { get; private set; }
        public List<SimpleEnvironmentChannel> Channels { get; private set; } = new List<SimpleEnvironmentChannel>();

        public SimpleEnvironmentMaterial(BinaryReader br, bool readOld)
        {
            this.Name = br.ReadPaddedString(260);
            this.Type = (SimpleEnvironmentMaterialType)br.ReadInt32();
            if (readOld)
            {
                Color diffuseColor = br.ReadColor(ColorFormat.RgbaF32);
                string diffuseName = br.ReadPaddedString(260);
                this.Channels.Add(new SimpleEnvironmentChannel(diffuseName, diffuseColor, Matrix4x4.Identity));

                Color emmisiveColor = br.ReadColor(ColorFormat.RgbaF32);
                string emissiveName = br.ReadPaddedString(260);
                this.Channels.Add(new SimpleEnvironmentChannel(emissiveName, emmisiveColor, Matrix4x4.Identity));

                for (int i = 0; i < 6; i++)
                {
                    this.Channels.Add(new SimpleEnvironmentChannel("", new Color(0, 0, 0, 0), Matrix4x4.Identity));
                }
            }
            else
            {
                this.Flags = (SimpleEnvironmentMaterialFlags)br.ReadUInt32();
                for (int i = 0; i < 8; i++)
                {
                    this.Channels.Add(new SimpleEnvironmentChannel(br));
                }
            }
        }

        public SimpleEnvironmentMaterial(
            string name,
            SimpleEnvironmentMaterialType type,
            SimpleEnvironmentMaterialFlags flag,
            List<SimpleEnvironmentChannel> channels
        )
        {
            this.Name = name;
            this.Type = type;
            this.Flags = flag;
            if (channels.Count != 8)
            {
                throw new MaterialInvalidChannelCountException(channels.Count);
            }
            this.Channels.AddRange(channels);
        }

        // Easy way to create a material with working values. Needs to be used with vertex 8
        public static SimpleEnvironmentMaterial CreateMaterial(string materialName, string textureName)
        {
            return CreateMaterial(
                materialName,
                textureName,
                new Color(0.003921569f, 0.003921569f, 0.003921569f, 0.003921569f),
                SimpleEnvironmentMaterialType.Default,
                SimpleEnvironmentMaterialFlags.DualVertexColor
            );
        }

        public static SimpleEnvironmentMaterial CreateMaterial(
            string materialName,
            string textureName,
            Color color,
            SimpleEnvironmentMaterialType matType,
            SimpleEnvironmentMaterialFlags matFlags
        )
        {
            List<SimpleEnvironmentChannel> channels = new List<SimpleEnvironmentChannel>();
            channels.Add(new SimpleEnvironmentChannel(textureName, color, Matrix4x4.Identity));
            for (int i = 0; i < 7; i++)
            {
                channels.Add(new SimpleEnvironmentChannel("", new Color(0, 0, 0, 0), Matrix4x4.Identity));
            }
            SimpleEnvironmentMaterial newMat = new SimpleEnvironmentMaterial(materialName, matType, matFlags, channels);
            return newMat;
        }

        public void Write(BinaryWriter bw)
        {
            bw.WritePaddedString(this.Name, 260);
            bw.Write((int)this.Type);
            bw.Write((int)this.Flags);
            foreach (SimpleEnvironmentChannel channel in this.Channels)
            {
                channel.Write(bw);
            }
        }
    }

    public enum SimpleEnvironmentMaterialType : int
    {
        Default = 0x0,
        Decal = 0x1,
        WallOfGrass = 0x2,
        FourBlend = 0x3
    };

    [Flags]
    public enum SimpleEnvironmentMaterialFlags : int
    {
        Ground = 1 << 0,
        NoShadow = 1 << 1,
        VertexAlpha = 1 << 2,
        Lightmapped = 1 << 3,
        DualVertexColor = 1 << 4,
        Background = 1 << 5,
        BackgroundWithFog = 1 << 6,
    }

    public class MaterialInvalidChannelCountException : Exception
    {
        public MaterialInvalidChannelCountException(int actual)
            : base(
                String.Format("There have to be exactly 8 channels in a material ({0} channel(s) specified).", actual)
            ) { }
    }
}
