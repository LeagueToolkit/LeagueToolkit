using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueToolkit.Core.Wad
{
    public static class WadUtils
    {
        // TODO: Refactor this to use Span API
        public static WadChunkCompression GetExtensionCompression(string extension)
        {
            if (string.IsNullOrEmpty(extension))
                return WadChunkCompression.None;

            if (extension[0] == '.')
                extension = extension.Remove(0, 1);

            if (extension.Contains("glsl") || extension.Contains("dx9") || extension.Contains("dx11"))
                return WadChunkCompression.None;

            switch (extension)
            {
                case "wpk":
                case "bnk":
                case "troybin":
                case "inibin":
                case "gfx":
                case "png":
                case "preload":
                    return WadChunkCompression.None;
                default:
                    return WadChunkCompression.Zstd;
            }
        }
    }
}
