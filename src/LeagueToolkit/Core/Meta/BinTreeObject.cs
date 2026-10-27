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
        // Legacy bins were written before the client stored any hash in 8 bytes
        if (useLegacyType || br is not BinTreeReader reader)
        {
            br.ReadUInt32(); // size
            return ReadContent(classHash, br, useLegacyType);
        }

        long sizeOffset = br.BaseStream.Position;
        uint size = br.ReadUInt32();
        long contentOffset = br.BaseStream.Position;

        try
        {
            BinTreeObject treeObject = ReadContent(classHash, br, useLegacyType: false);
            if (br.BaseStream.Position == contentOffset + size)
                return treeObject;

            // The object did not end at its declared size: some of its hashes may be 8 bytes wide.
            // If no widths fit, keep what a 4-byte read produced, as before.
            long readEnd = br.BaseStream.Position;
            if (ReadWithResolvedHashWidths(classHash, reader, contentOffset, size) is BinTreeObject resolved)
                return resolved;

            br.BaseStream.Position = readEnd;
            return treeObject;
        }
        catch (Exception exception)
            when (exception
                    is InvalidPropertyTypeException
                        or InvalidDataException
                        or ArgumentException
                        or EndOfStreamException
            )
        {
            if (ReadWithResolvedHashWidths(classHash, reader, contentOffset, size) is BinTreeObject resolved)
                return resolved;

            // Not a width problem; let the caller decide (e.g. retry with the legacy property types)
            br.BaseStream.Position = sizeOffset;
            throw;
        }
    }

    /// <summary>
    /// Reads the object again with the hash widths <see cref="BinHashWidthResolver"/> finds for it,
    /// or returns <see langword="null"/> when no widths fit its declared sizes
    /// </summary>
    private static BinTreeObject ReadWithResolvedHashWidths(
        uint classHash,
        BinTreeReader reader,
        long contentOffset,
        uint size
    )
    {
        if (contentOffset + size > reader.BaseStream.Length)
            return null;

        reader.BaseStream.Position = contentOffset;
        HashSet<long> wideHashOffsets = BinHashWidthResolver.Resolve(reader.ReadBytes((int)size), contentOffset);
        if (wideHashOffsets is null || wideHashOffsets.Count is 0)
            return null;

        reader.WideHashOffsets.UnionWith(wideHashOffsets);
        try
        {
            reader.BaseStream.Position = contentOffset;
            BinTreeObject treeObject = ReadContent(classHash, reader, useLegacyType: false);
            return reader.BaseStream.Position == contentOffset + size ? treeObject : null;
        }
        finally
        {
            reader.WideHashOffsets.Clear();
        }
    }

    private static BinTreeObject ReadContent(uint classHash, BinaryReader br, bool useLegacyType)
    {
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
