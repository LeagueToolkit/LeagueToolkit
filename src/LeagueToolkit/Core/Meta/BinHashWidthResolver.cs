using System.Buffers.Binary;

namespace LeagueToolkit.Core.Meta;

/// <summary>
/// Finds which <see cref="BinPropertyType.Hash"/> values of an object are stored in 8 bytes.
/// </summary>
/// <remarks>
/// <para>
/// Since 16.21 the client stores some <c>Hash</c> values (<c>StaticMaterialDef.name</c> among them) in
/// 8 bytes. The kind byte is 17 at both widths and the file does not say which: the client knows the
/// width from the property's class. A reader that takes every hash as 4 bytes stops short and reads a
/// hash byte as the next kind byte.
/// </para>
/// <para>
/// Every value is inside a region that declares its byte size (object, struct, container, map), so
/// the widths can be recovered from the file: within a property list, each <c>Hash</c> (or optional
/// hash) is tried as 4 bytes, then 8, with backtracking, until the list ends exactly at its declared
/// size; a container computes its item width from its size and count; a map tries its key and value
/// widths 4/4 first. This is the rule the Rust league-toolkit adopted (ADR-0022, "hash width from
/// declared sizes"), and it is only run for an object that does not read with 4-byte hashes.
/// </para>
/// </remarks>
internal sealed class BinHashWidthResolver
{
    private readonly byte[] _data;
    private readonly List<int> _wide = new();
    private long _steps;

    private BinHashWidthResolver(byte[] data)
    {
        this._data = data;
        this._steps = 64L * data.Length + 4096; // a hostile file cannot make the search run away
    }

    /// <summary>
    /// Resolves the widths of an object's body: the bytes its size covers (path hash, property count,
    /// properties). Returns the absolute offsets of the 8-byte hashes, or <see langword="null"/> when no
    /// assignment of widths fits the declared sizes.
    /// </summary>
    public static HashSet<long> Resolve(byte[] objectBody, long bodyOffset)
    {
        if (objectBody.Length < 6)
            return null;

        BinHashWidthResolver resolver = new(objectBody);
        ushort propertyCount = BinaryPrimitives.ReadUInt16LittleEndian(objectBody.AsSpan(4));

        try
        {
            if (!resolver.Properties(propertyCount, 6, objectBody.Length))
                return null;
        }
        catch (StepLimitException)
        {
            return null;
        }

        return resolver._wide.Select(x => bodyOffset + x).ToHashSet();
    }

    private sealed class StepLimitException : Exception { }

    /// <summary>The remaining <paramref name="count"/> properties from <paramref name="pos"/>, ending exactly at <paramref name="end"/>.</summary>
    private bool Properties(int count, int pos, int end)
    {
        if (--this._steps < 0)
            throw new StepLimitException();
        if (count == 0)
            return pos == end;
        if (pos + 5 > end)
            return false;

        BinPropertyType type = (BinPropertyType)this._data[pos + 4];
        bool hasChoice =
            type is BinPropertyType.Hash
            || (
                type is BinPropertyType.Optional
                && pos + 7 <= end
                && (BinPropertyType)this._data[pos + 5] is BinPropertyType.Hash
                && this._data[pos + 6] is not 0
            );

        for (int width = 4; width <= (hasChoice ? 8 : 4); width += 4)
        {
            int mark = this._wide.Count;
            int p = pos + 5;

            if (Value(type, ref p, end, width) && Properties(count - 1, p, end))
                return true;

            this._wide.RemoveRange(mark, this._wide.Count - mark);
        }

        return false;
    }

    /// <summary>One value of <paramref name="type"/>; a hash (or optional hash) takes <paramref name="hashWidth"/> bytes.</summary>
    private bool Value(BinPropertyType type, ref int pos, int end, int hashWidth)
    {
        switch (type)
        {
            case BinPropertyType.None:
                return true;
            case BinPropertyType.Bool or BinPropertyType.I8 or BinPropertyType.U8 or BinPropertyType.BitBool:
                return Skip(ref pos, 1, end);
            case BinPropertyType.I16 or BinPropertyType.U16:
                return Skip(ref pos, 2, end);
            case BinPropertyType.I32
            or BinPropertyType.U32
            or BinPropertyType.F32
            or BinPropertyType.Color
            or BinPropertyType.ObjectLink:
                return Skip(ref pos, 4, end);
            case BinPropertyType.I64 or BinPropertyType.U64 or BinPropertyType.Vector2 or BinPropertyType.WadChunkLink:
                return Skip(ref pos, 8, end);
            case BinPropertyType.Vector3:
                return Skip(ref pos, 12, end);
            case BinPropertyType.Vector4:
                return Skip(ref pos, 16, end);
            case BinPropertyType.Matrix44:
                return Skip(ref pos, 64, end);
            case BinPropertyType.String:
                return pos + 2 <= end && Skip(ref pos, 2 + BinaryPrimitives.ReadUInt16LittleEndian(this._data.AsSpan(pos)), end);
            case BinPropertyType.Hash:
                if (hashWidth is 8)
                    this._wide.Add(pos);
                return Skip(ref pos, hashWidth, end);
            case BinPropertyType.Optional:
            {
                if (pos + 2 > end)
                    return false;
                BinPropertyType itemType = (BinPropertyType)this._data[pos];
                bool isSome = this._data[pos + 1] is not 0;
                pos += 2;
                return !isSome || Value(itemType, ref pos, end, hashWidth);
            }
            case BinPropertyType.Container or BinPropertyType.UnorderedContainer:
                return Container(ref pos, end);
            case BinPropertyType.Struct or BinPropertyType.Embedded:
                return Struct(ref pos, end);
            case BinPropertyType.Map:
                return Map(ref pos, end);
            default:
                return false;
        }
    }

    private bool Container(ref int pos, int end)
    {
        if (pos + 9 > end)
            return false;

        BinPropertyType itemType = (BinPropertyType)this._data[pos];
        int size = BinaryPrimitives.ReadInt32LittleEndian(this._data.AsSpan(pos + 1));
        int count = BinaryPrimitives.ReadInt32LittleEndian(this._data.AsSpan(pos + 5));
        int regionEnd = pos + 5 + size;
        if (size < 4 || count < 0 || regionEnd > end)
            return false;

        // All items of a container share one width, which its size gives away
        int width = 4;
        if (itemType is BinPropertyType.Hash && count > 0)
        {
            width = (size - 4) / count;
            if (width is not (4 or 8) || width * count != size - 4)
                return false;
        }

        int p = pos + 9;
        for (int i = 0; i < count; i++)
            if (!Value(itemType, ref p, regionEnd, width))
                return false;

        if (p != regionEnd)
            return false;

        pos = regionEnd;
        return true;
    }

    private bool Struct(ref int pos, int end)
    {
        if (pos + 4 > end)
            return false;

        uint classHash = BinaryPrimitives.ReadUInt32LittleEndian(this._data.AsSpan(pos));
        pos += 4;
        if (classHash is 0)
            return true;

        if (pos + 6 > end)
            return false;

        int size = BinaryPrimitives.ReadInt32LittleEndian(this._data.AsSpan(pos));
        int regionEnd = pos + 4 + size;
        if (size < 2 || regionEnd > end)
            return false;

        ushort count = BinaryPrimitives.ReadUInt16LittleEndian(this._data.AsSpan(pos + 4));
        if (!Properties(count, pos + 6, regionEnd))
            return false;

        pos = regionEnd;
        return true;
    }

    private bool Map(ref int pos, int end)
    {
        if (pos + 10 > end)
            return false;

        BinPropertyType keyType = (BinPropertyType)this._data[pos];
        BinPropertyType valueType = (BinPropertyType)this._data[pos + 1];
        int size = BinaryPrimitives.ReadInt32LittleEndian(this._data.AsSpan(pos + 2));
        int count = BinaryPrimitives.ReadInt32LittleEndian(this._data.AsSpan(pos + 6));
        int regionEnd = pos + 6 + size;
        if (size < 4 || count < 0 || regionEnd > end)
            return false;

        // All keys share one width and all values share one; 4/4 first, as a 4-byte reader reads it
        foreach (var (keyWidth, valueWidth) in new[] { (4, 4), (8, 8), (4, 8), (8, 4) })
        {
            if ((keyType is not BinPropertyType.Hash && keyWidth is not 4) || (valueType is not BinPropertyType.Hash && valueWidth is not 4))
                continue;

            int mark = this._wide.Count;
            int p = pos + 10;
            bool fits = true;
            for (int i = 0; fits && i < count; i++)
                fits = Value(keyType, ref p, regionEnd, keyWidth) && Value(valueType, ref p, regionEnd, valueWidth);

            if (fits && p == regionEnd)
            {
                pos = regionEnd;
                return true;
            }

            this._wide.RemoveRange(mark, this._wide.Count - mark);
        }

        return false;
    }

    private static bool Skip(ref int pos, int length, int end)
    {
        if (pos + length > end)
            return false;

        pos += length;
        return true;
    }
}
