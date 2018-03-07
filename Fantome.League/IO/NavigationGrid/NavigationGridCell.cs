using System.IO;

namespace Fantome.Libraries.League.IO.NavigationGrid
{
    /// <summary>
    /// Represents a navigational cell inside of a <see cref="NavigationGridFile"/>
    /// </summary>
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
        HasGrass = 1 << 0,
        NotPassable = 1 << 1,
        Busy = 1 << 2,
        Targetted = 1 << 3,
        Marked = 1 << 4,
        PathedOn = 1 << 5,
        SeeThrough = 1 << 6,
        OtherdirectionEndToStart = 1 << 7,
        HasAntiBrush = 1 << 8,
        HasTransparentTerrain = NotPassable | SeeThrough
    }
}
