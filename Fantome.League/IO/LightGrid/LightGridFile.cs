using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fantome.League.Helpers.Structures;

namespace Fantome.League.IO.LightGrid
{
    public class LightGridFile
    {
        public UInt32 Version { get; private set; }
        public UInt32 Width { get; private set; }
        public UInt32 Heigth { get; private set; }
        public LightGridSun Sun { get; private set; }
        public List<ColorRGBAVector4Byte> Grid { get; private set; } = new List<ColorRGBAVector4Byte>();
        public LightGridFile(string location)
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(location)))
            {
                this.Version = br.ReadUInt32();
                UInt32 GridOffset = br.ReadUInt32();
                this.Width = br.ReadUInt32();
                this.Heigth = br.ReadUInt32();
                this.Sun = new LightGridSun(br);

                while (br.BaseStream.Position != br.BaseStream.Length)
                {
                    this.Grid.Add(new ColorRGBAVector4Byte(br));
                }
            }
        }
    }
}
