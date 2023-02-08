namespace LeagueToolkit.Core.Meta;

internal static class BinUtilities
{
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
