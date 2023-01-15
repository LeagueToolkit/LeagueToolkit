using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueToolkit.Core.Renderer
{
    public sealed class Texture { }

    public enum TextureFilter
    {
        None = 0,
        Nearest = 1,
        Linear = 2
    }

    public enum TextureAddress
    {
        Wrap = 0,
        Clamp = 1
    }
}
