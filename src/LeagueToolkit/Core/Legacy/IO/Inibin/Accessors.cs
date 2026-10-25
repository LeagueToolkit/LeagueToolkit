using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace LeagueToolkit.IO.Inibin;

public partial class InibinFile
{
    private static readonly Regex FloatPrefix = new(@"^[+-]?(?:infinity|inf|nan|(?:[0-9]+(?:\.[0-9]*)?|\.[0-9]+)(?:[eE][+-]?[0-9]+)?)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

    public bool TryGetValue(uint hash, out object value)
    {
        foreach (var set in Sets.Values)
            if (set.Properties.TryGetValue(hash, out value)) return true;
        value = null;
        return false;
    }

    public bool TryGetValue(string section, string property, out object value) => TryGetValue(HashKey(section, property), out value);

    public string GetString(uint hash) => TryGetValue(hash, out var value) ? value as string : null;
    public string GetString(string section, string property) => GetString(HashKey(section, property));

    /// <summary>Converts numbers, U1 values and leading decimal strings as the client does.</summary>
    public long? GetInt64(uint hash)
    {
        if (!TryGetValue(hash, out var value)) return null;
        return value switch
        {
            long v => v,
            int v => v,
            short v => v,
            byte v => v,
            bool v => v ? 1 : 0,
            float v => SaturatingInt64(v),
            double v => SaturatingInt64(v),
            string v => ParseInteger(v),
            _ => null
        };
    }

    public long? GetInt64(string section, string property) => GetInt64(HashKey(section, property));
    public int? GetInt32(uint hash)
    {
        if (TryGetValue(hash, out var raw) && raw is float number)
            return float.IsNaN(number) ? 0 : number >= int.MaxValue ? int.MaxValue : number <= int.MinValue ? int.MinValue : (int)number;
        long? value = GetInt64(hash);
        return value.HasValue ? unchecked((int)value.Value) : null;
    }
    public int? GetInt32(string section, string property) => GetInt32(HashKey(section, property));

    public float? GetFloat(uint hash)
    {
        if (!TryGetValue(hash, out var value)) return null;
        return value switch
        {
            long v => v,
            int v => v,
            short v => v,
            byte v => v,
            bool v => v ? 1 : 0,
            float v => v,
            double v => (float)v,
            string v => ParseFloat(v, out _),
            _ => null
        };
    }
    public float? GetFloat(string section, string property) => GetFloat(HashKey(section, property));

    /// <summary>The client accepts U1 values and strings beginning with '1', not arbitrary numbers.</summary>
    public bool? GetBoolean(uint hash)
    {
        if (!TryGetValue(hash, out var value)) return null;
        return value switch { bool v => v, string v => v.StartsWith('1'), _ => null };
    }
    public bool? GetBoolean(string section, string property) => GetBoolean(HashKey(section, property));

    /// <summary>Converts float vectors, fixed-point byte vectors, or the leading components of strings.</summary>
    public float[] GetVector(uint hash, int dimensions)
    {
        if (dimensions is < 2 or > 4) throw new VectorOverflowException();
        if (!TryGetValue(hash, out var value)) return null;
        if (value is float[] floats) return floats.Length == dimensions ? (float[])floats.Clone() : null;
        if (value is byte[] bytes) return bytes.Length == dimensions ? bytes.Select(b => b / 10f).ToArray() : null;
        if (value is not string text) return null;
        var result = new float[dimensions];
        for (int i = 0; i < result.Length; i++)
        {
            text = TrimWhitespace(text);
            result[i] = ParseFloat(text, out int consumed);
            if (consumed == 0) return null;
            text = text[consumed..];
        }
        return result;
    }
    public float[] GetVector(string section, string property, int dimensions) => GetVector(HashKey(section, property), dimensions);

    public void AddCompactValue(uint hash, long value)
    {
        object compact = value switch
        {
            0 or 1 => value == 1,
            >= byte.MinValue and <= byte.MaxValue => (byte)value,
            >= short.MinValue and <= short.MaxValue => (short)value,
            >= int.MinValue and <= int.MaxValue => (int)value,
            _ => value
        };
        AddValue(hash, compact);
    }

    public void AddCompactValue(uint hash, float value)
    {
        if (TryFixed(value, out byte raw)) AddValue(hash, raw / 10.0);
        else AddValue(hash, value);
    }

    public void AddCompactValue(uint hash, float[] value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value.Length is < 2 or > 4) throw new VectorOverflowException();
        var bytes = new byte[value.Length];
        for (int i = 0; i < value.Length; i++)
            if (!TryFixed(value[i], out bytes[i])) { AddValue(hash, value); return; }
        AddValue(hash, bytes);
    }

    private static bool TryFixed(float value, out byte raw)
    {
        raw = float.IsFinite(value) && value >= 0 && value <= 25.5f ? (byte)MathF.Round(value * 10) : (byte)0;
        return !float.IsNegative(value) && value == raw / 10f;
    }

    private static long SaturatingInt64(double value) => double.IsNaN(value) ? 0
        : value >= long.MaxValue ? long.MaxValue : value <= long.MinValue ? long.MinValue : (long)value;

    private static string TrimWhitespace(string text) => text.TrimStart(' ', '\t', '\n', '\v', '\f', '\r');

    private static long ParseInteger(string text)
    {
        text = TrimWhitespace(text);
        int end = text.Length > 0 && text[0] is '+' or '-' ? 1 : 0;
        int start = end;
        while (end < text.Length && text[end] is >= '0' and <= '9') end++;
        if (end == start) return 0;
        var value = BigInteger.Parse(text[..end], NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture);
        return value < long.MinValue ? long.MinValue : value > long.MaxValue ? long.MaxValue : (long)value;
    }

    private static float ParseFloat(string text, out int consumed)
    {
        text = TrimWhitespace(text);
        var match = FloatPrefix.Match(text);
        consumed = match.Length;
        if (!match.Success) return 0;
        string number = match.Value;
        string unsigned = number.TrimStart('+', '-');
        if (unsigned.StartsWith("inf", StringComparison.OrdinalIgnoreCase))
            return number[0] == '-' ? float.NegativeInfinity : float.PositiveInfinity;
        if (unsigned.Equals("nan", StringComparison.OrdinalIgnoreCase)) return float.NaN;
        return (float)double.Parse(number, NumberStyles.Float, CultureInfo.InvariantCulture);
    }
}
