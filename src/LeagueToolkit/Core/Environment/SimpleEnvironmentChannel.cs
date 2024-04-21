using System.Diagnostics;
using System.Numerics;
using LeagueToolkit.Core.Primitives;
using LeagueToolkit.Utils.Extensions;

namespace LeagueToolkit.Core.Environment;

[DebuggerDisplay("{Name}")]
internal readonly struct SimpleEnvironmentChannel
{
    public Color Color { get; }
    public string Texture { get; }
    public Matrix4x4 Transform { get; }

    public SimpleEnvironmentChannel(string name, Color color, Matrix4x4 matrix)
    {
        this.Texture = name;
        this.Color = color;
        this.Transform = matrix;
    }

    public static SimpleEnvironmentChannel Read(BinaryReader br)
    {
        Color color = br.ReadColor(ColorFormat.RgbaF32);
        string name = br.ReadPaddedString(260);
        Matrix4x4 transform = br.ReadMatrix4x4RowMajor();

        return new(name, color, transform);
    }
}
