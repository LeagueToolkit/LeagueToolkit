using LeagueToolkit.Utils.Extensions;
using System;
using System.IO;
using System.Numerics;

namespace LeagueToolkit.Core.Primitives
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
        /// Creates a clone of a <see cref="TimeGradient"/> object
        /// </summary>
        /// <param name="timeGradient">The <see cref="TimeGradient"/> to clone</param>
        public TimeGradient(TimeGradient timeGradient)
        {
            this.Type = timeGradient.Type;
            for (int i = 0; i < timeGradient.Values.Length; i++)
            {
                this.Values[i] = new TimeGradientValue(timeGradient.Values[i]);
            }
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

        /// <summary>
        /// Gets the <see cref="TimeGradientValue"/> count of this <see cref="TimeGradient"/>
        /// </summary>
        /// <remarks>This method should be used instead of assuming each gradient has 8 values, as most of the gradients won't have all 8 values set</remarks>
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

        /// <summary>
        /// Gets a value from this <see cref="TimeGradient"/> at the specified time
        /// </summary>
        /// <param name="time">The time at which to get the value</param>
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

                    float minTime = this.Values[gradientValueIndex - 1].Time;
                    float maxTime = this.Values[gradientValueIndex].Time;
                    float minX = this.Values[gradientValueIndex - 1].Value.X;
                    float minY = this.Values[gradientValueIndex - 1].Value.Y;
                    float minZ = this.Values[gradientValueIndex - 1].Value.Z;
                    float minW = this.Values[gradientValueIndex - 1].Value.W;
                    float maxX = this.Values[gradientValueIndex].Value.X;
                    float maxY = this.Values[gradientValueIndex].Value.Y;
                    float maxZ = this.Values[gradientValueIndex].Value.Z;
                    float maxW = this.Values[gradientValueIndex].Value.W;

                    float timeFraction = (time - minTime) / (maxTime - minTime);
                    float x = (maxX - minX) * timeFraction + minX;
                    float y = (maxY - minY) * timeFraction + minY;
                    float z = (maxZ - minZ) * timeFraction + minZ;
                    float w = (maxW - minW) * timeFraction + minW;

                    return new Vector4(x, y, z, w);
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
            this.Value = br.ReadVector4();
        }

        /// <summary>
        /// Creates a clone of a <see cref="TimeGradientValue"/> object
        /// </summary>
        /// <param name="timeGradientValue">The <see cref="TimeGradientValue"/> to clone</param>
        public TimeGradientValue(TimeGradientValue timeGradientValue)
        {
            this.Time = timeGradientValue.Time;
            this.Value = timeGradientValue.Value;
        }

        /// <summary>
        /// Writes this <see cref="TimeGradientValue"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        public void Write(BinaryWriter bw)
        {
            bw.Write(this.Time);
            bw.WriteVector4(this.Value);
        }
    }
}
