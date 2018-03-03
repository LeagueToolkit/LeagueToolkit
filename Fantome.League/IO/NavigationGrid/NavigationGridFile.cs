using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantome.Libraries.League.IO.NavigationGrid
{
    public class NavigationGridFile
    {
        public R3DBox BoundingBox { get; private set; }
        public float CellSize { get; private set; }
        public uint Width { get; private set; }
        public uint Height { get; private set; }
        public uint SampledHeightsWidth { get; private set; }
        public uint SampledHeightsHeight { get; private set; }
        public float UnknownFloat1 { get; private set; }
        public float UnknownFloat2 { get; private set; }
        public List<List<NavigationGridCell>> Cells { get; private set; } = new List<List<NavigationGridCell>>();
        public List<List<uint>> Unknown { get; private set; } = new List<List<uint>>();
        public List<byte[]> Unknown2 { get; private set; } = new List<byte[]>(32);
        public List<float> SampledHeights { get; private set; } = new List<float>();
        public List<float> Unknown4 { get; private set; } = new List<float>(810900);

        public NavigationGridFile(string fileLocation) : this(File.OpenRead(fileLocation)) { }

        public NavigationGridFile(Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream))
            {
                byte major = br.ReadByte();
                ushort minor = 0;
                if (major > 2)
                {
                    minor = br.ReadUInt16();
                }

                this.BoundingBox = new R3DBox(br);
                this.CellSize = br.ReadSingle();
                this.Width = br.ReadUInt32();
                this.Height = br.ReadUInt32();

                for (int i = 0; i < this.Width; i++)
                {
                    this.Cells.Add(new List<NavigationGridCell>());

                    for (int j = 0; j < this.Height; j++)
                    {
                        this.Cells[i].Add(new NavigationGridCell(br, major));
                    }
                }

                if (major == 7)
                {
                    for (int i = 0; i < this.Width; i++)
                    {
                        for (int j = 0; j < this.Height; j++)
                        {
                            this.Cells[i][j].Flags = br.ReadUInt16();
                        }
                    }
                }

                for (int i = 0; i < this.Width; i++)
                {
                    this.Unknown.Add(new List<uint>((int)this.Height));

                    for (int j = 0; j < this.Height; j++)
                    {
                        if (major < 4)
                        {
                            this.Unknown[i].Add(0);
                        }
                        else if (major < 7)
                        {
                            this.Unknown[i].Add(br.ReadUInt16());
                        }
                        else
                        {
                            this.Unknown[i].Add(br.ReadUInt32());
                        }
                    }
                }

                if (major < 5)
                {
                    for (int i = 0; i < 32; i++)
                    {
                        this.Unknown2.Add(new byte[33]);
                    }
                }
                else if (major == 5)
                {
                    for (int i = 0; i < 16; i++)
                    {
                        this.Unknown2.Add(br.ReadBytes(33));
                    }
                }
                else if (major >= 7)
                {
                    for (int i = 0; i < 32; i++)
                    {
                        this.Unknown2.Add(br.ReadBytes(33));
                    }
                }

                this.SampledHeightsWidth = br.ReadUInt32();
                this.SampledHeightsHeight = br.ReadUInt32();
                this.UnknownFloat1 = br.ReadSingle();
                this.UnknownFloat2 = br.ReadSingle();

                this.SampledHeights.Capacity = (int)(this.SampledHeightsWidth * this.SampledHeightsHeight);
                for (int i = 0; i < this.SampledHeightsWidth * this.SampledHeightsHeight; i++)
                {
                    this.SampledHeights.Add(br.ReadSingle());
                }

                for (int i = 0; i < 810900; i++)
                {
                    this.Unknown4.Add(br.ReadSingle());
                }

                foreach (List<NavigationGridCell> row in this.Cells)
                {
                    foreach (NavigationGridCell cell in row)
                    {
                        if (FDTest(cell.RefHintWeight) == 2)
                        {
                            cell.RefHintWeight = 0.5f;
                        }
                    }
                }
            }
        }

        private ushort FDTest(float f)
        {
            uint floatInt = BitConverter.ToUInt32(BitConverter.GetBytes(f), 0);
            uint mantissaBlock = floatInt & 0x0000FFFF;
            uint v2 = mantissaBlock & 0x7F80;

            if (v2 == 0x7F80)
            {
                if ((mantissaBlock & 0x7F) != 0 || floatInt != 0)
                {
                    return 2;
                }
                else
                {
                    return 1;
                }
            }
            else if ((mantissaBlock & 0xFFFF7FFF) != 0 || floatInt != 0)
            {
                return (ushort)(((v2 != 0) ? 1 : 0) - 2);
            }
            else
            {
                return 0;
            }
        }
    }
}
