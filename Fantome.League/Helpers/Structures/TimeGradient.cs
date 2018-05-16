using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantome.Libraries.League.Helpers.Structures
{
    /// <summary>
    /// Represents a Time Gradient
    /// </summary>
    public class TimeGradient
    {
        /// <summary>
        /// Type of this <see cref="TimeGradient"/>
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// The Values of this <see cref="TimeGradient"/>
        /// </summary>
        public TimeGradientValue[] Values { get; private set; } = new TimeGradientValue[8];

        /// <summary>
        /// Initializes a new <see cref="TimeGradient"/>
        /// </summary>
        /// <param name="type">Type of this <see cref="TimeGradient"/></param>
        /// <param name="values">The Values of this <see cref="TimeGradient"/></param>
        public TimeGradient(int type, TimeGradientValue[] values)
        {
            if (values.Length != 8)
            {
                throw new ArgumentException("There must be 8 values");
            }

            this.Type = type;
            this.Values = values;
        }

        /// <summary>
        /// Initializes a new <see cref="TimeGradient"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        public TimeGradient(BinaryReader br)
        {
            this.Type = br.ReadInt32();
            uint usedValueCount = br.ReadUInt32();
            for (int i = 0; i < usedValueCount; i++)
            {
                this.Values[i] = new TimeGradientValue(br);
            }

            for (int i = 0; i < 8 - usedValueCount; i++)
            {
                TimeGradientValue value = new TimeGradientValue(br);
            }
        }

        public uint GetValueCount()
        {
            uint count = 0;
            for (int i = 0; i < 8; i++)
            {
                if (this.Values[i] != null)
                {
                    count++;
                }
            }

            return count;
        }

        public Vector4 GetValue(float time)
        {
            if (time > 0)
            {
                uint valueCount = GetValueCount();

                if (time < 1)
                {
                    int gradientValueIndex = 0;
                    float accValue = 0;

                    for (int i = 0; ; i++)
                    {
                        accValue = this.Values[gradientValueIndex].Value.Y;
                        if (i >= valueCount || this.Values[gradientValueIndex].Time >= time)
                        {
                            break;
                        }

                        gradientValueIndex++;
                    }

                    float v7 = (time - this.Values[gradientValueIndex - 1].Value.Y) / (accValue - this.Values[gradientValueIndex - 1].Value.Y);
                    float v8 = ((this.Values[gradientValueIndex].Value.Z - this.Values[gradientValueIndex - 1].Value.Z) * v7) + this.Values[gradientValueIndex - 1].Value.Z;
                    float v9 = ((this.Values[gradientValueIndex].Value.W - this.Values[gradientValueIndex - 1].Value.W) * v7) + this.Values[gradientValueIndex - 1].Value.W;
                    float v10 = ((this.Values[gradientValueIndex + 1].Time - this.Values[gradientValueIndex].Time) * v7) * this.Values[gradientValueIndex].Time;

                    return new Vector4(v8, v9, v10, 0);
                }
                else
                {
                    return this.Values[valueCount].Value;
                }
            }
            else
            {
                return this.Values[0].Value;
            }
        }

        /// <summary>
        /// Writes this <see cref="TimeGradient"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        public void Write(BinaryWriter bw)
        {
            bw.Write(this.Type);

            uint usedValueCount = 0;
            for (int i = 0; i < 8; i++)
            {
                usedValueCount += this.Values[i] == null ? (uint)1 : 0;
            }
            bw.Write(usedValueCount);

            foreach (TimeGradientValue value in this.Values)
            {
                if (value != null)
                {
                    value.Write(bw);
                }
                else
                {
                    new TimeGradientValue(0, new Vector4(0, 0, 0, 0)).Write(bw);
                }
            }
        }
    }

    /// <summary>
    /// Represents a value inside a <see cref="TimeGradient"/>
    /// </summary>
    public class TimeGradientValue
    {
        /// <summary>
        /// The time at which this value starts
        /// </summary>
        public float Time { get; set; }

        /// <summary>
        /// The value of this <see cref="TimeGradientValue"/>
        /// </summary>
        public Vector4 Value { get; set; }

        /// <summary>
        /// Initializes a new <see cref="TimeGradientValue"/>
        /// </summary>
        /// <param name="time">The time at which this value starts</param>
        /// <param name="value">The value of this <see cref="TimeGradientValue"/></param>
        public TimeGradientValue(float time, Vector4 value)
        {
            if (time > 1 || time < 0)
            {
                throw new ArgumentException("Time must be normalized in a range from 0 - 1");
            }

            this.Time = time;
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new <see cref="TimeGradientValue"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        public TimeGradientValue(BinaryReader br)
        {
            this.Time = br.ReadSingle();
            this.Value = new Vector4(br);
        }

        /// <summary>
        /// Writes this <see cref="TimeGradientValue"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        public void Write(BinaryWriter bw)
        {
            bw.Write(this.Time);
            this.Value.Write(bw);
        }
    }
}
