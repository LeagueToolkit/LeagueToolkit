using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Fantome.Libraries.League.Helpers.Structures;

namespace Fantome.Libraries.League.IO.NVR
{
    public class NVRMaterial
    {
        public string Name { get; private set; }
        public NVRMaterialType Type { get; private set; }
        public NVRMaterialFlags Flags { get; private set; }
        public List<NVRChannel> Channels { get; private set; } = new List<NVRChannel>();

        public NVRMaterial(BinaryReader br)
        {
            this.Name = Encoding.ASCII.GetString(br.ReadBytes(260)).Replace("\0", "");
            this.Type = (NVRMaterialType)br.ReadInt32();
            this.Flags = (NVRMaterialFlags)br.ReadUInt32();
            for (int i = 0; i < 8; i++)
            {
                this.Channels.Add(new NVRChannel(br));
            }
        }

        public NVRMaterial(string name, NVRMaterialType type, NVRMaterialFlags flag, List<NVRChannel> channels)
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
        public static NVRMaterial CreateMaterial(string materialName, string textureName)
        {
            return CreateMaterial(materialName, textureName, new ColorRGBAVector4(0.003921569f, 0.003921569f, 0.003921569f, 0.003921569f), NVRMaterialType.MATERIAL_TYPE_DEFAULT, NVRMaterialFlags.ColoredVertex);
        }

        public static NVRMaterial CreateMaterial(string materialName, string textureName, ColorRGBAVector4 color, NVRMaterialType matType, NVRMaterialFlags matFlags)
        {
            List<NVRChannel> channels = new List<NVRChannel>();
            channels.Add(new NVRChannel(textureName, color, new R3DMatrix44()));
            for (int i = 0; i < 7; i++)
            {
                channels.Add(new NVRChannel("", new ColorRGBAVector4(0, 0, 0, 0), new R3DMatrix44()));
            }
            NVRMaterial newMat = new NVRMaterial(materialName, matType, matFlags, channels);
            return newMat;
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(this.Name.PadRight(260, '\u0000').ToCharArray());
            bw.Write((int)this.Type);
            bw.Write((UInt32)this.Flags);
            foreach (NVRChannel channel in this.Channels)
            {
                channel.Write(bw);
            }
        }
    }

    public enum NVRMaterialType : int
    {
        MATERIAL_TYPE_DEFAULT = 0x0,
        MATERIAL_TYPE_DECAL = 0x1,
        MATERIAL_TYPE_WALL_OF_GRASS = 0x2,
        MATERIAL_TYPE_FOUR_BLEND = 0x3,
        MATERIAL_TYPE_COUNT = 0x4
    };

    [Flags]
    public enum NVRMaterialFlags : UInt32
    {
        GroundVertex = 1,
        ColoredVertex = 16
    }

    public class MaterialInvalidChannelCountException : Exception
    {
        public MaterialInvalidChannelCountException(int actual) : base(String.Format("There have to be exactly 8 channels in a material ({0} channel(s) specified).", actual)) { }
    }
}
