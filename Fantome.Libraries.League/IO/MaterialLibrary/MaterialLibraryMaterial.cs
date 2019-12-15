using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Fantome.Libraries.League.Helpers.Structures;
using System.Globalization;

namespace Fantome.Libraries.League.IO.MaterialLibrary
{
    public class MaterialLibraryMaterial
    {
        public string Name { get; private set; }
        public MaterialFlags Flags { get; private set; }
        public ColorRGBVector3Byte EmissiveColor { get; private set; }
        public float[] UVScroll { get; private set; }
        public bool IsBackfaceCullingDisabled { get; private set; }
        public string ShaderName { get; private set; }
        public bool IsSimpleShader { get; private set; }
        public byte Opacity { get; private set; }
        public string Texture { get; private set; }
        public ColorRGBVector3Byte Color { get; private set; }

        public MaterialLibraryMaterial(string name, MaterialFlags flags, ColorRGBVector3Byte emissiveColor, float[] uvScroll, bool isBackfaceCullingDisabled,
            string shaderName, bool isSimpleShader, byte opacity, ColorRGBVector3Byte color)
        {
            this.Name = name;
            this.Flags = flags;
            this.EmissiveColor = emissiveColor;
            this.UVScroll = uvScroll;
            this.IsBackfaceCullingDisabled = isBackfaceCullingDisabled;
            this.ShaderName = shaderName;
            this.IsSimpleShader = isSimpleShader;
            this.Opacity = opacity;
            this.Color = color;
        }

        public MaterialLibraryMaterial(string name, MaterialFlags flags, ColorRGBVector3Byte emissiveColor, float[] uvScroll, bool isBackfaceCullingDisabled,
            string shaderName, bool isSimpleShader, byte opacity, ColorRGBVector3Byte color, string texture)
        {
            this.Name = name;
            this.Flags = flags;
            this.EmissiveColor = emissiveColor;
            this.UVScroll = uvScroll;
            this.IsBackfaceCullingDisabled = isBackfaceCullingDisabled;
            this.ShaderName = shaderName;
            this.IsSimpleShader = isSimpleShader;
            this.Opacity = opacity;
            this.Texture = texture;
            this.Color = color;
        }

        public MaterialLibraryMaterial(StreamReader sr)
        {
            string[] line;
            while ((line = sr.ReadLine().Split(new char[] { ' ', '=' }, StringSplitOptions.RemoveEmptyEntries))[0] != "[MaterialEnd]")
            {
                if (line[0] == "Name")
                {
                    this.Name = line[1];
                }
                else if (line[0] == "Flags")
                {
                    if (line.Contains("addop"))
                    {
                        this.Flags |= MaterialFlags.AddOp;
                    }
                    if (line.Contains("subop"))
                    {
                        this.Flags |= MaterialFlags.SubOp;
                    }
                    if (line.Contains("alphaop"))
                    {
                        this.Flags |= MaterialFlags.AlphaOp;
                    }
                    if (line.Contains("uvclamp"))
                    {
                        this.Flags |= MaterialFlags.UVClamp;
                    }
                    if (line.Contains("texture_gouraud_"))
                    {
                        this.Flags |= MaterialFlags.GroundTexture;
                    }
                }
                else if (line[0] == "EmissiveColor")
                {
                    int r = int.Parse(line[1]);
                    int g = int.Parse(line[2]);
                    int b = int.Parse(line[3]);
                    this.EmissiveColor = new ColorRGBVector3Byte
                        (
                        (r == int.MinValue) ? (byte)~r : (byte)r,
                        (g == int.MinValue) ? (byte)~g : (byte)g,
                        (b == int.MinValue) ? (byte)~b : (byte)b
                        );
                }
                else if (line[0] == "UVScroll")
                {
                    this.UVScroll = new float[]
                    {
                            float.Parse(line[1], CultureInfo.InvariantCulture),
                            float.Parse(line[2], CultureInfo.InvariantCulture)
                    };
                }
                else if (line[0] == "DisableBackfaceCulling")
                {
                    this.IsBackfaceCullingDisabled = (line[1] == "1");
                }
                else if (line[0] == "ShaderName")
                {
                    this.ShaderName = line[1];
                }
                else if (line[0] == "SimpleShader")
                {
                    this.IsSimpleShader = (line[1] == "1");
                }
                else if (line[0] == "Opacity")
                {
                    this.Opacity = byte.Parse(line[1]);
                }
                else if (line[0] == "Texture")
                {
                    this.Texture = line[1];
                }
                else if (line[0] == "Color24")
                {
                    int r = int.Parse(line[1]);
                    int g = int.Parse(line[2]);
                    int b = int.Parse(line[3]);
                    this.Color = new ColorRGBVector3Byte
                        (
                        (r == int.MinValue) ? (byte)~r : (byte)r,
                        (g == int.MinValue) ? (byte)~g : (byte)g,
                        (b == int.MinValue) ? (byte)~b : (byte)b
                        );
                }
            }
        }

        public void Write(StreamWriter sw)
        {
            string flags = "";
            if (this.Flags.HasFlag(MaterialFlags.GroundTexture))
            {
                flags += "texture_gouraud_ ";
            }
            if (this.Flags.HasFlag(MaterialFlags.AddOp))
            {
                flags += "addop ";
            }
            if (this.Flags.HasFlag(MaterialFlags.SubOp))
            {
                flags += "subop";
            }
            if (this.Flags.HasFlag(MaterialFlags.AlphaOp))
            {
                flags += "alphaop";
            }
            if (this.Flags.HasFlag(MaterialFlags.UVClamp))
            {
                flags += "uvclamp";
            }

            sw.WriteLine("[MaterialBegin]");
            sw.WriteLine("Name= " + this.Name);
            sw.WriteLine("Flags= " + flags);
            sw.WriteLine("EmissiveColor= {0} {1} {2}", this.EmissiveColor.R, this.EmissiveColor.G, this.EmissiveColor.B);
            sw.WriteLine("UVScroll = {0} {1}", this.UVScroll[0], this.UVScroll[1]);
            sw.WriteLine("DisableBackfaceCulling = " + (this.IsBackfaceCullingDisabled == true ? "1" : "0"));
            sw.WriteLine("ShaderName = " + this.ShaderName);
            sw.WriteLine("SimpleShader = " + (this.IsSimpleShader == true ? "1" : "0"));
            sw.WriteLine("Opacity= " + this.Opacity);
            if (this.Texture != null)
            {
                sw.WriteLine("Texture= " + this.Texture);
            }
            sw.WriteLine("Color24= {0} {1} {2}", this.Color.R, this.Color.G, this.Color.B);
            sw.WriteLine("[MaterialEnd]");
        }
    }

    [Flags]
    public enum MaterialFlags
    {
        AddOp = 0x100,
        SubOp = 0x200,
        AlphaOp = 0x400,
        UVClamp = 0x100000,
        GroundTexture = 0x200000
    }
}
