using CommunityToolkit.Diagnostics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeagueToolkit.Meta.Dump
{
    public sealed class MetaDumpClass
    {
        [JsonProperty(PropertyName = "alignment")]
        public uint Alignment { get; private set; }

        [JsonProperty(PropertyName = "base")]
        public string Base { get; private set; }

        [JsonProperty(PropertyName = "defaults")]
        public Dictionary<string, object> Defaults { get; private set; }

        [JsonProperty(PropertyName = "fn")]
        public MetaDumpClassFunctions Functions { get; private set; }

        [JsonProperty(PropertyName = "is")]
        public MetaDumpClassIs Is { get; private set; }

        [JsonProperty(PropertyName = "properties")]
        public Dictionary<string, MetaDumpProperty> Properties { get; private set; }

        [JsonProperty(PropertyName = "secondary_bases")]
        public Dictionary<string, uint> SecondaryBases { get; private set; }

        [JsonProperty(PropertyName = "secondary_children")]
        public Dictionary<string, uint> SecondaryChildren { get; private set; }

        [JsonProperty(PropertyName = "size")]
        public uint Size { get; private set; }

        // TODO: Cleanup
        internal IEnumerable<string> TakeSecondaryBases(
            IReadOnlyDictionary<string, MetaDumpClass> classes,
            bool includeMainBase
        )
        {
            if (
                includeMainBase
                && this.Base is not null
                && classes.TryGetValue(this.Base, out MetaDumpClass @base)
                && @base.Is.Interface
            )
            {
                yield return this.Base;
                foreach (string secondaryBase in @base.TakeSecondaryBasesRecursive(classes, true))
                {
                    yield return secondaryBase;
                }
            }

            for (int i = 0; i < this.SecondaryBases.Count; i++)
            {
                yield return this.SecondaryBases.ElementAt(i).Key;
            }
        }

        // TODO: Cleanup
        internal IEnumerable<string> TakeSecondaryBasesRecursive(
            IReadOnlyDictionary<string, MetaDumpClass> classes,
            bool includeMainBase
        )
        {
            if (
                includeMainBase
                && this.Base is not null
                && classes.TryGetValue(this.Base, out MetaDumpClass @base)
                && @base.Is.Interface
            )
            {
                yield return this.Base;
                foreach (string secondaryBase in @base.TakeSecondaryBasesRecursive(classes, true))
                {
                    yield return secondaryBase;
                }
            }

            for (int i = 0; i < this.SecondaryBases.Count; i++)
            {
                string secondaryBaseHash = this.SecondaryBases.ElementAt(i).Key;

                yield return secondaryBaseHash;

                if (
                    classes.TryGetValue(secondaryBaseHash, out MetaDumpClass secondaryBaseClass)
                    && secondaryBaseClass.Is.Interface
                )
                {
                    foreach (string secondaryBase in secondaryBaseClass.TakeSecondaryBasesRecursive(classes, true))
                    {
                        yield return secondaryBase;
                    }
                }
                else
                {
                    ThrowHelper.ThrowInvalidOperationException($"Failed to find interface: {secondaryBaseHash}");
                }
            }
        }
    }

    public sealed class MetaDumpClassFunctions
    {
        [JsonProperty(PropertyName = "constructor")]
        public string Constructor { get; private set; }

        [JsonProperty(PropertyName = "destructor")]
        public string Destructor { get; private set; }

        [JsonProperty(PropertyName = "inplace_constructor")]
        public string InplaceConstructor { get; private set; }

        [JsonProperty(PropertyName = "inplace_destructor")]
        public string InplaceDestructor { get; private set; }

        [JsonProperty(PropertyName = "register")]
        public string Register { get; private set; }

        [JsonProperty(PropertyName = "upcast_secondary")]
        public string UpcastSecondary { get; private set; }
    }

    public sealed class MetaDumpClassIs
    {
        [JsonProperty(PropertyName = "interface")]
        public bool Interface { get; private set; }

        [JsonProperty(PropertyName = "property_base")]
        public bool PropertyBase { get; private set; }

        [JsonProperty(PropertyName = "secondary_base")]
        public bool SecondaryBase { get; private set; }

        [JsonProperty(PropertyName = "unk5")]
        public bool Unknown5 { get; private set; }

        [JsonProperty(PropertyName = "value")]
        public bool Value { get; private set; }
    }
}
