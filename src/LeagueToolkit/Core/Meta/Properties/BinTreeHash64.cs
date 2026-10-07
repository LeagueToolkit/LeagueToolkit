using System.Diagnostics;

namespace LeagueToolkit.Core.Meta.Properties;

/// <summary>
/// Preserves the wide Hash-tagged name used by current PBE static materials.
/// </summary>
[DebuggerDisplay("{Value,x16}")]
public sealed class BinTreeHash64 : BinTreeProperty
{
    public override BinPropertyType Type => BinPropertyType.Hash;

    public ulong Value { get; set; }

    public BinTreeHash64(uint nameHash, ulong value) : base(nameHash) => this.Value = value;

    internal BinTreeHash64(BinaryReader reader, uint nameHash) : base(nameHash) => this.Value = reader.ReadUInt64();

    protected override void WriteContent(BinaryWriter writer) => writer.Write(this.Value);

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + sizeof(ulong);

    public override bool Equals(BinTreeProperty other) =>
        other is BinTreeHash64 property && this.NameHash == property.NameHash && this.Value == property.Value;
}
