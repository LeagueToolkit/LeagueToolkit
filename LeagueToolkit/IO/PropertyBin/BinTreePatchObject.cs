using System.Collections.Generic;

namespace LeagueToolkit.IO.PropertyBin
{
    public class BinTreePatchObject : IBinNestedProvider
    {
        public uint PathHash { get; }

        public List<BinTreeProperty> Properties { get; } = new();

        public BinTreePatchObject(uint pathHash)
        {
            this.PathHash = pathHash;
        }
    }

    public interface IBinNestedProvider : IBinTreeParent
    {
        public List<BinTreeProperty> Properties { get; }
    }
}
