using LeagueToolkit.Core.Meta;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LeagueToolkit.Meta.Dump
{
    public sealed class MetaDumpProperty
    {
        [JsonPropertyName("other_class")]
        [JsonInclude]
        public string OtherClass { get; private set; }

        [JsonInclude]
        public uint Offset { get; private set; }

        [JsonInclude]
        public uint Bitmask { get; private set; }

        [JsonConverter(typeof(MetaDumpBinPropertyTypeJsonConverter))]
        [JsonPropertyName("value_type")]
        [JsonInclude]
        public BinPropertyType Type { get; private set; }

        [JsonInclude]
        public MetaDumpContainer Container { get; private set; }

        [JsonInclude]
        public MetaDumpMap Map { get; private set; }
    }

    public sealed class MetaDumpContainer
    {
        [JsonInclude]
        public string VTable { get; private set; }

        [JsonConverter(typeof(MetaDumpBinPropertyTypeJsonConverter))]
        [JsonPropertyName("value_type")]
        [JsonInclude]
        public BinPropertyType Type { get; private set; }

        [JsonPropertyName("value_size")]
        [JsonInclude]
        public uint ValueSize { get; private set; }

        [JsonPropertyName("fixed_size")]
        [JsonInclude]
        public int? FixedSize { get; private set; }

        [JsonConverter(typeof(MetaDumpContainerStorageTypeJsonConverter))]
        [JsonInclude]
        public MetaDumpContainerStorageType? Storage { get; private set; }
    }

    public sealed class MetaDumpMap
    {
        [JsonInclude]
        public string VTable { get; private set; }

        [JsonConverter(typeof(MetaDumpBinPropertyTypeJsonConverter))]
        [JsonPropertyName("key_type")]
        [JsonInclude]
        public BinPropertyType KeyType { get; private set; }

        [JsonConverter(typeof(MetaDumpBinPropertyTypeJsonConverter))]
        [JsonPropertyName("value_type")]
        [JsonInclude]
        public BinPropertyType ValueType { get; private set; }

        [JsonConverter(typeof(MetaDumpMapStorageTypeJsonConverter))]
        [JsonInclude]
        public MetaDumpMapStorageType Storage { get; private set; }
    }

    public class MetaDumpBinPropertyTypeJsonConverter : JsonConverter<BinPropertyType>
    {
        public override BinPropertyType Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
        {
            string type = reader.GetString();

            return type switch
            {
                "None" => BinPropertyType.None,
                "Bool" => BinPropertyType.Bool,
                "I8" => BinPropertyType.I8,
                "U8" => BinPropertyType.U8,
                "I16" => BinPropertyType.I16,
                "U16" => BinPropertyType.U16,
                "I32" => BinPropertyType.I32,
                "U32" => BinPropertyType.U32,
                "I64" => BinPropertyType.I64,
                "U64" => BinPropertyType.U64,
                "F32" => BinPropertyType.F32,
                "Vec2" => BinPropertyType.Vector2,
                "Vec3" => BinPropertyType.Vector3,
                "Vec4" => BinPropertyType.Vector4,
                "Mtx44" => BinPropertyType.Matrix44,
                "Color" => BinPropertyType.Color,
                "String" => BinPropertyType.String,
                "Hash" => BinPropertyType.Hash,
                "File" => BinPropertyType.WadChunkLink,
                "List" => BinPropertyType.Container,
                "List2" => BinPropertyType.UnorderedContainer,
                "Pointer" => BinPropertyType.Struct,
                "Embed" => BinPropertyType.Embedded,
                "Link" => BinPropertyType.ObjectLink,
                "Option" => BinPropertyType.Optional,
                "Map" => BinPropertyType.Map,
                "Flag" => BinPropertyType.BitBool,
                _ => throw new NotImplementedException()
            };
        }

        public override void Write(Utf8JsonWriter writer, BinPropertyType value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }

    public class MetaDumpMapStorageTypeJsonConverter : JsonConverter<MetaDumpMapStorageType>
    {
        public override MetaDumpMapStorageType Read(
            ref Utf8JsonReader reader,
            Type objectType,
            JsonSerializerOptions options
        )
        {
            string storage = reader.GetString();
            return storage switch
            {
                "UnknownMap" => MetaDumpMapStorageType.UnknownMap,
                "StdMap" => MetaDumpMapStorageType.StdMap,
                "StdUnorderedMap" => MetaDumpMapStorageType.StdUnorderedMap,
                "RitoVectorMap" => MetaDumpMapStorageType.RiotVectorMap,
                _ => throw new NotImplementedException()
            };
        }

        public override void Write(
            Utf8JsonWriter writer,
            MetaDumpMapStorageType metaDumpMapStorageType,
            JsonSerializerOptions jsonSerializerOptions
        )
        {
            throw new NotImplementedException();
        }
    }

    public class MetaDumpContainerStorageTypeJsonConverter : JsonConverter<MetaDumpContainerStorageType?>
    {
        public override MetaDumpContainerStorageType? Read(
            ref Utf8JsonReader reader,
            Type objectType,
            JsonSerializerOptions options
        )
        {
            string storage = reader.GetString();
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

        public override void Write(
            Utf8JsonWriter writer,
            MetaDumpContainerStorageType? value,
            JsonSerializerOptions options
        )
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
