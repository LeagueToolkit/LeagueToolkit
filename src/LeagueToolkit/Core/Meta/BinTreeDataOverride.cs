using CommunityToolkit.Diagnostics;
using System.Text;

namespace LeagueToolkit.Core.Meta;

public sealed class BinTreeDataOverride
{
    public uint ObjectPathHash { get; }
    public string PropertyPath { get; }
    public BinTreeProperty Property { get; }

    public BinTreeDataOverride(uint objectPathHash, string propertyPath, BinTreeProperty property)
    {
        Guard.IsNotNullOrEmpty(propertyPath, nameof(propertyPath));
        Guard.IsNotNull(property, nameof(property));

        this.ObjectPathHash = objectPathHash;
        this.PropertyPath = propertyPath;
        this.Property = property;
    }

    internal static BinTreeDataOverride Read(BinaryReader br)
    {
        uint objectPathHash = br.ReadUInt32();
        uint size = br.ReadUInt32();

        BinPropertyType type = (BinPropertyType)br.ReadByte();
        string propertyPath = Encoding.ASCII.GetString(br.ReadBytes(br.ReadUInt16()));
        BinTreeProperty property = BinTreeProperty.ReadPropertyContent(0, type, br);

        return new(objectPathHash, propertyPath, property);
    }

    internal void Write(BinaryWriter bw)
    {
        bw.Write(this.ObjectPathHash);
        bw.Write(1 + 2 + this.PropertyPath.Length + this.Property.GetSize(includeHeader: false));

        bw.Write((byte)this.Property.Type);
        bw.Write((ushort)this.PropertyPath.Length);
        bw.Write(this.PropertyPath.AsSpan());

        this.Property.Write(bw, writeHeader: false);
    }
}
