using System;
using System.Collections.Generic;
using System.Text;

namespace LeagueToolkit.Helpers.Structures
{
    public struct Color: IEquatable<Color>
    {
        public static readonly Color Zero = new Color(0, 0, 0, 0);

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

        public byte[] GetBytes(ColorFormat format)
        {
            int formatSize = FormatSize(format);
            byte[] colorBuffer = new byte[formatSize];

            if(format == ColorFormat.RgbU8)
            {
                colorBuffer[0] = (byte)(this.R * 255);
                colorBuffer[1] = (byte)(this.G * 255);
                colorBuffer[2] = (byte)(this.B * 255);
            }
            else if (format == ColorFormat.RgbaU8)
            {
                colorBuffer[0] = (byte)(this.R * 255);
                colorBuffer[1] = (byte)(this.G * 255);
                colorBuffer[2] = (byte)(this.B * 255);
                colorBuffer[3] = (byte)(this.A * 255);
            }
            else if (format == ColorFormat.BgrU8)
            {
                colorBuffer[0] = (byte)(this.B * 255);
                colorBuffer[1] = (byte)(this.G * 255);
                colorBuffer[2] = (byte)(this.R * 255);
            }
            else if (format == ColorFormat.BgraU8)
            {
                colorBuffer[0] = (byte)(this.B * 255);
                colorBuffer[1] = (byte)(this.G * 255);
                colorBuffer[2] = (byte)(this.R * 255);
                colorBuffer[3] = (byte)(this.A * 255);
            }
            else if(format == ColorFormat.RgbF32)
            {
                Buffer.BlockCopy(BitConverter.GetBytes(this.R), 0, colorBuffer, sizeof(float) * 0, sizeof(float));
                Buffer.BlockCopy(BitConverter.GetBytes(this.G), 0, colorBuffer, sizeof(float) * 1, sizeof(float));
                Buffer.BlockCopy(BitConverter.GetBytes(this.B), 0, colorBuffer, sizeof(float) * 2, sizeof(float));
            }
            else if (format == ColorFormat.RgbaF32)
            {
                Buffer.BlockCopy(BitConverter.GetBytes(this.R), 0, colorBuffer, sizeof(float) * 0, sizeof(float));
                Buffer.BlockCopy(BitConverter.GetBytes(this.G), 0, colorBuffer, sizeof(float) * 1, sizeof(float));
                Buffer.BlockCopy(BitConverter.GetBytes(this.B), 0, colorBuffer, sizeof(float) * 2, sizeof(float));
                Buffer.BlockCopy(BitConverter.GetBytes(this.A), 0, colorBuffer, sizeof(float) * 3, sizeof(float));
            }
            else if (format == ColorFormat.BgrF32)
            {
                Buffer.BlockCopy(BitConverter.GetBytes(this.B), 0, colorBuffer, sizeof(float) * 0, sizeof(float));
                Buffer.BlockCopy(BitConverter.GetBytes(this.G), 0, colorBuffer, sizeof(float) * 1, sizeof(float));
                Buffer.BlockCopy(BitConverter.GetBytes(this.R), 0, colorBuffer, sizeof(float) * 2, sizeof(float));
            }
            else if (format == ColorFormat.BgraF32)
            {
                Buffer.BlockCopy(BitConverter.GetBytes(this.B), 0, colorBuffer, sizeof(float) * 0, sizeof(float));
                Buffer.BlockCopy(BitConverter.GetBytes(this.G), 0, colorBuffer, sizeof(float) * 1, sizeof(float));
                Buffer.BlockCopy(BitConverter.GetBytes(this.R), 0, colorBuffer, sizeof(float) * 2, sizeof(float));
                Buffer.BlockCopy(BitConverter.GetBytes(this.A), 0, colorBuffer, sizeof(float) * 3, sizeof(float));
            }

            return colorBuffer;
        }

        public string ToString(ColorFormat format)
        {
            if (format == ColorFormat.RgbU8)
            {
                return string.Format("{0} {1} {2}", (byte)(this.R * 255), (byte)(this.G * 255), (byte)(this.B * 255));
            }
            else if(format == ColorFormat.RgbaU8)
            {
                return string.Format("{0} {1} {2} {3}", (byte)(this.R * 255), (byte)(this.G * 255), (byte)(this.B * 255), (byte)(this.B * 255));
            }
            else if (format == ColorFormat.BgrU8)
            {
                return string.Format("{0} {1} {2} {3}", (byte)(this.B * 255), (byte)(this.G * 255), (byte)(this.R * 255), (byte)(this.B * 255));
            }
            else if (format == ColorFormat.BgraU8)
            {
                return string.Format("{0} {1} {2} {3}", (byte)(this.B * 255), (byte)(this.G * 255), (byte)(this.R * 255), (byte)(this.B * 255));
            }
            else if (format == ColorFormat.RgbF32)
            {
                return string.Format("{0} {1} {2}", this.R, this.G, this.B);
            }
            else if (format == ColorFormat.RgbaF32)
            {
                return string.Format("{0} {1} {2} {3}", this.R, this.G, this.B, this.A);
            }
            else if (format == ColorFormat.BgrF32)
            {
                return string.Format("{0} {1} {2}", this.B, this.G, this.R);
            }
            else if (format == ColorFormat.BgraF32)
            {
                return string.Format("{0} {1} {2} {3}", this.B, this.G, this.R, this.A);
            }
            else
            {
                throw new ArgumentException("Unsupported format", nameof(format));
            }
        }

        public bool Equals(Color other)
        {
            return this.R == other.R &&
                this.G == other.G &&
                this.B == other.B &&
                this.A == other.A;
        }

        public static bool operator ==(Color a, Color b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(Color a, Color b)
        {
            return !a.Equals(b);
        }

        public static implicit operator System.Numerics.Vector4(Color color) => new System.Numerics.Vector4(color.R, color.G, color.B, color.A);
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
