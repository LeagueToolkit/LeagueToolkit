using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueToolkit.Core.Animation
{
    public sealed class UncompressedAnimationAsset : IAnimationAsset
    {
        /// <inheritdoc/>
        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (this.IsDisposed)
                return;

            if (disposing) { }

            this.IsDisposed = true;
        }
    }
}
