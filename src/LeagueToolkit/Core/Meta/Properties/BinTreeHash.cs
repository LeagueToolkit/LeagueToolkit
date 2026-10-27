using LeagueToolkit.Hashing;
using System.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

/// <summary>
/// Represents a property with a <see cref="Fnv1a"/> hash value
/// </summary>
/// <remarks>
/// A hash is stored in 4 bytes, or in 8 for the properties the client declares as 64-bit
/// (<c>StaticMaterialDef.name</c> since 16.21). An 8-byte hash keeps its full value in
/// <see cref="WideValue"/> and is written back in 8 bytes.
/// </remarks>
[DebuggerDisplay("{_debuggerDisplay, nq}", Name = "{GetDebuggerDisplayName(), nq}")]
public sealed class BinTreeHash : BinTreeProperty
{
    /// <inheritdoc/>
    public override BinPropertyType Type => BinPropertyType.Hash;

    /// <summary>
    /// Gets the value of the property (for an 8-byte hash, its low 32 bits)
    /// </summary>
    public uint Value
    {
        get => this._value;
        set
        {
            this._value = value;
            if (this._wideValue is ulong wide)
                this._wideValue = (wide & 0xFFFFFFFF00000000UL) | value;
        }
    }
    private uint _value;

    /// <summary>
    /// Gets the full value of an 8-byte hash, or <see langword="null"/> for a 4-byte hash
    /// </summary>
    public ulong? WideValue
    {
        get => this._wideValue;
        set
        {
            this._wideValue = value;
            if (value is ulong wide)
                this._value = (uint)wide;
        }
    }
    private ulong? _wideValue;

    private string _debuggerDisplay =>
        this._wideValue is ulong wide ? string.Format("{0:x16}", wide) : string.Format("{0:x}", this.Value);

    /// <summary>
    /// Creates a new <see cref="BinTreeHash"/> object with the specified parameters
    /// </summary>
    /// <param name="nameHash">The hashed property name</param>
    /// <param name="value">The value of the property</param>
    public BinTreeHash(uint nameHash, uint value) : base(nameHash) => this.Value = value;

    /// <summary>
    /// Creates a new 8-byte <see cref="BinTreeHash"/> object with the specified parameters
    /// </summary>
    /// <param name="nameHash">The hashed property name</param>
    /// <param name="wideValue">The 64-bit value of the property</param>
    public BinTreeHash(uint nameHash, ulong wideValue) : base(nameHash) => this.WideValue = wideValue;

    internal BinTreeHash(BinaryReader br, uint nameHash) : base(nameHash)
    {
        if (br is BinTreeReader reader && reader.IsWideHashAt(br.BaseStream.Position))
            this.WideValue = br.ReadUInt64();
        else
            this.Value = br.ReadUInt32();
    }

    protected override void WriteContent(BinaryWriter bw)
    {
        if (this._wideValue is ulong wide)
            bw.Write(wide);
        else
            bw.Write(this.Value);
    }

    internal override int GetSize(bool includeHeader) =>
        (includeHeader ? HEADER_SIZE : 0) + (this._wideValue is null ? 4 : 8);

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeHash property
                => this.NameHash == property.NameHash
                    && this.Value == property.Value
                    && this.WideValue == property.WideValue,
            _ => false
        };

    public static implicit operator uint(BinTreeHash property) => property.Value;
}
