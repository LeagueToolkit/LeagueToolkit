using System.IO;

namespace Fantome.Libraries.League.IO.PropertyBin.Properties
{
    public sealed class BinTreeContainer2 : BinTreeContainer
    {
        public override BinPropertyType Type => BinPropertyType.Container2;

        internal BinTreeContainer2(BinaryReader br, IBinTreeParent parent, uint nameHash) : base(br, parent, nameHash) { }

        public override bool Equals(BinTreeProperty other)
        {
            if (this.NameHash != other.NameHash) return false;

            if (other is BinTreeContainer2 otherProperty)
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
