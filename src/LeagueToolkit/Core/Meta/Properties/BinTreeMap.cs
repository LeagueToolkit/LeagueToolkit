using CommunityToolkit.Diagnostics;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace LeagueToolkit.Core.Meta.Properties;

public sealed class BinTreeMap : BinTreeProperty, IDictionary<BinTreeProperty, BinTreeProperty>
{
    public override BinPropertyType Type => BinPropertyType.Map;
    public BinPropertyType KeyType { get; private set; }
    public BinPropertyType ValueType { get; private set; }

    public ICollection<BinTreeProperty> Keys => this._map.Keys;

    public ICollection<BinTreeProperty> Values => this._map.Values;

    public int Count => this._map.Count;

    public bool IsReadOnly => false;

    public BinTreeProperty this[BinTreeProperty key]
    {
        get => this._map[key];
        set
        {
            if (key.Type != this.KeyType)
                ThrowHelper.ThrowArgumentException(nameof(value), "Key type must match the map's key type");
            if (value.Type != this.ValueType)
                ThrowHelper.ThrowArgumentException(nameof(value), "Value type must match the map's value type");

            this._map[key] = value;
        }
    }

    private readonly Dictionary<BinTreeProperty, BinTreeProperty> _map = new();

    public BinTreeMap(
        uint nameHash,
        BinPropertyType keyType,
        BinPropertyType valueType,
        IEnumerable<KeyValuePair<BinTreeProperty, BinTreeProperty>> map
    ) : base(nameHash)
    {
        Guard.IsNotNull(map, nameof(map));

        this.KeyType = keyType;
        this.ValueType = valueType;
        this._map = new(map);

        // Verify property types
        foreach (var pair in map)
        {
            if (pair.Key.Type != keyType)
                throw new ArgumentException("Found a key that does not match the specified key type", nameof(map));
            if (pair.Value.Type != valueType)
                throw new ArgumentException("Found a value that does not match the specified value type", nameof(map));
        }
    }

    internal BinTreeMap(BinaryReader br, uint nameHash, bool useLegacyType = false) : base(nameHash)
    {
        this.KeyType = BinUtilities.UnpackType((BinPropertyType)br.ReadByte(), useLegacyType);
        this.ValueType = BinUtilities.UnpackType((BinPropertyType)br.ReadByte(), useLegacyType);
        uint size = br.ReadUInt32();
        long contentOffset = br.BaseStream.Position;

        uint valueCount = br.ReadUInt32();
        for (int i = 0; i < valueCount; i++)
            this._map.Add(
                ReadPropertyContent(0, this.KeyType, br, useLegacyType),
                ReadPropertyContent(0, this.ValueType, br, useLegacyType)
            );

        if (br.BaseStream.Position != contentOffset + size)
            ThrowHelper.ThrowInvalidDataException(
                $"Invalid size: {br.BaseStream.Position - contentOffset}, expected {size}"
            );
    }

    protected override void WriteContent(BinaryWriter bw)
    {
        bw.Write((byte)this.KeyType);
        bw.Write((byte)this.ValueType);
        bw.Write(GetContentSize());
        bw.Write(this._map.Count);

        foreach (var (key, value) in this._map)
        {
            key.Write(bw, writeHeader: false);
            value.Write(bw, writeHeader: false);
        }
    }

    internal override int GetSize(bool includeHeader) =>
        1 + 1 + 4 + GetContentSize() + (includeHeader ? HEADER_SIZE : 0);

    private int GetContentSize() =>
        4 + this.Sum(x => x.Key.GetSize(includeHeader: false) + x.Value.GetSize(includeHeader: false));

    public override bool Equals(BinTreeProperty other)
    {
        if (this.NameHash != other.NameHash)
            return false;

        if (other is not BinTreeMap map)
            return false;

        if (this.KeyType != map.KeyType || this.ValueType != map.ValueType)
            return false;

        return this.SequenceEqual(map);
    }

    #region IDictionary
    /// <inheritdoc/>
    public bool ContainsKey(BinTreeProperty key) => this._map.ContainsKey(key);

    /// <inheritdoc/>
    public void Add(BinTreeProperty key, BinTreeProperty value) => this._map.Add(key, value);

    /// <inheritdoc/>
    public bool Remove(BinTreeProperty key) => this._map.Remove(key);

    /// <inheritdoc/>
    public bool TryGetValue(BinTreeProperty key, [MaybeNullWhen(false)] out BinTreeProperty value) =>
        this._map.TryGetValue(key, out value);
    #endregion

    #region Enumerator
    /// <inheritdoc/>
    public IEnumerator<KeyValuePair<BinTreeProperty, BinTreeProperty>> GetEnumerator() => this._map.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => this._map.GetEnumerator();
    #endregion

    #region ICollection
    void ICollection<KeyValuePair<BinTreeProperty, BinTreeProperty>>.Add(
        KeyValuePair<BinTreeProperty, BinTreeProperty> item
    ) => this._map.Add(item.Key, item.Value);

    bool ICollection<KeyValuePair<BinTreeProperty, BinTreeProperty>>.Remove(
        KeyValuePair<BinTreeProperty, BinTreeProperty> item
    ) => this._map.Remove(item.Key);

    void ICollection<KeyValuePair<BinTreeProperty, BinTreeProperty>>.Clear() => this._map.Clear();

    bool ICollection<KeyValuePair<BinTreeProperty, BinTreeProperty>>.Contains(
        KeyValuePair<BinTreeProperty, BinTreeProperty> item
    ) => this._map.Contains(item);

    void ICollection<KeyValuePair<BinTreeProperty, BinTreeProperty>>.CopyTo(
        KeyValuePair<BinTreeProperty, BinTreeProperty>[] array,
        int arrayIndex
    ) => throw new NotImplementedException();
    #endregion
}
