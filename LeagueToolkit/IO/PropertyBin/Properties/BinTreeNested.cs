using System;
using System.Collections.Generic;
using System.IO;

namespace LeagueToolkit.IO.PropertyBin.Properties
{
    // pseudo class for bin patch entries
    public class BinTreeNested : BinTreeProperty, IBinNestedProvider
    {
        public override BinPropertyType Type => BinPropertyType.None;

        public List<(BinTreeProperty, string)> Properties { get; } = new();

        public BinTreeNested(IBinTreeParent parent, uint nameHash) : base(parent, nameHash)
        {
        }

        protected override void WriteContent(BinaryWriter bw) => throw new NotImplementedException();
        internal override int GetSize(bool includeHeader) => throw new NotImplementedException();

        public IEnumerable<(string, BinTreeProperty)> GetObjects()
        {
            foreach ((BinTreeProperty property, string name) in this.Properties)
            {
                if (property is BinTreeNested nested)
                {
                    foreach ((string nestedName, BinTreeProperty p) in nested.GetObjects())
                    {
                        yield return ($"{name}.{nestedName}", p);
                    }
                }
                else
                {
                    yield return (name, property);
                }
            }
        }

        public override bool Equals(BinTreeProperty other)
        {
            if (other is BinTreeNested otherProperty)
            {
                if (this.NameHash != otherProperty.NameHash) return false;
                if (this.Properties.Count != otherProperty.Properties.Count) return false;

                for (int i = 0; i < this.Properties.Count; i++)
                {
                    if (!this.Properties[i].Equals(otherProperty.Properties[i])) return false;
                }

                return true;
            }

            return false;
        }
    }
}
