using System;
using System.Numerics;

namespace LeagueToolkit.Core.Primitives
{
    public static class QuantizedQuaternion
    {
        public static Quaternion Decompress(ReadOnlySpan<byte> data)
        {
            ulong bits =
                0
                | (ulong)(ushort)(data[0] | (data[1] << 8))
                | (ulong)(ushort)(data[2] | (data[3] << 8)) << 16
                | (ulong)(ushort)(data[4] | (data[5] << 8)) << 32;
            ushort maxIndex = (ushort)(bits >> 45 & 0x0003u);
            ushort v_a = (ushort)(bits >> 30 & 0x7FFFu);
            ushort v_b = (ushort)(bits >> 15 & 0x7FFFu);
            ushort v_c = (ushort)(bits & 0x7FFFu);

            double sqrt2 = 1.41421356237;
            float a = (float)(v_a / 32767.0 * sqrt2 - 1 / sqrt2);
            float b = (float)(v_b / 32767.0 * sqrt2 - 1 / sqrt2);
            float c = (float)(v_c / 32767.0 * sqrt2 - 1 / sqrt2);
            float sub = Math.Max(0, 1 - (a * a + b * b + c * c));
            float d = (float)Math.Sqrt(sub);

            return maxIndex switch
            {
                0 => new(d, a, b, c),
                1 => new(a, d, b, c),
                2 => new(a, b, d, c),
                _ => new(a, b, c, d),
            };
        }
    }
}
