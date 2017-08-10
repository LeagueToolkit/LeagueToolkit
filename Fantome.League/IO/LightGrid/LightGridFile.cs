using Fantome.Libraries.League.Helpers.Structures;
using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.IO.LightGrid
{
    public class LightGridFile
    {
        public uint Version { get; private set; }
        private uint _gridOffset;
        public uint Width { get; private set; }
        public uint Heigth { get; private set; }
        public LightGridSun Sun { get; private set; }
        public List<ColorRGBAVector4Byte> Grid { get; private set; } = new List<ColorRGBAVector4Byte>();

        public LightGridFile(string fileLocation)
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(fileLocation)))
            {
                this.Version = br.ReadUInt32();
                this._gridOffset = br.ReadUInt32();
                this.Width = br.ReadUInt32();
                this.Heigth = br.ReadUInt32();
                this.Sun = new LightGridSun(br);

                while (br.BaseStream.Position != br.BaseStream.Length)
                {
                    this.Grid.Add(new ColorRGBAVector4Byte(br));
                }
            }
        }

        public void Write(string fileLocation)
        {
            using (BinaryWriter bw = new BinaryWriter(File.Open(fileLocation, FileMode.Create)))
            {
                bw.Write(this.Version);
                bw.Write(this._gridOffset);
                bw.Write(this.Width);
                bw.Write(this.Heigth);
                this.Sun.Write(bw);

                foreach(ColorRGBAVector4Byte color in this.Grid)
                {
                    color.Write(bw);
                }
            }
        }
    }
}
