using CommunityToolkit.Diagnostics;
using LeagueToolkit.Hashing;
using System.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

/// <summary>
/// Represents a property which imitates a pointer to a struct
/// </summary>
[DebuggerDisplay("{_debuggerDisplay, nq}", Name = "{_debuggerDisplayName, nq}")]
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

    private string _debuggerDisplay => string.Format("Class: {0:x}", this.ClassHash);

    /// <summary>
    /// Gets the properties of the struct
    /// </summary>
    public IReadOnlyList<BinTreeProperty> Properties => this._properties;
    protected List<BinTreeProperty> _properties = new();

    /// <summary>
    /// Creates a new <see cref="BinTreeStruct"/> object with the specified parameters
    /// </summary>
    /// <param name="nameHash">The hashed property name</param>
    /// <param name="classHash">The hashed class name</param>
    /// <param name="properties">The properties of the class</param>
    public BinTreeStruct(uint nameHash, uint classHash, IEnumerable<BinTreeProperty> properties) : base(nameHash)
    {
        this.ClassHash = classHash;
        this._properties = properties.ToList();

        // Verify properties
        foreach (BinTreeProperty property in this.Properties)
            if (properties.Any(x => x.NameHash == property.NameHash && x != property))
                throw new ArgumentException($"Found two properties with the same name hash: {property.NameHash}");
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
            this._properties.Add(Read(br, useLegacyType));

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
        foreach (BinTreeProperty property in this.Properties)
            property.Write(bw, writeHeader: true);
    }

    /// <summary>
    /// Adds the specified <see cref="BinTreeProperty"/> into the struct
    /// </summary>
    /// <param name="property">The property to add</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when a <see cref="BinTreeProperty"/> with the same name hash already exists
    /// </exception>
    public void AddProperty(BinTreeProperty property)
    {
        if (this._properties.Any(x => x.NameHash == property.NameHash))
            throw new InvalidOperationException("A property with the same name already exists");

        this._properties.Add(property);
    }

    /// <summary>
    /// Removes the specified <see cref="BinTreeProperty"/> from the struct
    /// </summary>
    /// <param name="property">The property to remove</param>
    /// /// <returns><see langword="true"/> if <paramref name="property"/> was successfully removed; otherwise <see langword="false"/></returns>
    public bool RemoveProperty(BinTreeProperty property) => this._properties.Remove(property);

    /// <summary>
    /// Removes a <see cref="BinTreeProperty"/> with the specified name hash from the struct
    /// </summary>
    /// <param name="nameHash">The hashed name of the property to remove</param>
    /// /// <returns><see langword="true"/> if <paramref name="nameHash"/> was successfully removed; otherwise <see langword="false"/></returns>
    public bool RemoveProperty(uint nameHash) =>
        RemoveProperty(this._properties.FirstOrDefault(x => x.NameHash == nameHash));

    internal override int GetSize(bool includeHeader) =>
        this.ClassHash switch
        {
            0 => (includeHeader ? HEADER_SIZE : 0) + 4,
            _ => (includeHeader ? HEADER_SIZE : 0) + 4 + 4 + GetContentSize()
        };

    private int GetContentSize() => 2 + this.Properties.Sum(x => x.GetSize(includeHeader: true));

    public override bool Equals(BinTreeProperty other)
    {
        if (this.NameHash != other.NameHash)
            return false;

        if (other is BinTreeStruct otherProperty && other is not BinTreeEmbedded)
        {
            if (this.ClassHash != otherProperty.ClassHash)
                return false;
            if (this._properties.Count != otherProperty._properties.Count)
                return false;

            for (int i = 0; i < this._properties.Count; i++)
            {
                if (!this._properties[i].Equals(otherProperty._properties[i]))
                    return false;
            }
        }

        return true;
    }
}
