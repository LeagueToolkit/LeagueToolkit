using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.Helpers.Structures
{
    public struct QuantizedQuaternion
    {
        private ushort[] _data;

        public QuantizedQuaternion(BinaryReader br)
        {
            this._data = new ushort[]
            {
                br.ReadUInt16(),
                br.ReadUInt16(),
                br.ReadUInt16()
            };
        }

        public Quaternion Decompress()
        {
            ulong bits = (ulong)this._data[0] | (ulong)this._data[1] << 16 | (ulong)this._data[2] << 32;
            ushort maxIndex = (ushort)((bits >> 45) & 0x0003u);
            ushort v_a = (ushort)((bits >> 30) & 0x7FFFu);
            ushort v_b = (ushort)((bits >> 15) & 0x7FFFu);
            ushort v_c = (ushort)(bits & 0x7FFFu);

            double sqrt2 = Math.Sqrt(2);
            float a = (float)((v_a / 32767.0) * sqrt2 - 1 / sqrt2);
            float b = (float)((v_b / 32767.0) * sqrt2 - 1 / sqrt2);
            float c = (float)((v_c / 32767.0) * sqrt2 - 1 / sqrt2);
            float sub = Math.Max(0, 1 - (a * a + b * b + c * c));
            float d = (float)Math.Sqrt(sub);

            switch(maxIndex)
            {
                case 0: return new Quaternion(d, a, b, c);
                case 1: return new Quaternion(a, d, b, c);
                case 2: return new Quaternion(a, b, d, c);
                default: return new Quaternion(a, b, c, d);
            }
        }
    }
}
