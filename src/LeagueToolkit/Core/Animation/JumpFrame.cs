using LeagueToolkit.Core.Primitives;
using System;
using System.Diagnostics;
using System.Numerics;

namespace LeagueToolkit.Core.Animation;

[DebuggerDisplay("{GetDebuggerDisplay(), nq}")]
internal unsafe struct JumpFrameU16
{
    public fixed ushort RotationKeys[4];
    public fixed ushort TranslationKeys[4];
    public fixed ushort ScaleKeys[4];

    private string GetDebuggerDisplay()
    {
        fixed (ushort* rotationKeys = this.TranslationKeys)
        fixed (ushort* translationKeys = this.TranslationKeys)
        fixed (ushort* scaleKeys = this.ScaleKeys)
        {
            return $"R:[{string.Join(',', rotationKeys[0], rotationKeys[1], rotationKeys[2], rotationKeys[3])}] "
                + $"T:[{string.Join(',', translationKeys[0], translationKeys[1], translationKeys[2], translationKeys[3])}] "
                + $"S:[{string.Join(',', scaleKeys[0], scaleKeys[1], scaleKeys[2], scaleKeys[3])}]";
        }
    }
}

[DebuggerDisplay("{GetDebuggerDisplay(), nq}")]
internal unsafe struct JumpFrameU32
{
    public fixed int RotationKeys[4];
    public fixed int TranslationKeys[4];
    public fixed int ScaleKeys[4];

    private string GetDebuggerDisplay()
    {
        fixed (int* rotationKeys = this.RotationKeys)
        fixed (int* translationKeys = this.TranslationKeys)
        fixed (int* scaleKeys = this.ScaleKeys)
        {
            return $"R:[{string.Join(',', rotationKeys[0], rotationKeys[1], rotationKeys[2], rotationKeys[3])}] "
                + $"T:[{string.Join(',', translationKeys[0], translationKeys[1], translationKeys[2], translationKeys[3])}] "
                + $"S:[{string.Join(',', scaleKeys[0], scaleKeys[1], scaleKeys[2], scaleKeys[3])}]";
        }
    }
}
