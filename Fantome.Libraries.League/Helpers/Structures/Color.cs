using System;
using System.Collections.Generic;
using System.Text;

namespace Fantome.Libraries.League.Helpers.Structures
{
    public struct Color
    {
        public float R
        {
            get => this._r;
            set
            {
                if (value < 0 || value > 1) throw new ArgumentOutOfRangeException("value", "value must be in 0-1 range");
                else this._r = value;
            }
        }
        public float G
        {
            get => this._g;
            set
            {
                if (value < 0 || value > 1) throw new ArgumentOutOfRangeException("value", "value must be in 0-1 range");
                else this._g = value;
            }
        }
        public float B
        {
            get => this._b;
            set
            {
                if (value < 0 || value > 1) throw new ArgumentOutOfRangeException("value", "value must be in 0-1 range");
                else this._b = value;
            }
        }
        public float A
        {
            get => this._a;
            set
            {
                if (value < 0 || value > 1) throw new ArgumentOutOfRangeException("value", "value must be in 0-1 range");
                else this._a = value;
            }
        }

        private float _r;
        private float _g;
        private float _b;
        private float _a;

        public Color(byte r, byte g, byte b) : this(r, g, b, 1) { }
        public Color(byte r, byte g, byte b, byte a)
        {
            this._r = r / 255f;
            this._g = g / 255f;
            this._b = b / 255f;
            this._a = a / 255f;
        }
        public Color(float r, float g, float b) : this(r, g, b, 1) { }
        public Color(float r, float g, float b, float a)
        {
            if (r < 0 || r > 1) Math.Clamp(r, 0, 1);
            if (g < 0 || g > 1) Math.Clamp(g, 0, 1);
            if (b < 0 || b > 1) Math.Clamp(b, 0, 1);
            if (a < 0 || a > 1) Math.Clamp(a, 0, 1);

            this._r = r;
            this._g = g;
            this._b = b;
            this._a = a;
        }

        public static int FormatSize(ColorFormat format)
        {
            switch (format)
            {
                case ColorFormat.RgbU8:
                case ColorFormat.BgrU8:
                    return 3;
                case ColorFormat.RgbaU8:
                case ColorFormat.BgraU8:
                    return 4;
                case ColorFormat.RgbF32:
                case ColorFormat.BgrF32:
                    return 12;
                case ColorFormat.RgbaF32:
                case ColorFormat.BgraF32:
                    return 16;
                default:
                    throw new ArgumentException("Unsupported format", nameof(format));
            }
        }
    }

    public enum ColorFormat
    {
        RgbU8,
        RgbF32,
        RgbaU8,
        RgbaF32,
        BgrU8,
        BgrF32,
        BgraU8,
        BgraF32
    }
}
