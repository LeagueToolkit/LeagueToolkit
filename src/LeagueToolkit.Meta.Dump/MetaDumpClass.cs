using CommunityToolkit.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LeagueToolkit.Meta.Dump
{
    public sealed class MetaDumpClass
    {
        [JsonInclude]
        public uint Alignment { get; private set; }

        [JsonInclude]
        public string Base { get; private set; }

        [JsonInclude]
        public Dictionary<string, JsonElement> Defaults { get; private set; }

        [JsonPropertyName("fn")]
        [JsonInclude]
        public MetaDumpClassFunctions Functions { get; private set; }

        [JsonInclude]
        public MetaDumpClassIs Is { get; private set; }

        [JsonInclude]
        public Dictionary<string, MetaDumpProperty> Properties { get; private set; }

        [JsonPropertyName("secondary_bases")]
        [JsonInclude]
        public Dictionary<string, uint> SecondaryBases { get; private set; }

        [JsonPropertyName("secondary_children")]
        [JsonInclude]
        public Dictionary<string, uint> SecondaryChildren { get; private set; }

        [JsonInclude]
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
        [JsonInclude]
        public string Constructor { get; private set; }

        [JsonInclude]
        public string Destructor { get; private set; }

        [JsonPropertyName("inplace_constructor")]
        [JsonInclude]
        public string InplaceConstructor { get; private set; }

        [JsonPropertyName("inplace_destructor")]
        [JsonInclude]
        public string InplaceDestructor { get; private set; }

        [JsonInclude]
        public string Register { get; private set; }

        [JsonPropertyName("upcast_secondary")]
        [JsonInclude]
        public string UpcastSecondary { get; private set; }
    }

    public sealed class MetaDumpClassIs
    {
        [JsonInclude]
        public bool Interface { get; private set; }

        [JsonPropertyName("property_base")]
        [JsonInclude]
        public bool PropertyBase { get; private set; }

        [JsonPropertyName("secondary_base")]
        [JsonInclude]
        public bool SecondaryBase { get; private set; }

        [JsonPropertyName("unk5")]
        [JsonInclude]
        public bool Unknown5 { get; private set; }

        [JsonInclude]
        public bool Value { get; private set; }
    }
}
