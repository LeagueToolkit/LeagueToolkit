using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Fantome.Libraries.League.IO.FX
{
    [DebuggerDisplay("[ Type: {Type} ]")]
    public class FXTimeGradient
    {
        public Int32 Type { get; private set; }
        public UInt32 UsedValueCount { get; private set; }
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
            foreach (Value Value in this.Values)
            {
                Value.Write(bw);
            }
        }
        [DebuggerDisplay("[ {Time}, [ {Values[0]}, {Values[1]}, {Values[2]}, {Values[3]} ] ]")]
        public struct Value
        {
            public float Time;
            public float[] Values;

            public Value(BinaryReader br)
            {
                this.Time = br.ReadSingle();
                this.Values = new float[4];
                for (int i = 0; i < 4; i++)
                {
                    this.Values[i] = br.ReadSingle();
                }
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
