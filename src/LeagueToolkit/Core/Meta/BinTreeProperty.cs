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

    internal static BinTreeProperty Read(BinaryReader br, BinPropertyType type, uint nameHash)
    {
        BinTreeProperty x = Read(br, type);
        x.NameHash = nameHash;
        return x;
    }

    internal static BinTreeProperty Read(BinaryReader br, BinPropertyType? type = null)
    {
        uint nameHash = 0;
        BinPropertyType packedType;
        if (type is null)
        {
            nameHash = br.ReadUInt32();
            packedType = (BinPropertyType)br.ReadByte();
            type = BinUtilities.UnpackType(packedType);
        }

        return type switch
        {
            BinPropertyType.None => new BinTreeNone(nameHash),
            BinPropertyType.Bool => new BinTreeBool(br, nameHash),
            BinPropertyType.SByte => new BinTreeSByte(br, nameHash),
            BinPropertyType.Byte => new BinTreeByte(br, nameHash),
            BinPropertyType.Int16 => new BinTreeInt16(br, nameHash),
            BinPropertyType.UInt16 => new BinTreeUInt16(br, nameHash),
            BinPropertyType.Int32 => new BinTreeInt32(br, nameHash),
            BinPropertyType.UInt32 => new BinTreeUInt32(br, nameHash),
            BinPropertyType.Int64 => new BinTreeInt64(br, nameHash),
            BinPropertyType.UInt64 => new BinTreeUInt64(br, nameHash),
            BinPropertyType.Float => new BinTreeFloat(br, nameHash),
            BinPropertyType.Vector2 => new BinTreeVector2(br, nameHash),
            BinPropertyType.Vector3 => new BinTreeVector3(br, nameHash),
            BinPropertyType.Vector4 => new BinTreeVector4(br, nameHash),
            BinPropertyType.Matrix44 => new BinTreeMatrix44(br, nameHash),
            BinPropertyType.Color => new BinTreeColor(br, nameHash),
            BinPropertyType.String => new BinTreeString(br, nameHash),
            BinPropertyType.Hash => new BinTreeHash(br, nameHash),
            BinPropertyType.WadEntryLink => new BinTreeWadEntryLink(br, nameHash),
            BinPropertyType.Container => new BinTreeContainer(br, nameHash),
            BinPropertyType.UnorderedContainer => new BinTreeUnorderedContainer(br, nameHash),
            BinPropertyType.Struct => new BinTreeStruct(br, nameHash),
            BinPropertyType.Embedded => new BinTreeEmbedded(br, nameHash),
            BinPropertyType.ObjectLink => new BinTreeObjectLink(br, nameHash),
            BinPropertyType.Optional => new BinTreeOptional(br, nameHash),
            BinPropertyType.Map => new BinTreeMap(br, nameHash),
            BinPropertyType.BitBool => new BinTreeBitBool(br, nameHash),
            _ => throw new InvalidOperationException("Invalid BinPropertyType: " + type),
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
        bw.Write((byte)BinUtilities.PackType(this.Type));
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
    // PRIMITIVE TYPES \\
    None,
    Bool,
    SByte,
    Byte,
    Int16,
    UInt16,
    Int32,
    UInt32,
    Int64,
    UInt64,
    Float,
    Vector2,
    Vector3,
    Vector4,
    Matrix44,
    Color,
    String,
    Hash,
    WadEntryLink,

    // COMPLEX TYPES \\
    Container,
    UnorderedContainer,
    Struct,
    Embedded,
    ObjectLink,
    Optional,
    Map,
    BitBool
}
