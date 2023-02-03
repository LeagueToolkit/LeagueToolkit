using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueToolkit.Core.Wad;

public static class WadBaker
{
    public static void BakeDirectory(string path, WadBakeSettings settings)
    {
        BakeDirectory(new() { Path = path, Setings = settings });
    }

    private static void BakeDirectory(WadBakeContext context) { }
}

internal sealed class WadBakeContext
{
    public string Path { get; init; }
    public WadBakeSettings Setings { get; init; }
}

public struct WadBakeSettings
{
    public bool DetectDuplicateChunkData { get; set; }

    public WadBakeSettings()
    {
        this.DetectDuplicateChunkData = true;
    }
}
