using System.Diagnostics;

namespace LeagueToolkit.Core.Animation;

[DebuggerDisplay("{GetJointId()} | {GetTransformType()}")]
internal unsafe struct CompressedFrame
{
    public readonly ushort Time;
    public readonly ushort JointId;
    public fixed ushort Value[3];

    public ushort GetJointId() => (ushort)(this.JointId & 0x3FFF);

    public CompressedTransformType GetTransformType() => (CompressedTransformType)(this.JointId >> 14);
}

internal enum CompressedTransformType : byte
{
    Rotation = 0,
    Translation = 1,
    Scale = 2
}
