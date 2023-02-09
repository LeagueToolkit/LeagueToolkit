using CommunityToolkit.Diagnostics;
using LeagueToolkit.Hashing;
using System.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

/// <summary>
/// Represents a property which imitates a pointer to a struct
/// </summary>
[DebuggerDisplay("{GetDebuggerDisplay(), nq}", Name = "{GetDebuggerDisplayName(), nq}")]
public class BinTreeStruct : BinTreeProperty
{
    /// <inheritdoc/>
    public override BinPropertyType Type => BinPropertyType.Struct;

    /// <summary>
    /// Gets the meta class hash
    /// </summary>
    /// <remarks>
    /// The meta class is hashed using <see cref="Fnv1a.HashLower(string)"/>
    /// </remarks>
    public uint ClassHash { get; }

    /// <summary>
    /// Gets the properties of the struct
    /// </summary>
    public Dictionary<uint, BinTreeProperty> Properties { get; } = new();

    /// <summary>
    /// Creates a new <see cref="BinTreeStruct"/> object with the specified parameters
    /// </summary>
    /// <param name="nameHash">The hashed property name</param>
    /// <param name="classHash">The hashed class name</param>
    /// <param name="properties">The properties of the class</param>
    public BinTreeStruct(uint nameHash, uint classHash, IEnumerable<BinTreeProperty> properties) : base(nameHash)
    {
        this.ClassHash = classHash;
        this.Properties = properties.ToDictionary(x => x.NameHash);
    }

    internal BinTreeStruct(BinaryReader br, uint nameHash, bool useLegacyType = false) : base(nameHash)
    {
        this.ClassHash = br.ReadUInt32();
        if (this.ClassHash is 0)
            return; // Skip

        uint size = br.ReadUInt32();
        long contentOffset = br.BaseStream.Position;

        ushort propertyCount = br.ReadUInt16();
        for (int i = 0; i < propertyCount; i++)
        {
            BinTreeProperty property = Read(br, useLegacyType);

            this.Properties.Add(property.NameHash, property);
        }

        if (br.BaseStream.Position != contentOffset + size)
            ThrowHelper.ThrowInvalidDataException(
                $"Invalid size: {br.BaseStream.Position - contentOffset}, expected {size}"
            );
    }

    protected override void WriteContent(BinaryWriter bw)
    {
        bw.Write(this.ClassHash);
        if (this.ClassHash is 0)
            return; // Skip

        bw.Write(GetContentSize());

        bw.Write((ushort)this.Properties.Count);
        foreach (var (_, property) in this.Properties)
            property.Write(bw, writeHeader: true);
    }

    internal override int GetSize(bool includeHeader) =>
        this.ClassHash switch
        {
            0 => (includeHeader ? HEADER_SIZE : 0) + 4,
            _ => (includeHeader ? HEADER_SIZE : 0) + 4 + 4 + GetContentSize()
        };

    private int GetContentSize() => 2 + this.Properties.Sum(x => x.Value.GetSize(includeHeader: true));

    public override bool Equals(BinTreeProperty other)
    {
        if (this.NameHash != other.NameHash)
            return false;

        return other switch
        {
            BinTreeEmbedded _ => false,
            BinTreeStruct otherStruct
                => this.ClassHash == otherStruct.ClassHash && this.Properties.SequenceEqual(otherStruct.Properties),
            _ => false
        };
    }

    private string GetDebuggerDisplay() => string.Format("Class: {0:x}", this.ClassHash);
}
