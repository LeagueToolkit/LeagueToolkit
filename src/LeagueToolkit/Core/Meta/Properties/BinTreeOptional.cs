using System.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

/// <summary>
/// Represents a property with an optional <see cref="BinTreeProperty"/> value
/// </summary>
[DebuggerDisplay("{Value, nq}", Name = "{GetDebuggerDisplayName(), nq}")]
public sealed class BinTreeOptional : BinTreeProperty
{
    /// <inheritdoc/>
    public override BinPropertyType Type => BinPropertyType.Optional;

    public BinPropertyType ValueType => this._valueType;
    private BinPropertyType _valueType;

    /// <summary>
    /// Gets the optional value of the property
    /// </summary>
    /// <remarks>
    /// Set to <see langword="null"/> if there is no value
    /// </remarks>
    public BinTreeProperty Value
    {
        get => this._value;
        set
        {
            this._value = value;
            if (value is not null)
                this._valueType = value.Type;
        }
    }
    private BinTreeProperty _value;

    /// <summary>
    /// Creates a new <see cref="BinTreeOptional"/> object with the specified parameters
    /// </summary>
    /// <param name="nameHash">The hashed property name</param>
    /// <param name="value">The value of the property</param>
    public BinTreeOptional(uint nameHash, BinTreeProperty value) : base(nameHash) => this.Value = value;

    internal BinTreeOptional(BinaryReader br, uint nameHash, bool useLegacyType = false) : base(nameHash)
    {
        this._valueType = BinUtilities.UnpackType((BinPropertyType)br.ReadByte(), useLegacyType);
        bool isSome = br.ReadBoolean();

        if (isSome)
            this.Value = ReadPropertyContent(0, this._valueType, br, useLegacyType);
    }

    protected override void WriteContent(BinaryWriter bw)
    {
        bw.Write((byte)this.ValueType);
        bw.Write(this.Value is not null);

        this.Value?.Write(bw, false);
    }

    //i<3functionalprogrammingi<3functionalprogrammingi<3functionalprogramming
    internal override int GetSize(bool includeHeader) =>
        (includeHeader ? 5 : 0)
        + 2
        + (
            this.Value switch
            {
                null => 0,
                BinTreeProperty someValue => someValue.GetSize(includeHeader: false)
            }
        );

    public override bool Equals(BinTreeProperty other)
    {
        if (this.NameHash != other?.NameHash)
            return false;

        if (other is not BinTreeOptional optional)
            return false;

        return this.Value.Equals(optional.Value);
    }
}
