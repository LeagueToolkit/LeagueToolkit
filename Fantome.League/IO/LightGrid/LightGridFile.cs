using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.IO.LightGrid
{
    public class LightGridFile
    {
        public uint Width { get; private set; }
        public uint Heigth { get; private set; }
        public float XBound { get; private set; }
        public float YBound { get; private set; }
        public LightGridSun Sun { get; private set; }
        public List<ColorRGBAVector4Byte[]> Lights { get; private set; } = new List<ColorRGBAVector4Byte[]>();


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
                    throw new Exception("This is not a valid Light Grid file");
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
                    this.Lights.Add(new ColorRGBAVector4Byte[]
                        {
                            new ColorRGBAVector4Byte(br), new ColorRGBAVector4Byte(br), new ColorRGBAVector4Byte(br),
                            new ColorRGBAVector4Byte(br), new ColorRGBAVector4Byte(br), new ColorRGBAVector4Byte(br)
                        });
                }
            }
        }

        public void Write(string fileLocation)
        {
            Write(File.Create(fileLocation));
        }
        public void Write(Stream stream)
        {
            using (BinaryWriter bw = new BinaryWriter(stream))
            {
                bw.Write((uint)3);
                bw.Write((uint)76);
                bw.Write(this.Width);
                bw.Write(this.Heigth);
                bw.Write(this.XBound);
                bw.Write(this.YBound);
                this.Sun.Write(bw);

                foreach (ColorRGBAVector4Byte[] cell in this.Lights)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        cell[i].Write(bw);
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

                foreach (ColorRGBAVector4Byte[] cell in this.Lights)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        cell[i].Write(bw);
                    }
                }
            }
        }
    }
}
