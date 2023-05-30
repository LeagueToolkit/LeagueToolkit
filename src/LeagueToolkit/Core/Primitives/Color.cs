using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

namespace LeagueToolkit.Core.Primitives;

[DebuggerDisplay("{R} {G} {B} {A}")]
public struct Color : IEquatable<Color>
{
    public static readonly Color Zero = new(0f, 0f, 0f, 0f);
    public static readonly Color One = new(1f, 1f, 1f, 1f);

    public float R
    {
        get => this._r;
        set
        {
            if (value < 0 || value > 1)
                throw new ArgumentOutOfRangeException(nameof(value), "value must be in 0-1 range");
            else
                this._r = value;
        }
    }
    public float G
    {
        get => this._g;
        set
        {
            if (value < 0 || value > 1)
                throw new ArgumentOutOfRangeException(nameof(value), "value must be in 0-1 range");
            else
                this._g = value;
        }
    }
    public float B
    {
        get => this._b;
        set
        {
            if (value < 0 || value > 1)
                throw new ArgumentOutOfRangeException(nameof(value), "value must be in 0-1 range");
            else
                this._b = value;
        }
    }
    public float A
    {
        get => this._a;
        set
        {
            if (value < 0 || value > 1)
                throw new ArgumentOutOfRangeException(nameof(value), "value must be in 0-1 range");
            else
                this._a = value;
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
        if (r < 0 || r > 1)
            Math.Clamp(r, 0, 1);
        if (g < 0 || g > 1)
            Math.Clamp(g, 0, 1);
        if (b < 0 || b > 1)
            Math.Clamp(b, 0, 1);
        if (a < 0 || a > 1)
            Math.Clamp(a, 0, 1);

        this._r = r;
        this._g = g;
        this._b = b;
        this._a = a;
    }

    public static int GetFormatSize(ColorFormat format)
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

    public static Color Read(ReadOnlySpan<byte> source, ColorFormat format)
    {
        if (format == ColorFormat.RgbU8)
        {
            float r = source[0] / 255f;
            float g = source[1] / 255f;
            float b = source[2] / 255f;
            return new Color(r, g, b);
        }
        else if (format == ColorFormat.RgbaU8)
        {
            float r = source[0] / 255f;
            float g = source[1] / 255f;
            float b = source[2] / 255f;
            float a = source[3] / 255f;
            return new Color(r, g, b, a);
        }
        else if (format == ColorFormat.RgbF32)
        {
            ReadOnlySpan<float> colorFloats = MemoryMarshal.Cast<byte, float>(source);
            float r = colorFloats[0];
            float g = colorFloats[1];
            float b = colorFloats[2];
            return new Color(r, g, b);
        }
        else if (format == ColorFormat.RgbaF32)
        {
            ReadOnlySpan<float> colorFloats = MemoryMarshal.Cast<byte, float>(source);
            float r = colorFloats[0];
            float g = colorFloats[1];
            float b = colorFloats[2];
            float a = colorFloats[3];
            return new Color(r, g, b, a);
        }
        else if (format == ColorFormat.BgrU8)
        {
            float b = source[0] / 255f;
            float g = source[1] / 255f;
            float r = source[2] / 255f;
            return new Color(r, g, b);
        }
        else if (format == ColorFormat.BgraU8)
        {
            float b = source[0] / 255f;
            float g = source[1] / 255f;
            float r = source[2] / 255f;
            float a = source[3] / 255f;
            return new Color(r, g, b, a);
        }
        else if (format == ColorFormat.BgrF32)
        {
            ReadOnlySpan<float> colorFloats = MemoryMarshal.Cast<byte, float>(source);
            float b = colorFloats[0];
            float g = colorFloats[1];
            float r = colorFloats[2];
            return new Color(r, g, b);
        }
        else if (format == ColorFormat.BgraF32)
        {
            ReadOnlySpan<float> colorFloats = MemoryMarshal.Cast<byte, float>(source);
            float b = colorFloats[0];
            float g = colorFloats[1];
            float r = colorFloats[2];
            float a = colorFloats[3];
            return new Color(r, g, b, a);
        }
        else
        {
            throw new ArgumentException($"Unsupported color format: {format}", nameof(format));
        }
    }

    public void Write(Span<byte> destination, ColorFormat format)
    {
        if (format == ColorFormat.RgbU8)
        {
            destination[0] = (byte)(this._r * 255);
            destination[1] = (byte)(this._g * 255);
            destination[2] = (byte)(this._b * 255);
        }
        else if (format == ColorFormat.RgbaU8)
        {
            destination[0] = (byte)(this._r * 255);
            destination[1] = (byte)(this._g * 255);
            destination[2] = (byte)(this._b * 255);
            destination[3] = (byte)(this._a * 255);
        }
        else if (format == ColorFormat.BgrU8)
        {
            destination[0] = (byte)(this._b * 255);
            destination[1] = (byte)(this._g * 255);
            destination[2] = (byte)(this._r * 255);
        }
        else if (format == ColorFormat.BgraU8)
        {
            destination[0] = (byte)(this._b * 255);
            destination[1] = (byte)(this._g * 255);
            destination[2] = (byte)(this._r * 255);
            destination[3] = (byte)(this._a * 255);
        }
        else if (format == ColorFormat.RgbF32)
        {
            MemoryMarshal.Write(destination.Slice(sizeof(float) * 0, sizeof(float)), ref this._r);
            MemoryMarshal.Write(destination.Slice(sizeof(float) * 1, sizeof(float)), ref this._g);
            MemoryMarshal.Write(destination.Slice(sizeof(float) * 2, sizeof(float)), ref this._b);
        }
        else if (format == ColorFormat.RgbaF32)
        {
            MemoryMarshal.Write(destination.Slice(sizeof(float) * 0, sizeof(float)), ref this._r);
            MemoryMarshal.Write(destination.Slice(sizeof(float) * 1, sizeof(float)), ref this._g);
            MemoryMarshal.Write(destination.Slice(sizeof(float) * 2, sizeof(float)), ref this._b);
            MemoryMarshal.Write(destination.Slice(sizeof(float) * 3, sizeof(float)), ref this._a);
        }
        else if (format == ColorFormat.BgrF32)
        {
            MemoryMarshal.Write(destination.Slice(sizeof(float) * 0, sizeof(float)), ref this._b);
            MemoryMarshal.Write(destination.Slice(sizeof(float) * 1, sizeof(float)), ref this._g);
            MemoryMarshal.Write(destination.Slice(sizeof(float) * 2, sizeof(float)), ref this._r);
        }
        else if (format == ColorFormat.BgraF32)
        {
            MemoryMarshal.Write(destination.Slice(sizeof(float) * 0, sizeof(float)), ref this._b);
            MemoryMarshal.Write(destination.Slice(sizeof(float) * 1, sizeof(float)), ref this._g);
            MemoryMarshal.Write(destination.Slice(sizeof(float) * 2, sizeof(float)), ref this._r);
            MemoryMarshal.Write(destination.Slice(sizeof(float) * 3, sizeof(float)), ref this._a);
        }
    }

    public string ToString(ColorFormat format)
    {
        if (format == ColorFormat.RgbU8)
        {
            return string.Format("{0} {1} {2}", (byte)(this.R * 255), (byte)(this.G * 255), (byte)(this.B * 255));
        }
        else if (format == ColorFormat.RgbaU8)
        {
            return string.Format(
                "{0} {1} {2} {3}",
                (byte)(this.R * 255),
                (byte)(this.G * 255),
                (byte)(this.B * 255),
                (byte)(this.B * 255)
            );
        }
        else if (format == ColorFormat.BgrU8)
        {
            return string.Format(
                "{0} {1} {2} {3}",
                (byte)(this.B * 255),
                (byte)(this.G * 255),
                (byte)(this.R * 255),
                (byte)(this.B * 255)
            );
        }
        else if (format == ColorFormat.BgraU8)
        {
            return string.Format(
                "{0} {1} {2} {3}",
                (byte)(this.B * 255),
                (byte)(this.G * 255),
                (byte)(this.R * 255),
                (byte)(this.B * 255)
            );
        }
        else if (format == ColorFormat.RgbF32)
        {
            return string.Format(NumberFormatInfo.InvariantInfo, "{0} {1} {2}", this.R, this.G, this.B);
        }
        else if (format == ColorFormat.RgbaF32)
        {
            return string.Format(NumberFormatInfo.InvariantInfo, "{0} {1} {2} {3}", this.R, this.G, this.B, this.A);
        }
        else if (format == ColorFormat.BgrF32)
        {
            return string.Format(NumberFormatInfo.InvariantInfo, "{0} {1} {2}", this.B, this.G, this.R);
        }
        else if (format == ColorFormat.BgraF32)
        {
            return string.Format(NumberFormatInfo.InvariantInfo, "{0} {1} {2} {3}", this.B, this.G, this.R, this.A);
        }
        else
        {
            throw new ArgumentException("Unsupported format", nameof(format));
        }
    }

    public bool Equals(Color other)
    {
        return this.R == other.R && this.G == other.G && this.B == other.B && this.A == other.A;
    }

    public override bool Equals(object obj)
    {
        return obj is Color other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(R, G, B, A);
    }

    public static bool operator ==(Color a, Color b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Color a, Color b)
    {
        return !a.Equals(b);
    }

    public static implicit operator System.Numerics.Vector4(Color color) => new(color.R, color.G, color.B, color.A);

    public static implicit operator Color(System.Numerics.Vector4 vector) =>
        new(vector.X, vector.Y, vector.Z, vector.W);
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
