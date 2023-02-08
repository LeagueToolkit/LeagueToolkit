using LeagueToolkit.Core.Meta.Properties;

namespace LeagueToolkit.Core.Meta;

public abstract class BinTreeProperty : IEquatable<BinTreeProperty>
{
    protected const int HEADER_SIZE = 5;

    public abstract BinPropertyType Type { get; }

    public uint NameHash { get; private set; }

    protected BinTreeProperty(uint nameHash)
    {
        this.NameHash = nameHash;
    }

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
            BinPropertyType.I8 => new BinTreeSByte(br, nameHash),
            BinPropertyType.U8 => new BinTreeByte(br, nameHash),
            BinPropertyType.I16 => new BinTreeInt16(br, nameHash),
            BinPropertyType.U16 => new BinTreeUInt16(br, nameHash),
            BinPropertyType.I32 => new BinTreeInt32(br, nameHash),
            BinPropertyType.U32 => new BinTreeUInt32(br, nameHash),
            BinPropertyType.I64 => new BinTreeInt64(br, nameHash),
            BinPropertyType.U64 => new BinTreeUInt64(br, nameHash),
            BinPropertyType.F32 => new BinTreeFloat(br, nameHash),
            BinPropertyType.Vector2 => new BinTreeVector2(br, nameHash),
            BinPropertyType.Vector3 => new BinTreeVector3(br, nameHash),
            BinPropertyType.Vector4 => new BinTreeVector4(br, nameHash),
            BinPropertyType.Matrix44 => new BinTreeMatrix44(br, nameHash),
            BinPropertyType.Color => new BinTreeColor(br, nameHash),
            BinPropertyType.String => new BinTreeString(br, nameHash),
            BinPropertyType.Hash => new BinTreeHash(br, nameHash),
            BinPropertyType.WadChunkLink => new BinTreeWadEntryLink(br, nameHash),
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
