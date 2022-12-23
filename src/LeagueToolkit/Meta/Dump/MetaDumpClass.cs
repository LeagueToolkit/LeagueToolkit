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
        public string ParentClass { get; private set; }

        [JsonProperty(PropertyName = "defaults")]
        public Dictionary<string, object> Defaults { get; private set; }

        [JsonProperty(PropertyName = "fn")]
        public MetaDumpClassFunctions Functions { get; private set; }

        [JsonProperty(PropertyName = "is")]
        public MetaDumpClassIs Is { get; private set; }

        [JsonProperty(PropertyName = "properties")]
        public Dictionary<string, MetaDumpProperty> Properties { get; private set; }

        [JsonProperty(PropertyName = "secondary_bases")]
        public Dictionary<string, uint> Implements { get; private set; }

        [JsonProperty(PropertyName = "secondary_children")]
        public Dictionary<string, uint> ImplementedBy { get; private set; }

        [JsonProperty(PropertyName = "size")]
        public uint Size { get; private set; }

        // TODO: Cleanup
        internal IEnumerable<string> TakeInterfaces(
            IReadOnlyDictionary<string, MetaDumpClass> classes,
            bool includeMainParent
        )
        {
            if (
                includeMainParent
                && this.ParentClass is not null
                && classes.TryGetValue(this.ParentClass, out MetaDumpClass parentInterface)
                && parentInterface.Is.Interface
            )
            {
                yield return this.ParentClass;
                foreach (string @interface in parentInterface.TakeInterfacesRecursive(classes, true))
                {
                    yield return @interface;
                }
            }

            for (int i = 0; i < this.Implements.Count; i++)
            {
                yield return this.Implements.ElementAt(i).Key;
            }
        }

        // TODO: Cleanup
        internal IEnumerable<string> TakeInterfacesRecursive(
            IReadOnlyDictionary<string, MetaDumpClass> classes,
            bool includeMainParent
        )
        {
            if (
                includeMainParent
                && this.ParentClass is not null
                && classes.TryGetValue(this.ParentClass, out MetaDumpClass parentInterface)
                && parentInterface.Is.Interface
            )
            {
                yield return this.ParentClass;
                foreach (string @interface in parentInterface.TakeInterfacesRecursive(classes, true))
                {
                    yield return @interface;
                }
            }

            for (int i = 0; i < this.Implements.Count; i++)
            {
                string interfaceHash = this.Implements.ElementAt(i).Key;

                yield return interfaceHash;

                if (classes.TryGetValue(interfaceHash, out MetaDumpClass interfaceClass) && interfaceClass.Is.Interface)
                {
                    foreach (string @interface in interfaceClass.TakeInterfacesRecursive(classes, true))
                    {
                        yield return @interface;
                    }
                }
                else
                {
                    ThrowHelper.ThrowInvalidOperationException($"Failed to find interface: {interfaceHash}");
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
