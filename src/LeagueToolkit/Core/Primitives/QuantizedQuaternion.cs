using CommunityToolkit.HighPerformance;
using System;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;

namespace LeagueToolkit.Core.Primitives
{
    public static class QuantizedQuaternion
    {
        private const double SQRT_2 = 1.41421356237;
        private const double ONE_OVER_SQRT_2 = 0.70710678118;

        public static void Compress(Quaternion quat, Span<byte> compressed)
        {
            uint maxIndex = 3;
            float x_abs = Math.Abs(quat.X);
            float y_abs = Math.Abs(quat.Y);
            float z_abs = Math.Abs(quat.Z);
            float w_abs = Math.Abs(quat.W);
            if (x_abs >= w_abs && x_abs >= y_abs && x_abs >= z_abs)
            {
                maxIndex = 0;
                if (quat.X < 0f)
                    quat *= -1;
            }
            else if (y_abs >= w_abs && y_abs >= x_abs && y_abs >= z_abs)
            {
                maxIndex = 1;
                if (quat.Y < 0f)
                    quat *= -1;
            }
            else if (z_abs >= w_abs && z_abs >= x_abs && z_abs >= y_abs)
            {
                maxIndex = 2;
                if (quat.Z < 0f)
                    quat *= -1;
            }
            else if (quat.W < 0f)
                quat *= -1;

            Span<float> components = stackalloc float[] { quat.X, quat.Y, quat.Z, quat.W };
            ulong bits = (ulong)maxIndex << 45;
            for (int i = 0, compressedIndex = 0; i < 4; i++)
            {
                if (i == maxIndex)
                    continue;

                ushort component = (ushort)Math.Round(32767.0 / 2.0 * (SQRT_2 * components[i] + 1.0));
                bits |= (ulong)(component & 0b0111_1111_1111_1111) << (15 * (2 - compressedIndex));

                compressedIndex++;
            }

            for (int i = 0; i < 6; i++)
            {
                compressed[i] = (byte)((bits >> (8 * i)) & 0b1111_1111);
            }
        }

        public static Quaternion Decompress(ReadOnlySpan<byte> data) => Decompress(data.Cast<byte, ushort>());

        public static Quaternion Decompress(ReadOnlySpan<ushort> data)
        {
            ulong bits = (ulong)(data[0]) | (ulong)data[1] << 16 | (ulong)data[2] << 32;
            ushort maxIndex = (ushort)(bits >> 45 & 0x0003u);
            ushort v_a = (ushort)(bits >> 30 & 0x7FFFu);
            ushort v_b = (ushort)(bits >> 15 & 0x7FFFu);
            ushort v_c = (ushort)(bits & 0x7FFFu);

            float a = (float)(v_a / 32767.0 * SQRT_2 - ONE_OVER_SQRT_2);
            float b = (float)(v_b / 32767.0 * SQRT_2 - ONE_OVER_SQRT_2);
            float c = (float)(v_c / 32767.0 * SQRT_2 - ONE_OVER_SQRT_2);
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
