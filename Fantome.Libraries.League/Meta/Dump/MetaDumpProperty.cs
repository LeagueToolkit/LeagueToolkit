using Fantome.Libraries.League.IO.PropertyBin;
using Newtonsoft.Json;

namespace Fantome.Libraries.League.Meta.Dump
{
    public sealed class MetaDumpProperty
    {
        [JsonProperty(PropertyName = "bitmask")] public uint Bitmask { get; private set; }
        [JsonProperty(PropertyName = "containerI")] public MetaDumpContainerI Container { get; private set; }
        [JsonProperty(PropertyName = "hash")] public uint Hash { get; private set; }
        [JsonProperty(PropertyName = "mapI")] public MetaDumpMapI Map { get; private set; }
        [JsonProperty(PropertyName = "offset")] public uint Offset { get; private set; }
        [JsonProperty(PropertyName = "otherClass")] public uint OtherClass { get; private set; }
        [JsonProperty(PropertyName = "type")] public BinPropertyType Type { get; private set; }
    }

    public sealed class MetaDumpContainerI
    {
        [JsonProperty(PropertyName = "elemSize")] public uint ElementSize { get; private set; }
        [JsonProperty(PropertyName = "fixedSize")] public int FixedSize { get; private set; }
        [JsonProperty(PropertyName = "type")] public BinPropertyType Type { get; private set; }
        [JsonProperty(PropertyName = "vtable")] public uint VTable { get; private set; }
    }

    public sealed class MetaDumpMapI
    {
        [JsonProperty(PropertyName = "key")] public BinPropertyType KeyType { get; private set; }
        [JsonProperty(PropertyName = "storage")] public int Storage { get; private set; }
        [JsonProperty(PropertyName = "value")] public BinPropertyType ValueType { get; private set; }
        [JsonProperty(PropertyName = "vtable")] public uint VTable { get; private set; }
    }
}
