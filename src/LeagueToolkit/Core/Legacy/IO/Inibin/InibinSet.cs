using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LeagueToolkit.IO.Inibin;

/// <summary>Values with a single INIBIN storage type.</summary>
public class InibinSet
{
    public InibinFlags Type { get; private set; }
    public Dictionary<uint, object> Properties { get; private set; } = new();

    public InibinSet() { }
    public InibinSet(InibinFlags type) => Type = type;
    public InibinSet(InibinFlags type, Dictionary<uint, object> properties)
    {
        ArgumentNullException.ThrowIfNull(properties);
        Type = type;
        Properties = properties;
    }

    public InibinSet(BinaryReader reader, InibinFlags type)
    {
        InibinBinaryCodec.ValidateType(type);
        if (type == InibinFlags.StringList)
            throw new ArgumentException("String sets require a string-data offset", nameof(type));
        Type = type;
        var keys = new uint[reader.ReadUInt16()];
        for (int i = 0; i < keys.Length; i++) keys[i] = reader.ReadUInt32();
        byte bits = 0;
        for (int i = 0; i < keys.Length; i++)
        {
            object value;
            if (type == InibinFlags.BitList)
            {
                if (i % 8 == 0) bits = reader.ReadByte();
                value = (bits & (1 << (i % 8))) != 0;
            }
            else value = InibinBinaryCodec.ReadValue(reader, type);
            if (!Properties.TryAdd(keys[i], value))
                throw new InvalidDataException($"Duplicate INIBIN key: 0x{keys[i]:x8}");
        }
    }

    /// <summary>Reads legacy v1 interleaved entries, or v2 string keys and offsets.</summary>
    public InibinSet(BinaryReader reader, InibinFlags type, uint stringOffset, uint? valueCount = null)
    {
        if (type != InibinFlags.StringList) throw new ArgumentException("Expected a string set", nameof(type));
        Type = type;
        var offsets = new List<(uint Key, uint Offset)>();
        if (valueCount.HasValue)
        {
            for (uint i = 0; i < valueCount.Value; i++) offsets.Add((reader.ReadUInt32(), reader.ReadUInt32()));
        }
        else
        {
            var keys = new uint[reader.ReadUInt16()];
            foreach (int i in Enumerable.Range(0, keys.Length)) keys[i] = reader.ReadUInt32();
            foreach (uint key in keys) offsets.Add((key, reader.ReadUInt16()));
        }
        long position = reader.BaseStream.Position;
        try
        {
            reader.BaseStream.Position = stringOffset;
            long length = reader.BaseStream.Length - stringOffset;
            if (length < 0 || length > int.MaxValue) throw new InvalidDataException("Invalid string-data offset");
            InibinBinaryCodec.ReadStrings(this, offsets, InibinBinaryCodec.ReadBytes(reader, (int)length));
        }
        finally { reader.BaseStream.Position = position; }
    }

    /// <summary>Writes one sorted set. For strings, writes the offsets followed by the string data.</summary>
    public void Write(BinaryWriter writer)
    {
        InibinBinaryCodec.ValidateSet(Type, this);
        var entries = Properties.OrderBy(p => p.Key).ToArray();
        var layout = Type == InibinFlags.StringList
            ? InibinBinaryCodec.LayoutStrings(entries.Select(p => (string)p.Value).ToArray())
            : (Data: Array.Empty<byte>(), Offsets: Array.Empty<ushort>());
        InibinBinaryCodec.WriteSet(writer, Type, entries, layout.Offsets);
        writer.Write(layout.Data);
    }
}
