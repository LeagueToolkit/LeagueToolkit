using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueToolkit.Core.Animation;

public interface IAnimationAsset : IDisposable
{
    /// <summary>
    /// Gets a value indicating whether the asset has been disposed of
    /// </summary>
    bool IsDisposed { get; }
}
