using LeagueToolkit.Core.Meta.Properties;
using LeagueToolkit.Hashing;

namespace LeagueToolkit.Core.Meta;

/// <summary>
/// Represents a <see cref="BinTree"/> property
/// </summary>
public abstract class BinTreeProperty : IEquatable<BinTreeProperty>
{
    protected const int HEADER_SIZE = 5;

    /// <summary>
    /// Gets the type of the property
    /// </summary>
    public abstract BinPropertyType Type { get; }

    /// <summary>
    /// Gets the name hash
    /// </summary>
    /// <remarks>
    /// The name is hashed using <see cref="Fnv1a.HashLower(string)"/>
    /// </remarks>
    public uint NameHash { get; }

    protected BinTreeProperty(uint nameHash) => this.NameHash = nameHash;

    internal static BinTreeProperty Read(BinaryReader br, bool useLegacyType = false)
    {
        uint nameHash = br.ReadUInt32();
        BinPropertyType type = BinUtilities.UnpackType((BinPropertyType)br.ReadByte(), useLegacyType);

        return ReadPropertyContent(nameHash, type, br, useLegacyType);
    }

    internal static BinTreeProperty ReadPropertyContent(
        uint nameHash,
        BinPropertyType type,
        BinaryReader br,
        bool useLegacyType = false
    )
    {
        return type switch
        {
            BinPropertyType.None => new BinTreeNone(nameHash),
            BinPropertyType.Bool => new BinTreeBool(br, nameHash),
            BinPropertyType.I8 => new BinTreeI8(br, nameHash),
            BinPropertyType.U8 => new BinTreeU8(br, nameHash),
            BinPropertyType.I16 => new BinTreeI16(br, nameHash),
            BinPropertyType.U16 => new BinTreeU16(br, nameHash),
            BinPropertyType.I32 => new BinTreeI32(br, nameHash),
            BinPropertyType.U32 => new BinTreeU32(br, nameHash),
            BinPropertyType.I64 => new BinTreeI64(br, nameHash),
            BinPropertyType.U64 => new BinTreeU64(br, nameHash),
            BinPropertyType.F32 => new BinTreeF32(br, nameHash),
            BinPropertyType.Vector2 => new BinTreeVector2(br, nameHash),
            BinPropertyType.Vector3 => new BinTreeVector3(br, nameHash),
            BinPropertyType.Vector4 => new BinTreeVector4(br, nameHash),
            BinPropertyType.Matrix44 => new BinTreeMatrix44(br, nameHash),
            BinPropertyType.Color => new BinTreeColor(br, nameHash),
            BinPropertyType.String => new BinTreeString(br, nameHash),
            BinPropertyType.Hash => new BinTreeHash(br, nameHash),
            BinPropertyType.WadChunkLink => new BinTreeWadChunkLink(br, nameHash),
            BinPropertyType.Container => new BinTreeContainer(br, nameHash, useLegacyType),
            BinPropertyType.UnorderedContainer => new BinTreeUnorderedContainer(br, nameHash, useLegacyType),
            BinPropertyType.Struct => new BinTreeStruct(br, nameHash, useLegacyType),
            BinPropertyType.Embedded => new BinTreeEmbedded(br, nameHash, useLegacyType),
            BinPropertyType.ObjectLink => new BinTreeObjectLink(br, nameHash),
            BinPropertyType.Optional => new BinTreeOptional(br, nameHash, useLegacyType),
            BinPropertyType.Map => new BinTreeMap(br, nameHash, useLegacyType),
            BinPropertyType.BitBool => new BinTreeBitBool(br, nameHash),
            _ => throw new InvalidPropertyTypeException(type),
        };
    }

    internal virtual void Write(BinaryWriter bw, bool writeHeader)
    {
        if (writeHeader)
            WriteHeader(bw);

        WriteContent(bw);
    }

    protected void WriteHeader(BinaryWriter bw)
    {
        bw.Write(this.NameHash);
        bw.Write((byte)this.Type);
    }

    protected abstract void WriteContent(BinaryWriter bw);

    internal abstract int GetSize(bool includeHeader);

    public abstract bool Equals(BinTreeProperty other);

    public override bool Equals(object obj) =>
        obj switch
        {
            BinTreeProperty property => Equals(property),
            _ => false
        };

    public override int GetHashCode() => (int)this.NameHash;

    protected virtual string GetDebuggerDisplayName() => string.Format("{0:x}: {1}", this.NameHash, this.Type);
}

public static class BinTreePropertyDictionaryExtensions
{
    public static void Add(this Dictionary<uint, BinTreeProperty> dictionary, BinTreeProperty value) =>
        dictionary.Add(value.NameHash, value);

    public static bool TryAdd(this Dictionary<uint, BinTreeProperty> dictionary, BinTreeProperty value) =>
        dictionary.TryAdd(value.NameHash, value);

    public static TValue GetValueOrDefault<TValue>(this Dictionary<uint, BinTreeProperty> dictionary, uint nameHash)
        where TValue : BinTreeProperty =>
        TryGetValue(dictionary, nameHash, out TValue value) switch
        {
            true => value,
            false => default
        };

    public static bool TryGetValue<TValue>(
        this Dictionary<uint, BinTreeProperty> dictionary,
        uint nameHash,
        out TValue value
    ) where TValue : BinTreeProperty
    {
        (bool result, value) = dictionary.TryGetValue(nameHash, out BinTreeProperty property) switch
        {
            true when property is TValue concreteProperty => (true, concreteProperty),
            _ => (false, default)
        };

        return result;
    }
}

public enum BinPropertyType : byte
{
    // PRIMITIVE TYPES
    None,
    Bool,
    I8,
    U8,
    I16,
    U16,
    I32,
    U32,
    I64,
    U64,
    F32,
    Vector2,
    Vector3,
    Vector4,
    Matrix44,
    Color,
    String,
    Hash,
    WadChunkLink,

    // COMPLEX TYPES
    Container = 128 | 0,
    UnorderedContainer = 128 | 1,
    Struct = 128 | 2,
    Embedded = 128 | 3,
    ObjectLink = 128 | 4,
    Optional = 128 | 5,
    Map = 128 | 6,
    BitBool = 128 | 7
}
