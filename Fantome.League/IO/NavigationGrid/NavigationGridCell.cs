using System.IO;

namespace Fantome.Libraries.League.IO.NavigationGrid
{
    public class NavigationGridCell
    {
        public float CenterHeight { get; private set; }
        public int SessionID { get; private set; } = -1;
        public float ArrivalCost { get; private set; }
        public bool IsOpen { get; private set; }
        public float Heuristic { get; private set; }
        public uint ActorList { get; private set; }
        public uint Unknown1 { get; private set; }
        public ushort X { get; private set; } = 32768;
        public ushort Y { get; private set; } = 32768;
        public float AdditionalCost { get; private set; }
        public float HintAsGoodCell { get; private set; }
        public int AdditionalCostRefCount { get; private set; }
        public int GoodCellSessionID { get; private set; } = -1;
        public float RefHintWeight { get; set; } = 0.5f;
        public ushort Unknown2 { get; private set; }
        public ushort ArrivalDirection { get; private set; } = 9;
        public NavigationGridCellFlags Flags { get; set; }
        public short[] RefHintNodes { get; private set; } = new short[2] { -1, -1 };

        public NavigationGridCell(BinaryReader br, byte version)
        {
            this.CenterHeight = br.ReadSingle();
            this.SessionID = br.ReadInt32();
            this.ArrivalCost = br.ReadSingle();
            this.IsOpen = br.ReadUInt32() == 1;
            if (version < 6)
            {
                this.Heuristic = br.ReadSingle();
                this.ActorList = br.ReadUInt32();
                this.X = br.ReadUInt16();
                this.Y = br.ReadUInt16();
                this.AdditionalCost = br.ReadSingle();
                this.HintAsGoodCell = br.ReadSingle();
                this.AdditionalCostRefCount = br.ReadInt32();
                this.GoodCellSessionID = br.ReadInt32();
                this.RefHintWeight = br.ReadSingle();
                this.ArrivalDirection = br.ReadUInt16();
                this.Flags = (NavigationGridCellFlags)br.ReadUInt16();
                for (int i = 0; i < 2; i++)
                {
                    this.RefHintNodes[i] = br.ReadInt16();
                }
            }
            else if (version == 7)
            {
                this.Heuristic = br.ReadSingle();
                this.X = br.ReadUInt16();
                this.Y = br.ReadUInt16();
                this.ActorList = br.ReadUInt32();
                this.Unknown1 = br.ReadUInt32();
                this.GoodCellSessionID = br.ReadInt32();
                this.RefHintWeight = br.ReadSingle();
                this.Unknown2 = br.ReadUInt16();
                this.ArrivalDirection = br.ReadUInt16();
                for (int i = 0; i < 2; i++)
                {
                    this.RefHintNodes[i] = br.ReadInt16();
                }
            }
        }
    }

    public enum NavigationGridCellFlags : ushort
    {
        CELL_HAS_GRASS = 0x1,
        CELL_NOT_PASSABLE = 0x2,
        CELL_BUSY = 0x4,
        CELL_TARGETTED = 0x8,
        CELL_MARKED = 0x10,
        CELL_PATHED_ON = 0x20,
        CELL_SEE_THROUGH = 0x40,
        CELL_OTHERDIRECTION_END_TO_START = 0x80,
        CELL_HAS_ANTI_BRUSH = 0x100,
        CELL_HAS_TRANSPARENTTERRAIN = 0x42,
    }
}
