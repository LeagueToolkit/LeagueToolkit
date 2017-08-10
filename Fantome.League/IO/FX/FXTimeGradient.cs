using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.IO.FX
{
    public class FXTimeGradient
    {
        public int Type { get; private set; }
        public uint UsedValueCount { get; private set; }
        public List<Value> Values { get; private set; } = new List<Value>();

        public FXTimeGradient(BinaryReader br)
        {
            this.Type = br.ReadInt32();
            this.UsedValueCount = br.ReadUInt32();
            for (int i = 0; i < 8; i++)
            {
                this.Values.Add(new Value(br));
            }
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(this.Type);
            bw.Write(this.UsedValueCount);
            foreach (Value value in this.Values)
            {
                value.Write(bw);
            }
        }
        public struct Value
        {
            public float Time { get; private set; }
            public float[] Values { get; private set; }

            public Value(BinaryReader br)
            {
                this.Time = br.ReadSingle();
                this.Values = new float[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
            }

            public void Write(BinaryWriter bw)
            {
                bw.Write(this.Time);
                for (int i = 0; i < 4; i++)
                {
                    bw.Write(this.Values[i]);
                }
            }
        }
    }
}
