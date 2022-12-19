using LeagueToolkit.IO.PropertyBin;
using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;

namespace LeagueToolkit.Meta.Dump
{
    public sealed class MetaDumpProperty
    {
        [JsonProperty(PropertyName = "other_class")] public string OtherClass { get; private set; }
        [JsonProperty(PropertyName = "offset")] public uint Offset { get; private set; }
        [JsonProperty(PropertyName = "bitmask")] public uint Bitmask { get; private set; }

        [JsonConverter(typeof(MetaDumpBinPropertyTypeJsonConverter))]
        [JsonProperty(PropertyName = "value_type")] public BinPropertyType Type { get; private set; }

        [JsonProperty(PropertyName = "container")] public MetaDumpContainer Container { get; private set; }
        [JsonProperty(PropertyName = "map")] public MetaDumpMap Map { get; private set; }
    }

    public sealed class MetaDumpContainer
    {
        [JsonProperty(PropertyName = "vtable")] public string VTable { get; private set; }

        [JsonConverter(typeof(MetaDumpBinPropertyTypeJsonConverter))]
        [JsonProperty(PropertyName = "value_type")] public BinPropertyType Type { get; private set; }

        [JsonProperty(PropertyName = "value_size")] public uint ValueSize { get; private set; }
        [JsonProperty(PropertyName = "fixed_size")] public int? FixedSize { get; private set; }

        [JsonConverter(typeof(MetaDumpContainerStorageTypeJsonConverter))]
        [JsonProperty(PropertyName = "storage")] public MetaDumpContainerStorageType? Storage { get; private set; }
    }

    public sealed class MetaDumpMap
    {
        [JsonProperty(PropertyName = "vtable")] public string VTable { get; private set; }
        
        [JsonConverter(typeof(MetaDumpBinPropertyTypeJsonConverter))]
        [JsonProperty(PropertyName = "key_type")] public BinPropertyType KeyType { get; private set; }

        [JsonConverter(typeof(MetaDumpBinPropertyTypeJsonConverter))]
        [JsonProperty(PropertyName = "value_type")] public BinPropertyType ValueType { get; private set; }

        [JsonConverter(typeof(MetaDumpMapStorageTypeJsonConverter))]
        [JsonProperty(PropertyName = "storage")] public MetaDumpMapStorageType Storage { get; private set; }
    }

    public class MetaDumpBinPropertyTypeJsonConverter : JsonConverter<BinPropertyType>
    {
        public override BinPropertyType ReadJson(JsonReader reader, Type objectType, [AllowNull] BinPropertyType existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string type = reader.Value as string;

            return type switch
            {
                "None" => BinPropertyType.None,
                "Bool" => BinPropertyType.Bool,
                "I8" => BinPropertyType.SByte,
                "U8" => BinPropertyType.Byte,
                "I16" => BinPropertyType.Int16,
                "U16" => BinPropertyType.UInt16,
                "I32" => BinPropertyType.Int32,
                "U32" => BinPropertyType.UInt32,
                "I64" => BinPropertyType.Int64,
                "U64" => BinPropertyType.UInt64,
                "F32" => BinPropertyType.Float,
                "Vec2" => BinPropertyType.Vector2,
                "Vec3" => BinPropertyType.Vector3,
                "Vec4" => BinPropertyType.Vector4,
                "Mtx44" => BinPropertyType.Matrix44,
                "Color" => BinPropertyType.Color,
                "String" => BinPropertyType.String,
                "Hash" => BinPropertyType.Hash,
                "File" => BinPropertyType.WadEntryLink,
                "List" => BinPropertyType.Container,
                "List2" => BinPropertyType.UnorderedContainer,
                "Pointer" => BinPropertyType.Structure,
                "Embed" => BinPropertyType.Embedded,
                "Link" => BinPropertyType.ObjectLink,
                "Option" => BinPropertyType.Optional,
                "Map" => BinPropertyType.Map,
                "Flag" => BinPropertyType.BitBool,
                _ => throw new NotImplementedException()
            };
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] BinPropertyType value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public class MetaDumpMapStorageTypeJsonConverter : JsonConverter<MetaDumpMapStorageType>
    {
        public override MetaDumpMapStorageType ReadJson(JsonReader reader, Type objectType, [AllowNull] MetaDumpMapStorageType existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string storage = reader.Value as string;
            return storage switch
            {
                "UnknownMap" => MetaDumpMapStorageType.UnknownMap,
                "StdMap" => MetaDumpMapStorageType.StdMap,
                "StdUnorderedMap" => MetaDumpMapStorageType.StdUnorderedMap,
                "RitoVectorMap" => MetaDumpMapStorageType.RiotVectorMap,
                _ => throw new NotImplementedException()
            };
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] MetaDumpMapStorageType value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public class MetaDumpContainerStorageTypeJsonConverter : JsonConverter<MetaDumpContainerStorageType?>
    {
        public override MetaDumpContainerStorageType? ReadJson(JsonReader reader, Type objectType, [AllowNull] MetaDumpContainerStorageType? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string storage = reader.Value as string;
            return storage switch
            {
                "UnknownVector" => MetaDumpContainerStorageType.UnknownVector,
                "Option" => MetaDumpContainerStorageType.Option,
                "Fixed" => MetaDumpContainerStorageType.Fixed,
                "StdVector" => MetaDumpContainerStorageType.StdVector,
                "RitoVector" => MetaDumpContainerStorageType.RiotVector,
                null => null,
                _ => throw new NotImplementedException()
            };
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] MetaDumpContainerStorageType? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public enum MetaDumpMapStorageType
    {
        UnknownMap,
        StdMap,
        StdUnorderedMap,
        RiotVectorMap
    }

    public enum MetaDumpContainerStorageType
    {
        UnknownVector,
        Option,
        Fixed,
        StdVector,
        RiotVector,
    }
}
