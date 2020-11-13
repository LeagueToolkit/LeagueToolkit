using System.Collections.Generic;
using System.IO;

namespace LeagueToolkit.IO.PropertyBin.Properties
{
    public sealed class BinTreeUnorderedContainer : BinTreeContainer
    {
        public override BinPropertyType Type => BinPropertyType.UnorderedContainer;

        public BinTreeUnorderedContainer(IBinTreeParent parent, uint nameHash, BinPropertyType propertiesType, IEnumerable<BinTreeProperty> properties)
            : base(parent, nameHash, propertiesType, properties)
        {

        }
        internal BinTreeUnorderedContainer(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(br, parent, nameHash) { }

        public override bool Equals(BinTreeProperty other)
        {
            if (this.NameHash != other.NameHash) return false;

            if (other is BinTreeUnorderedContainer otherProperty)
            {
                if (this._properties.Count != otherProperty._properties.Count) return false;

                for (int i = 0; i < this._properties.Count; i++)
                {
                    if (!this._properties[i].Equals(otherProperty._properties[i])) return false;
                }
            }

            return true;
        }
    }
}
