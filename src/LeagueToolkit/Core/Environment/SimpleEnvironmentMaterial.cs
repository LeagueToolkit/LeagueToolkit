using System.Diagnostics;
using System.Numerics;
using LeagueToolkit.Core.Memory;
using LeagueToolkit.Core.Primitives;
using LeagueToolkit.Utils.Extensions;
using static LeagueToolkit.Core.Environment.SimpleEnvironment;

namespace LeagueToolkit.Core.Environment;

[DebuggerDisplay("Type: {Type} Flags: {Flags}", Name = "{Name}")]
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

        Color emissiveColor = br.ReadColor(ColorFormat.RgbaF32);
        string emissiveName = br.ReadPaddedString(260);
        channels[1] = new(emissiveName, emissiveColor, Matrix4x4.Identity);

        return new(name, type, 0, channels);
    }

    public string GetFormattedName()
    {
        if (this.Flags.HasFlag(SimpleEnvironmentMaterialFlags.VertexAlpha))
            return $"NVRMaterial_AlphaTest_{this.Name}";
        else
            return $"NVRMaterial_{this.Name}";
    }

    public VertexBufferDescription GetVertexDeclaration()
    {
        return this.Type switch
        {
            SimpleEnvironmentMaterialType.Default
                => this.Flags.HasFlag(SimpleEnvironmentMaterialFlags.DualVertexColor) switch
                {
                    true => new(VertexBufferUsage.Static, SimpleEnvironmentVertexDescription.DUAL_VERTEX_COLOR),
                    false => new(VertexBufferUsage.Static, SimpleEnvironmentVertexDescription.DEFAULT)
                },
            SimpleEnvironmentMaterialType.Decal
                => new(VertexBufferUsage.Static, SimpleEnvironmentVertexDescription.DEFAULT),
            SimpleEnvironmentMaterialType.WallOfGrass
                => new(VertexBufferUsage.Static, SimpleEnvironmentVertexDescription.DEFAULT),
            SimpleEnvironmentMaterialType.FourBlend
                => new(VertexBufferUsage.Static, SimpleEnvironmentVertexDescription.FOUR_BLEND),
            SimpleEnvironmentMaterialType.AntiBrush
                => new(VertexBufferUsage.Static, SimpleEnvironmentVertexDescription.DEFAULT),
            _ => throw new InvalidOperationException()
        };
    }
}

internal enum SimpleEnvironmentMaterialType : int
{
    Default = 0, // vertex - 4
    Decal = 1, // vertex - 0
    WallOfGrass = 2, // vertex - 0
    FourBlend = 3, // vertex - 2
    AntiBrush = 4 // vertex - 0
};

[Flags]
internal enum SimpleEnvironmentMaterialFlags : int
{
    Ground = 1 << 0,
    NoShadow = 1 << 1,
    VertexAlpha = 1 << 2,
    Lightmapped = 1 << 3,
    DualVertexColor = 1 << 4,
    Background = 1 << 5,
    BackgroundWithFog = 1 << 6,
}
