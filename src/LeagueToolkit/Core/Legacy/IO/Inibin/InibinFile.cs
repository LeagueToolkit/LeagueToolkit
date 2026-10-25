using System;
using System.Collections.Generic;
using System.IO;

namespace LeagueToolkit.IO.Inibin;

/// <summary>Reads binary INI configurations (.inibin, .troybin and .cfgbin).</summary>
public partial class InibinFile
{
    /// <summary>Values grouped by their storage type. Keys must be unique across all sets.</summary>
    public Dictionary<InibinFlags, InibinSet> Sets { get; private set; } = new();

    public InibinFile() { }

    public InibinFile(Dictionary<InibinFlags, InibinSet> sets)
    {
        ArgumentNullException.ThrowIfNull(sets);
        Sets = sets;
    }

    public InibinFile(string fileLocation) : this(File.OpenRead(fileLocation)) { }

    public InibinFile(Stream stream) : this(stream, false) { }

    /// <summary>Reads one file, including from non-seekable streams. Optionally leaves the stream open.</summary>
    public InibinFile(Stream stream, bool leaveOpen)
    {
        using var reader = new BinaryReader(stream, InibinBinaryCodec.Utf8, leaveOpen);
        Sets = InibinBinaryCodec.Read(reader);
    }

    public void Write(string fileLocation)
    {
        using var stream = File.Create(fileLocation);
        Write(stream);
    }

    /// <summary>Writes version 2 without header padding, sorting sets and keys.</summary>
    public void Write(Stream stream, bool leaveOpen = false)
    {
        using var writer = new BinaryWriter(stream, InibinBinaryCodec.Utf8, leaveOpen);
        InibinBinaryCodec.Write(writer, Sets);
    }

    /// <summary>Hashes UTF-8 names using ASCII folding and signed bytes, as the game does.</summary>
    public static uint HashKey(string section, string property)
    {
        ArgumentNullException.ThrowIfNull(section);
        ArgumentNullException.ThrowIfNull(property);
        uint hash = 0;
        foreach (byte value in InibinBinaryCodec.Utf8.GetBytes(section + "*" + property))
        {
            byte folded = value is >= (byte)'A' and <= (byte)'Z' ? (byte)(value + 32) : value;
            hash = unchecked(hash * 65599 + (uint)(sbyte)folded);
        }
        return hash;
    }

    public void AddValue(string section, string property, object value) => AddValue(HashKey(section, property), value);

    /// <summary>Adds a value without changing its storage type. Double values represent fixed-point tenths.</summary>
    public void AddValue(uint hash, object value)
    {
        ArgumentNullException.ThrowIfNull(value);
        InibinFlags type = value switch
        {
            int => InibinFlags.Int32List,
            long => InibinFlags.Int64List,
            float => InibinFlags.Float32List,
            double => InibinFlags.FixedPointFloatList,
            short => InibinFlags.Int16List,
            byte => InibinFlags.Int8List,
            bool => InibinFlags.BitList,
            string => InibinFlags.StringList,
            byte[] v => v.Length switch
            {
                2 => InibinFlags.FixedPointFloatListVec2,
                3 => InibinFlags.FixedPointFloatListVec3,
                4 => InibinFlags.FixedPointFloatListVec4,
                _ => throw new VectorOverflowException()
            },
            float[] v => v.Length switch
            {
                2 => InibinFlags.Float32ListVec2,
                3 => InibinFlags.Float32ListVec3,
                4 => InibinFlags.Float32ListVec4,
                _ => throw new VectorOverflowException()
            },
            _ => throw new UnsupportedValueTypeException($"The type {value.GetType()} is not supported")
        };
        InibinBinaryCodec.ValidateValue(type, value);
        if (TryGetValue(hash, out _))
            throw new ArgumentException($"Duplicate INIBIN key: 0x{hash:x8}", nameof(hash));
        if (!Sets.TryGetValue(type, out var set))
            Sets.Add(type, set = new InibinSet(type));
        set.Properties.Add(hash, value);
    }
}

public class UnsupportedValueTypeException : Exception
{
    public UnsupportedValueTypeException(string message) : base(message) { }
}

public class FixedPointFloatOverflowException : Exception
{
    public FixedPointFloatOverflowException() : base("A fixed-point value must be an exact tenth between 0.0 and 25.5") { }
}

public class VectorOverflowException : Exception
{
    public VectorOverflowException() : base("A vector must have 2, 3, or 4 components") { }
}

[Flags]
public enum InibinFlags : ushort
{
    Int32List = 1,
    Float32List = 1 << 1,
    FixedPointFloatList = 1 << 2,
    Int16List = 1 << 3,
    /// <summary>Unsigned 8-bit integers. The name is retained for compatibility.</summary>
    Int8List = 1 << 4,
    /// <summary>Unsigned 1-bit integers (0 or 1), represented by bool for compatibility.</summary>
    BitList = 1 << 5,
    FixedPointFloatListVec3 = 1 << 6,
    Float32ListVec3 = 1 << 7,
    FixedPointFloatListVec2 = 1 << 8,
    Float32ListVec2 = 1 << 9,
    FixedPointFloatListVec4 = 1 << 10,
    Float32ListVec4 = 1 << 11,
    StringList = 1 << 12,
    Int64List = 1 << 13
}
