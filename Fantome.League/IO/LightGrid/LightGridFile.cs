using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.IO.LightGrid
{
    public class LightGridFile
    {
        private uint _headerVersion = 1;
        public uint Width { get; private set; }
        public uint Height { get; private set; }
        public Vector2 BoundsMax { get; set; }
        public float CharacterIntensity { get; set; }
        public float CharacterFullBrightIntensity { get; set; }
        public float Unknown2 { get; set; }
        public float Unknown3 { get; set; }
        public float Unknown4 { get; set; }
        public float Unknown5 { get; set; }
        public float Unknown6 { get; set; }
        public float Unknown7 { get; set; }
        public float Unknown8 { get; set; }
        public float Unknown9 { get; set; }
        public float Unknown10 { get; set; }
        public float Unknown11 { get; set; }
        public float Unknown12 { get; set; }
        public List<ColorRGBAVector4Byte[]> Grid { get; private set; } = new List<ColorRGBAVector4Byte[]>();

        public LightGridFile(string fileLocation) : this(File.OpenRead(fileLocation)) { }

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
                this.Height = br.ReadUInt32();
                this.BoundsMax = new Vector2(br);
                this.CharacterIntensity = br.ReadSingle();
                this.CharacterFullBrightIntensity = br.ReadSingle();

                if (gridOffset == 76)
                {
                    this._headerVersion = 2;

                    this.Unknown2 = br.ReadSingle();
                    this.Unknown3 = br.ReadSingle();
                    this.Unknown4 = br.ReadSingle();
                    this.Unknown5 = br.ReadSingle();
                    this.Unknown6 = br.ReadSingle();
                    this.Unknown7 = br.ReadSingle();
                    this.Unknown8 = br.ReadSingle();
                    this.Unknown9 = br.ReadSingle();
                    this.Unknown10 = br.ReadSingle();
                    this.Unknown11 = br.ReadSingle();
                    this.Unknown12 = br.ReadSingle();
                }

                br.BaseStream.Seek(gridOffset, SeekOrigin.Begin);
                uint cellCount = this.Width * this.Height;
                for (int i = 0; i < cellCount; i++)
                {
                    this.Grid.Add(new ColorRGBAVector4Byte[]
                        {
                            new ColorRGBAVector4Byte(br), new ColorRGBAVector4Byte(br), new ColorRGBAVector4Byte(br),
                            new ColorRGBAVector4Byte(br), new ColorRGBAVector4Byte(br), new ColorRGBAVector4Byte(br)
                        });
                }
            }
        }

        public void Write(Stream stream)
        {
            using (BinaryWriter bw = new BinaryWriter(stream))
            {
                bw.Write((uint)3);
                bw.Write(this._headerVersion == 1 ? 76 : 32);
                bw.Write(this.Width);
                bw.Write(this.Height);
                this.BoundsMax.Write(bw);
                bw.Write(this.CharacterIntensity);
                bw.Write(this.CharacterFullBrightIntensity);

                if (this._headerVersion == 2)
                {
                    bw.Write(this.Unknown2);
                    bw.Write(this.Unknown3);
                    bw.Write(this.Unknown4);
                    bw.Write(this.Unknown5);
                    bw.Write(this.Unknown6);
                    bw.Write(this.Unknown7);
                    bw.Write(this.Unknown8);
                    bw.Write(this.Unknown9);
                    bw.Write(this.Unknown10);
                    bw.Write(this.Unknown11);
                    bw.Write(this.Unknown12);
                }

                foreach (ColorRGBAVector4Byte[] cell in this.Grid)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        cell[i].Write(bw);
                    }
                }
            }
        }

        public void Write(string fileLocation)
        {
            Write(File.OpenWrite(fileLocation));
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
                bw.Write((ushort)this.Height);
                bw.Write((byte)32); //Bits Per Color
                bw.Write((byte)0); //Image Descriptor

                foreach (ColorRGBAVector4Byte[] cell in this.Grid)
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
