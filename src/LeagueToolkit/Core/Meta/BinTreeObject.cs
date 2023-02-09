using CommunityToolkit.Diagnostics;
using LeagueToolkit.Hashing;
using System.Diagnostics;

namespace LeagueToolkit.Core.Meta;

/// <summary>
/// Represents an object instance in a <see cref="BinTree"/>
/// </summary>
[DebuggerDisplay("{GetDebuggerDisplayValue(), nq}", Name = "{GetDebuggerDisplayName(), nq}")]
public sealed class BinTreeObject : IEquatable<BinTreeObject>
{
    /// <summary>
    /// Gets the meta class hash
    /// </summary>
    /// <remarks>
    /// The meta class is hashed using <see cref="Fnv1a.HashLower(string)"/>
    /// </remarks>
    public uint ClassHash { get; }

    /// <summary>
    /// Gets the path hash
    /// </summary>
    /// <remarks>
    /// The path is hashed using <see cref="Fnv1a.HashLower(string)"/>
    /// </remarks>
    public uint PathHash { get; }

    /// <summary>
    /// Gets the properties
    /// </summary>
    public Dictionary<uint, BinTreeProperty> Properties { get; } = new();

    /// <summary>
    /// Creates a new <see cref="BinTreeObject"/> object with the specified parameters
    /// </summary>
    /// <param name="path">The path of the object</param>
    /// <param name="metaClass">The meta class of the object</param>
    /// <param name="properties">The properties of the object</param>
    public BinTreeObject(string path, string metaClass, IEnumerable<BinTreeProperty> properties)
        : this(Fnv1a.HashLower(path), Fnv1a.HashLower(metaClass), properties) { }

    /// <summary>
    /// Creates a new <see cref="BinTreeObject"/> object with the specified parameters
    /// </summary>
    /// <param name="pathHash">The path hash of the object</param>
    /// <param name="metaClassHash">The meta class hash of the object</param>
    /// <param name="properties">The properties of the object</param>
    public BinTreeObject(uint pathHash, uint metaClassHash, IEnumerable<BinTreeProperty> properties)
    {
        Guard.IsNotNull(properties, nameof(properties));

        this.ClassHash = metaClassHash;
        this.PathHash = pathHash;
        this.Properties = properties.ToDictionary(x => x.NameHash);
    }

    internal static BinTreeObject Read(uint classHash, BinaryReader br, bool useLegacyType = false)
    {
        uint size = br.ReadUInt32();
        uint pathHash = br.ReadUInt32();

        ushort propertyCount = br.ReadUInt16();

        // We are very smart programmers so we defer the execution of the reading to avoid allocating twice
        // This is definitely safe and not going to break :^)
        return new(pathHash, classHash, ReadProperties());

        IEnumerable<BinTreeProperty> ReadProperties()
        {
            for (int i = 0; i < propertyCount; i++)
                yield return BinTreeProperty.Read(br, useLegacyType);
        }
    }

    internal void Write(BinaryWriter bw)
    {
        bw.Write(GetSize());
        bw.Write(this.PathHash);

        bw.Write((ushort)this.Properties.Count);
        foreach (var (_, property) in this.Properties)
            property.Write(bw, writeHeader: true);
    }

    private int GetSize() => 4 + 2 + this.Properties.Sum(x => x.Value.GetSize(includeHeader: true));

    /// <inheritdoc/>
    public bool Equals(BinTreeObject other)
    {
        return this.PathHash == other.PathHash
            && this.ClassHash == other.ClassHash
            && this.Properties.SequenceEqual(other.Properties);
    }

    /// <inheritdoc/>
    public override bool Equals(object obj) =>
        obj switch
        {
            BinTreeObject treeObject => Equals(treeObject),
            _ => false
        };

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(this.PathHash, this.ClassHash);

    private string GetDebuggerDisplayName() => string.Format("{0:x}", this.PathHash);

    private string GetDebuggerDisplayValue() =>
        string.Format("Class: {0:x} Properties: {1}", this.ClassHash, this.Properties.Count);
}
