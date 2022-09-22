using System;
using System.Linq;
using System.Numerics;

namespace LeagueToolkit.Helpers.Structures
{
    public readonly struct QuantizedQuaternion : IEquatable<QuantizedQuaternion>, IComparable<QuantizedQuaternion>
    {
        private readonly ushort[] _data;

        public QuantizedQuaternion(byte[] data)
        {
            this._data = new ushort[] 
            {
                (ushort)(data[0] | (data[1] << 8)),
                (ushort)(data[2] | (data[3] << 8)),
                (ushort)(data[4] | (data[5] << 8)),
            };
        }

        public Quaternion Decompress()
        {
            ulong bits = (ulong)this._data[0] | (ulong)this._data[1] << 16 | (ulong)this._data[2] << 32;
            ushort maxIndex = (ushort)((bits >> 45) & 0x0003u);
            ushort v_a = (ushort)((bits >> 30) & 0x7FFFu);
            ushort v_b = (ushort)((bits >> 15) & 0x7FFFu);
            ushort v_c = (ushort)(bits & 0x7FFFu);

            const double sqrt2 = 1.41421356237;
            float a = (float)((v_a / 32767.0) * sqrt2 - 1 / sqrt2);
            float b = (float)((v_b / 32767.0) * sqrt2 - 1 / sqrt2);
            float c = (float)((v_c / 32767.0) * sqrt2 - 1 / sqrt2);
            float sub = Math.Max(0, 1 - (a * a + b * b + c * c));
            float d = (float)Math.Sqrt(sub);

            return maxIndex switch
            {
                0 => new Quaternion(d, a, b, c),
                1 => new Quaternion(a, d, b, c),
                2 => new Quaternion(a, b, d, c),
                _ => new Quaternion(a, b, c, d)
            };
        }

        public static QuantizedQuaternion Compress(Quaternion quaternion)
        {
            double sqrt2 = Math.Sqrt(2);
            float[] quaternionFloats = { quaternion.X, quaternion.Y, quaternion.Z, quaternion.W };
            if (quaternionFloats.Any(f => -f > 1 / sqrt2))
                quaternionFloats = quaternionFloats.Select(f => -f).ToArray(); // no float is allowed with a value of < -1 / sqrt(2)
            int maxIndex = Array.IndexOf(quaternionFloats, quaternionFloats.Max());
            ulong bits = (ulong)((long)maxIndex << 45);

            for (int i = 0, compressedIndex = 0; i < 4; i++)
            {
                if (i == maxIndex) continue;
                ushort compressedValue = (ushort)Math.Round(32767 / 2.0 * (sqrt2 * quaternionFloats[i] + 1));
                bits |= (ulong)(compressedValue & 0x7FFF) << (15 * (2 - compressedIndex));

                compressedIndex++;
            }

            return new QuantizedQuaternion(BitConverter.GetBytes(bits).Take(6).ToArray());
        }

        public byte[] GetBytes() => BitConverter.GetBytes(this._data[0] | (ulong)this._data[1] << 16 | (ulong)this._data[2] << 32).Take(6).ToArray();

        public bool Equals(QuantizedQuaternion other) => _data.SequenceEqual(other._data);

        public int CompareTo(QuantizedQuaternion other) =>
            this._data[0] == other._data[0]
                ? this._data[1] == other._data[1]
                    ? this._data[2].CompareTo(other._data[2])
                    : this._data[1].CompareTo(other._data[1])
                : this._data[0].CompareTo(other._data[0]);

        public override bool Equals(object obj) => obj is QuantizedQuaternion other && Equals(other);

        public override int GetHashCode() => _data != null ? _data[0] + _data[1] * 17 + _data[2] * 661 : 0;
    }
}
