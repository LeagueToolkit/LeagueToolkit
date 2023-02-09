using System.Diagnostics;
using System.Text;

namespace LeagueToolkit.Core.Meta.Properties;

[DebuggerDisplay("{Value, nq}", Name = "{_debuggerDisplayName, nq}")]
public sealed class BinTreeString : BinTreeProperty
{
    public override BinPropertyType Type => BinPropertyType.String;

    public string Value { get; set; }

    public BinTreeString(uint nameHash, string value) : base(nameHash) => this.Value = value;

    internal BinTreeString(BinaryReader br, uint nameHash) : base(nameHash) =>
        this.Value = Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));

    protected override void WriteContent(BinaryWriter bw)
    {
        bw.Write((ushort)this.Value.Length);
        bw.Write(Encoding.UTF8.GetBytes(this.Value));
    }

    internal override int GetSize(bool includeHeader) => (includeHeader ? HEADER_SIZE : 0) + 2 + this.Value.Length;

    public override bool Equals(BinTreeProperty other) =>
        other switch
        {
            BinTreeString property => this.NameHash == property.NameHash && this.Value == property.Value,
            _ => false
        };

    public static implicit operator string(BinTreeString property) => property.Value;
}
