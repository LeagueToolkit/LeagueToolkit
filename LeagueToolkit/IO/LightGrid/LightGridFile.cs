using LeagueToolkit.Helpers.Exceptions;
using LeagueToolkit.Helpers.Extensions;
using LeagueToolkit.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LeagueToolkit.IO.LightGrid
{
    public class LightGridFile
    {
        public uint Width { get; private set; }
        public uint Heigth { get; private set; }
        public float XBound { get; private set; }
        public float YBound { get; private set; }
        public LightGridSun Sun { get; private set; }
        public List<Color[]> Lights { get; private set; } = new List<Color[]>();


        public LightGridFile(string fileLocation)
            : this(File.OpenRead(fileLocation))
        {

        }
        public LightGridFile(Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream))
            {
                uint version = br.ReadUInt32();
                if (version != 3)
                {
                    throw new InvalidFileSignatureException();
                }

                uint gridOffset = br.ReadUInt32();
                this.Width = br.ReadUInt32();
                this.Heigth = br.ReadUInt32();
                this.XBound = br.ReadSingle();
                this.YBound = br.ReadSingle();
                this.Sun = new LightGridSun(br);

                br.BaseStream.Seek(gridOffset, SeekOrigin.Begin);
                uint cellCount = this.Width * this.Heigth;
                for (int i = 0; i < cellCount; i++)
                {
                    this.Lights.Add(new Color[]
                    {
                        br.ReadColor(ColorFormat.RgbaU8), br.ReadColor(ColorFormat.RgbaU8), br.ReadColor(ColorFormat.RgbaU8),
                        br.ReadColor(ColorFormat.RgbaU8), br.ReadColor(ColorFormat.RgbaU8), br.ReadColor(ColorFormat.RgbaU8)
                    });
                }
            }
        }

        public void Write(string fileLocation)
        {
            Write(File.Create(fileLocation));
        }
        public void Write(Stream stream, bool leaveOpen = false)
        {
            using (BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8, leaveOpen))
            {
                bw.Write((uint)3);
                bw.Write((uint)76);
                bw.Write(this.Width);
                bw.Write(this.Heigth);
                bw.Write(this.XBound);
                bw.Write(this.YBound);
                this.Sun.Write(bw);

                foreach (Color[] cell in this.Lights)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        bw.WriteColor(cell[i], ColorFormat.RgbaU8);
                    }
                }
            }
        }

        public void WriteTexture(string fileLocation)
        {
            using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(fileLocation)))
            {
                bw.Write((byte)0); //ID Length
                bw.Write((byte)0); //ColorMap Type
                bw.Write((byte)2); //DataType Code
                bw.Write((uint)0); //ColorMap Origin and Length
                bw.Write((byte)0); //ColorMap Depth
                bw.Write((uint)0); //X and Y Origin
                bw.Write((ushort)this.Width);
                bw.Write((ushort)this.Heigth);
                bw.Write((byte)32); //Bits Per Color
                bw.Write((byte)0); //Image Descriptor

                foreach (Color[] cell in this.Lights)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        bw.WriteColor(cell[i], ColorFormat.RgbaU8);
                    }
                }
            }
        }
    }
}
