using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LeagueToolkit.IO.Inibin;

internal static class InibinBinaryCodec
{
    internal static readonly UTF8Encoding Utf8 = new(false, true);

    internal static Dictionary<InibinFlags, InibinSet> Read(BinaryReader reader)
    {
        var sets = new Dictionary<InibinFlags, InibinSet>();
        var offsets = new List<(uint Key, uint Offset)>();
        uint stringLength;
        byte version = reader.ReadByte();
        if (version == 1)
        {
            ReadBytes(reader, 3);
            uint count = reader.ReadUInt32();
            stringLength = reader.ReadUInt32();
            for (uint i = 0; i < count; i++) offsets.Add((reader.ReadUInt32(), reader.ReadUInt32()));
        }
        else if (version == 2)
        {
            stringLength = reader.ReadUInt16();
            ushort flags = reader.ReadUInt16();
            if (flags == 0)
            {
                // Historical RADS files insert a zero u16 before the actual flags.
                int low = reader.BaseStream.ReadByte();
                if (low >= 0) flags = (ushort)(low | (reader.ReadByte() << 8));
            }
            for (int bit = 0; bit < 14; bit++)
            {
                var type = (InibinFlags)(1 << bit);
                if ((flags & (ushort)type) == 0) continue;
                if (type != InibinFlags.StringList)
                {
                    sets.Add(type, new InibinSet(reader, type));
                    continue;
                }
                var keys = new uint[reader.ReadUInt16()];
                for (int i = 0; i < keys.Length; i++) keys[i] = reader.ReadUInt32();
                foreach (uint key in keys) offsets.Add((key, reader.ReadUInt16()));
            }
        }
        else throw new InvalidDataException($"Unsupported INIBIN version: {version}");

        // Read only the declared block, not the rest of the enclosing stream. V1 lengths are untrusted.
        using var data = new MemoryStream();
        var buffer = new byte[8192];
        uint remaining = stringLength;
        while (remaining > 0)
        {
            int read = reader.Read(buffer, 0, (int)Math.Min((uint)buffer.Length, remaining));
            if (read == 0) throw new EndOfStreamException("Truncated INIBIN string data");
            data.Write(buffer, 0, read);
            remaining -= (uint)read;
        }
        if (offsets.Count > 0)
        {
            var strings = new InibinSet(InibinFlags.StringList);
            ReadStrings(strings, offsets, data.ToArray());
            sets.Add(InibinFlags.StringList, strings);
        }
        var seen = new HashSet<uint>();
        foreach (var set in sets.Values)
            foreach (uint key in set.Properties.Keys)
                if (!seen.Add(key)) throw new InvalidDataException($"Duplicate INIBIN key: 0x{key:x8}");
        return sets;
    }

    internal static byte[] ReadBytes(BinaryReader reader, int count)
    {
        byte[] bytes = reader.ReadBytes(count);
        if (bytes.Length != count) throw new EndOfStreamException("Truncated INIBIN value");
        return bytes;
    }

    internal static void ReadStrings(InibinSet set, List<(uint Key, uint Offset)> offsets, byte[] data)
    {
        foreach (var (key, offset) in offsets)
        {
            if (offset >= data.Length) throw new InvalidDataException($"String offset outside INIBIN data for key 0x{key:x8}");
            int end = Array.IndexOf(data, (byte)0, (int)offset);
            if (end < 0) throw new InvalidDataException($"Unterminated INIBIN string for key 0x{key:x8}");
            string value;
            try { value = Utf8.GetString(data, (int)offset, end - (int)offset); }
            catch (DecoderFallbackException ex) { throw new InvalidDataException($"Invalid INIBIN UTF-8 for key 0x{key:x8}", ex); }
            if (!set.Properties.TryAdd(key, value)) throw new InvalidDataException($"Duplicate INIBIN key: 0x{key:x8}");
        }
    }

    internal static object ReadValue(BinaryReader reader, InibinFlags type) => type switch
    {
        InibinFlags.Int32List => reader.ReadInt32(),
        InibinFlags.Int64List => reader.ReadInt64(),
        InibinFlags.Float32List => reader.ReadSingle(),
        InibinFlags.FixedPointFloatList => reader.ReadByte() / 10.0,
        InibinFlags.Int16List => reader.ReadInt16(),
        InibinFlags.Int8List => reader.ReadByte(),
        InibinFlags.FixedPointFloatListVec2 => ReadBytes(reader, 2),
        InibinFlags.FixedPointFloatListVec3 => ReadBytes(reader, 3),
        InibinFlags.FixedPointFloatListVec4 => ReadBytes(reader, 4),
        InibinFlags.Float32ListVec2 => ReadFloats(reader, 2),
        InibinFlags.Float32ListVec3 => ReadFloats(reader, 3),
        InibinFlags.Float32ListVec4 => ReadFloats(reader, 4),
        _ => throw new InvalidDataException($"Invalid fixed-size INIBIN type: {type}")
    };

    private static float[] ReadFloats(BinaryReader reader, int count)
    {
        var result = new float[count];
        for (int i = 0; i < count; i++) result[i] = reader.ReadSingle();
        return result;
    }

    internal static void ValidateType(InibinFlags type)
    {
        int bits = (ushort)type;
        if (bits == 0 || bits > (int)InibinFlags.Int64List || (bits & (bits - 1)) != 0)
            throw new InvalidDataException($"Invalid INIBIN storage type: {type}");
    }

    internal static void ValidateSet(InibinFlags type, InibinSet set)
    {
        ValidateType(type);
        if (set == null || type != set.Type) throw new InvalidDataException("INIBIN set type does not match its dictionary key");
        if (set.Properties.Count > ushort.MaxValue) throw new InvalidDataException("An INIBIN set cannot contain more than 65535 values");
        foreach (object value in set.Properties.Values) ValidateValue(type, value);
    }

    internal static void ValidateValue(InibinFlags type, object value)
    {
        bool valid = type switch
        {
            InibinFlags.Int32List => value is int,
            InibinFlags.Int64List => value is long,
            InibinFlags.Float32List => value is float,
            InibinFlags.FixedPointFloatList => value is double,
            InibinFlags.Int16List => value is short,
            InibinFlags.Int8List => value is byte,
            InibinFlags.BitList => value is bool,
            InibinFlags.StringList => value is string,
            InibinFlags.FixedPointFloatListVec2 => value is byte[] { Length: 2 },
            InibinFlags.FixedPointFloatListVec3 => value is byte[] { Length: 3 },
            InibinFlags.FixedPointFloatListVec4 => value is byte[] { Length: 4 },
            InibinFlags.Float32ListVec2 => value is float[] { Length: 2 },
            InibinFlags.Float32ListVec3 => value is float[] { Length: 3 },
            InibinFlags.Float32ListVec4 => value is float[] { Length: 4 },
            _ => false
        };
        if (!valid) throw new InvalidDataException($"Value does not match INIBIN storage type {type}");
        if (value is double fixedValue)
        {
            int raw = double.IsFinite(fixedValue) && fixedValue >= 0 && fixedValue <= 25.5
                ? (int)Math.Round(fixedValue * 10) : -1;
            if (raw < 0 || fixedValue != raw / 10.0 || double.IsNegative(fixedValue))
                throw new FixedPointFloatOverflowException();
        }
        if (value is string text)
        {
            if (text.Contains('\0')) throw new InvalidDataException("An INIBIN string cannot contain NUL");
            try { Utf8.GetByteCount(text); }
            catch (EncoderFallbackException ex) { throw new InvalidDataException("Invalid INIBIN UTF-16 input", ex); }
        }
    }

    internal static void Write(BinaryWriter writer, Dictionary<InibinFlags, InibinSet> sets)
    {
        var ordered = sets.OrderBy(p => (ushort)p.Key).ToArray();
        var seen = new HashSet<uint>();
        ushort flags = 0;
        foreach (var (type, set) in ordered)
        {
            ValidateSet(type, set);
            if (set.Properties.Count > 0) flags |= (ushort)type;
            foreach (uint key in set.Properties.Keys)
                if (!seen.Add(key)) throw new InvalidDataException($"Duplicate INIBIN key: 0x{key:x8}");
        }
        var strings = sets.TryGetValue(InibinFlags.StringList, out var stringSet)
            ? stringSet.Properties.OrderBy(p => p.Key).Select(p => (string)p.Value).ToArray()
            : Array.Empty<string>();
        var layout = LayoutStrings(strings);
        writer.Write((byte)2);
        writer.Write((ushort)layout.Data.Length);
        writer.Write(flags);
        foreach (var (type, set) in ordered)
            if (set.Properties.Count > 0)
                WriteSet(writer, type, set.Properties.OrderBy(p => p.Key).ToArray(), layout.Offsets);
        // I64 follows string offsets; string bytes always follow every value set.
        writer.Write(layout.Data);
    }

    internal static void WriteSet(BinaryWriter writer, InibinFlags type, KeyValuePair<uint, object>[] entries, ushort[] offsets)
    {
        writer.Write((ushort)entries.Length);
        foreach (var entry in entries) writer.Write(entry.Key);
        if (type == InibinFlags.BitList)
        {
            for (int i = 0; i < entries.Length; i += 8)
            {
                byte bits = 0;
                for (int bit = 0; bit < 8 && i + bit < entries.Length; bit++)
                    if ((bool)entries[i + bit].Value) bits |= (byte)(1 << bit);
                writer.Write(bits);
            }
        }
        else if (type == InibinFlags.StringList)
            foreach (ushort offset in offsets) writer.Write(offset);
        else
            foreach (var entry in entries)
                switch (entry.Value)
                {
                    case int v: writer.Write(v); break;
                    case long v: writer.Write(v); break;
                    case float v: writer.Write(v); break;
                    case double v: writer.Write((byte)Math.Round(v * 10)); break;
                    case short v: writer.Write(v); break;
                    case byte v: writer.Write(v); break;
                    case byte[] v: writer.Write(v); break;
                    case float[] v: foreach (float component in v) writer.Write(component); break;
                    default: throw new InvalidDataException("Unsupported INIBIN value");
                }
    }

    internal static (byte[] Data, ushort[] Offsets) LayoutStrings(string[] strings)
    {
        // Reversed UTF-8 ordering makes suffix sharing linear after the sort.
        var encoded = new Dictionary<string, byte[]>(StringComparer.Ordinal);
        var bytes = new byte[strings.Length][];
        for (int i = 0; i < strings.Length; i++)
        {
            if (!encoded.TryGetValue(strings[i], out bytes[i]))
            {
                if (Utf8.GetByteCount(strings[i]) >= ushort.MaxValue)
                    throw new InvalidDataException("INIBIN string data exceeds 65535 bytes");
                bytes[i] = Utf8.GetBytes(strings[i]);
                encoded.Add(strings[i], bytes[i]);
            }
        }
        var order = Enumerable.Range(0, strings.Length).ToArray();
        Array.Sort(order, (a, b) =>
        {
            int length = Math.Min(bytes[a].Length, bytes[b].Length);
            for (int i = 1; i <= length; i++)
            {
                int diff = bytes[a][^i].CompareTo(bytes[b][^i]);
                if (diff != 0) return diff;
            }
            int result = bytes[a].Length.CompareTo(bytes[b].Length);
            return result != 0 ? result : b.CompareTo(a);
        });
        var holders = Enumerable.Range(0, strings.Length).ToArray();
        for (int i = order.Length - 2; i >= 0; i--)
            if (bytes[order[i + 1]].AsSpan().EndsWith(bytes[order[i]])) holders[order[i]] = holders[order[i + 1]];
        using var data = new MemoryStream();
        var positions = new int[strings.Length];
        for (int i = 0; i < strings.Length; i++)
        {
            if (holders[i] != i) continue;
            if (data.Length + bytes[i].Length + 1 > ushort.MaxValue)
                throw new InvalidDataException("INIBIN string data exceeds 65535 bytes");
            positions[i] = (int)data.Length;
            data.Write(bytes[i], 0, bytes[i].Length);
            data.WriteByte(0);
        }
        var offsets = new ushort[strings.Length];
        for (int i = 0; i < strings.Length; i++)
            offsets[i] = checked((ushort)(positions[holders[i]] + bytes[holders[i]].Length - bytes[i].Length));
        return (data.ToArray(), offsets);
    }
}
