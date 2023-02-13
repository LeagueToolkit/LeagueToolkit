using LeagueToolkit.Helpers.Extensions;
using LeagueToolkit.Helpers.Structures;
using System.Numerics;

namespace LeagueToolkit.Core.Environment.SimpleEnvironment;

internal readonly struct SimpleEnvironmentMaterial
{
    public string Name { get; }
    public SimpleEnvironmentMaterialType Type { get; }
    public SimpleEnvironmentMaterialFlags Flags { get; }
    public SimpleEnvironmentChannel[] Channels { get; }

    public SimpleEnvironmentMaterial(
        string name,
        SimpleEnvironmentMaterialType type,
        SimpleEnvironmentMaterialFlags flags,
        IEnumerable<SimpleEnvironmentChannel> channels
    )
    {
        this.Name = name;
        this.Type = type;
        this.Flags = flags;
        this.Channels = channels.ToArray();
    }

    public static SimpleEnvironmentMaterial Read(BinaryReader br)
    {
        string name = br.ReadPaddedString(260);
        SimpleEnvironmentMaterialType type = (SimpleEnvironmentMaterialType)br.ReadInt32();
        SimpleEnvironmentMaterialFlags flags = (SimpleEnvironmentMaterialFlags)br.ReadUInt32();
        SimpleEnvironmentChannel[] channels = new SimpleEnvironmentChannel[8];

        for (int i = 0; i < 8; i++)
            channels[i] = SimpleEnvironmentChannel.Read(br);

        return new(name, type, flags, channels);
    }

    public static SimpleEnvironmentMaterial ReadOld(BinaryReader br)
    {
        string name = br.ReadPaddedString(260);
        SimpleEnvironmentMaterialType type = (SimpleEnvironmentMaterialType)br.ReadInt32();
        SimpleEnvironmentChannel[] channels = new SimpleEnvironmentChannel[8];

        Color diffuseColor = br.ReadColor(ColorFormat.RgbaF32);
        string diffuseName = br.ReadPaddedString(260);
        channels[0] = new(diffuseName, diffuseColor, Matrix4x4.Identity);

        Color emmisiveColor = br.ReadColor(ColorFormat.RgbaF32);
        string emissiveName = br.ReadPaddedString(260);
        channels[1] = new(emissiveName, emmisiveColor, Matrix4x4.Identity);

        return new(name, type, 0, channels);
    }
}

public enum SimpleEnvironmentMaterialType : int
{
    Default = 0,
    Decal = 1,
    WallOfGrass = 2,
    FourBlend = 3,
    AntiBrush = 4
};

[Flags]
public enum SimpleEnvironmentMaterialFlags : int
{
    Ground = 1 << 0,
    NoShadow = 1 << 1,
    VertexAlpha = 1 << 2,
    Lightmapped = 1 << 3,
    DualVertexColor = 1 << 4,
    Background = 1 << 5,
    BackgroundWithFog = 1 << 6,
}

public class MaterialInvalidChannelCountException : Exception
{
    public MaterialInvalidChannelCountException(int actual)
        : base(string.Format("There have to be exactly 8 channels in a material ({0} channel(s) specified).", actual))
    { }
}
