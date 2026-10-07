namespace LeagueToolkit.Core.Meta;

internal static class BinUtilities
{
    internal static void ValidateSize(BinaryReader reader, uint size, uint minimumSize)
    {
        if (size < minimumSize || size > reader.BaseStream.Length - reader.BaseStream.Position)
            throw new InvalidDataException($"Invalid BIN block size {size} at offset {reader.BaseStream.Position}");
    }

    internal static BinPropertyType UnpackType(BinPropertyType type, bool useLegacyType = false)
    {
        if (useLegacyType is false)
            return type;

        if (type >= BinPropertyType.WadChunkLink && type < BinPropertyType.Container)
        {
            type -= BinPropertyType.WadChunkLink;
            type |= BinPropertyType.Container;
        }

        if (type >= BinPropertyType.UnorderedContainer)
            type += 1; // WadChunkLink didn't exist

        return type;
    }
}
