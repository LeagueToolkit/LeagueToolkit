using LeagueToolkit.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.IO;

namespace LeagueToolkit.IO.NavigationGridOverlay
{
    public class NavigationGridOverlayRegion
    {
        public uint X { get; set; }
        public uint Y { get; set; }
        public uint Width { get; set; }
        public uint Height { get; set; }
        public List<List<NavigationGridCellFlags>> CellFlags { get; set; }

        public NavigationGridOverlayRegion(BinaryReader br)
        {
            this.X = br.ReadUInt32();
            this.Y = br.ReadUInt32();
            this.Width = br.ReadUInt32();
            this.Height = br.ReadUInt32();

            this.CellFlags = new List<List<NavigationGridCellFlags>>((int)this.Height);
            for(int i = 0; i < this.Height; i++)
            {
                var line = new List<NavigationGridCellFlags>((int)this.Width);
                for (int j = 0; j < this.Width; j++)
                {
                    line.Add((NavigationGridCellFlags)br.ReadUInt16());
                }

                this.CellFlags.Add(line);
            }
        }
    }

    [Flags]
    public enum NavigationGridCellFlags : ushort
    {
        HAS_GRASS = 0x1,
        NOT_PASSABLE = 0x2,
        BUSY = 0x4,
        TARGETTED = 0x8,
        MARKED = 0x10,
        PATHED_ON = 0x20,
        SEE_THROUGH = 0x40,
        OTHER_DIRECTION_END_TO_START = 0x80,
        HAS_GLOBAL_VISION = 0x100,
        // HAS_TRANSPARENT_TERRAIN = 0x42 // (SeeThrough | NotPassable)
    }
}