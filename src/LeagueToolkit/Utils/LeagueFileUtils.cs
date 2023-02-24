using LeagueToolkit.Core.Animation;
using System.Buffers.Binary;

namespace LeagueToolkit.Utils;

public static class LeagueFileUtils
{
    public static LeagueFileType GetFileType(ReadOnlySpan<byte> magicData)
    {
        return magicData switch
        {
            { Length: >= 8 } when magicData[..8].SequenceEqual("r3d2Mesh"u8) => LeagueFileType.StaticObjectBinary,
            { Length: >= 8 } when magicData[..8].SequenceEqual("r3d2Mesh"u8) => LeagueFileType.Skeleton,
            { Length: >= 8 } when magicData[..8].SequenceEqual("r3d2anmd"u8) => LeagueFileType.Animation,
            { Length: >= 8 } when magicData[..8].SequenceEqual("r3d2canm"u8) => LeagueFileType.Animation,
            { Length: >= 8 }
                when magicData[..4].SequenceEqual("r3d2"u8)
                    && BinaryPrimitives.ReadUInt32LittleEndian(magicData[4..]) is 1
                => LeagueFileType.WwisePackage,
            { Length: >= 4 } when magicData[1..4].SequenceEqual("PNG"u8) => LeagueFileType.Png,
            { Length: >= 4 } when magicData[..4].SequenceEqual("DDS\0"u8) => LeagueFileType.TextureDds,
            { Length: >= 4 } when BinaryPrimitives.ReadUInt32LittleEndian(magicData) is 0x00112233
                => LeagueFileType.SimpleSkin,
            { Length: >= 4 } when magicData[..4].SequenceEqual("PROP"u8) => LeagueFileType.PropertyBin,
            { Length: >= 4 } when magicData[..4].SequenceEqual("BKHD"u8) => LeagueFileType.WwiseBank,
            { Length: >= 4 } when magicData[..4].SequenceEqual("WGEO"u8) => LeagueFileType.WorldGeometry,
            { Length: >= 4 } when magicData[..4].SequenceEqual("OEGM"u8) => LeagueFileType.MapGeometry,
            { Length: >= 4 } when magicData[..4].SequenceEqual("[Obj"u8) => LeagueFileType.StaticObjectAscii,
            { Length: >= 5 } when magicData[1..4].SequenceEqual("LuaQ"u8) => LeagueFileType.LuaObj,
            { Length: >= 7 } when magicData[..7].SequenceEqual("PreLoad"u8) => LeagueFileType.Preload,
            { Length: >= 4 } when BinaryPrimitives.ReadUInt32LittleEndian(magicData) is 3 => LeagueFileType.LightGrid,
            { Length: >= 3 } when magicData[..3].SequenceEqual("RST"u8) => LeagueFileType.RiotStringTable,
            { Length: >= 4 } when magicData[..4].SequenceEqual("PTCH"u8) => LeagueFileType.PropertyBinOverride,
            { Length: >= 3 } when (BinaryPrimitives.ReadUInt32LittleEndian(magicData) & 0xFFFFFF00) is 0xFFD8FF00
                => LeagueFileType.Jpeg,
            { Length: >= 8 } when BinaryPrimitives.ReadInt32LittleEndian(magicData[4..]) is RigResource.FORMAT_TOKEN
                => LeagueFileType.Skeleton,
            { Length: >= 4 } when magicData[..4].SequenceEqual("TEX\0"u8) => LeagueFileType.Texture,
            _ => LeagueFileType.Unknown
        };
    }

    public static LeagueFileType GetFileType(Stream stream, int magicSizeHint = 8)
    {
        long originalPosition = stream.Position;
        Span<byte> header = stackalloc byte[magicSizeHint];

        stream.Read(header);
        stream.Seek(originalPosition, SeekOrigin.Begin);

        return GetFileType(header);
    }

    public static LeagueFileType GetFileType(ReadOnlySpan<char> extension)
    {
        if (extension.IsEmpty)
            return LeagueFileType.Unknown;

        if (extension[0] is '.')
            extension = extension[1..];

        return extension switch
        {
            "anm" => LeagueFileType.Animation,
            "bin" => LeagueFileType.PropertyBin,
            "bnk" => LeagueFileType.WwiseBank,
            "dds" => LeagueFileType.TextureDds,
            "jpg" => LeagueFileType.Jpeg,
            "luaobj" => LeagueFileType.LuaObj,
            "mapgeo" => LeagueFileType.MapGeometry,
            "png" => LeagueFileType.Png,
            "preload" => LeagueFileType.Preload,
            "rst" => LeagueFileType.RiotStringTable,
            "scb" => LeagueFileType.StaticObjectBinary,
            "sco" => LeagueFileType.StaticObjectAscii,
            "skl" => LeagueFileType.Skeleton,
            "skn" => LeagueFileType.SimpleSkin,
            "tex" => LeagueFileType.Texture,
            "wgeo" => LeagueFileType.WorldGeometry,
            "wpk" => LeagueFileType.WwisePackage,
            _ => LeagueFileType.Unknown,
        };
    }

    public static string GetExtension(LeagueFileType extensionType)
    {
        return extensionType switch
        {
            LeagueFileType.Animation => "anm",
            LeagueFileType.Jpeg => "jpg",
            LeagueFileType.LightGrid => "dat",
            LeagueFileType.LuaObj => "luaobj",
            LeagueFileType.MapGeometry => "mapgeo",
            LeagueFileType.Png => "png",
            LeagueFileType.Preload => "preload",
            LeagueFileType.PropertyBin => "bin",
            LeagueFileType.PropertyBinOverride => "bin",
            LeagueFileType.RiotStringTable => "txt",
            LeagueFileType.SimpleSkin => "skn",
            LeagueFileType.Skeleton => "skl",
            LeagueFileType.StaticObjectAscii => "sco",
            LeagueFileType.StaticObjectBinary => "scb",
            LeagueFileType.Texture => "tex",
            LeagueFileType.TextureDds => "dds",
            LeagueFileType.WorldGeometry => "wgeo",
            LeagueFileType.WwiseBank => "bnk",
            LeagueFileType.WwisePackage => "wpk",
            _ => "",
        };
    }
}

public enum LeagueFileType
{
    Animation,
    Jpeg,
    LightGrid,
    LuaObj,
    MapGeometry,
    Png,
    Preload,
    PropertyBin,
    PropertyBinOverride,
    RiotStringTable,
    SimpleSkin,
    Skeleton,
    StaticObjectAscii,
    StaticObjectBinary,
    Texture,
    TextureDds,
    WadArchive,
    WorldGeometry,
    WwiseBank,
    WwisePackage,

    Unknown,
}
