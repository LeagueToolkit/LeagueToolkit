using LeagueToolkit.Helpers.Hashing;
using LeagueToolkit.Helpers.Structures;
using LeagueToolkit.IO.PropertyBin;
using LeagueToolkit.IO.PropertyBin.Properties;
using LeagueToolkit.Meta.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;

namespace LeagueToolkit.Meta
{
    public static class MetaSerializer
    {
        public static T Deserialize<T>(MetaEnvironment environment, BinTreeObject treeObject)
            where T : IMetaClass
        {
            Type metaClassType = typeof(T);
            MetaClassAttribute metaClassAttribute = metaClassType.GetCustomAttribute(typeof(MetaClassAttribute)) as MetaClassAttribute;

            // Verify attribute
            if (metaClassAttribute is null) throw new InvalidOperationException("The provided MetaClass does not have a MetaClass Attribute");
            if (metaClassAttribute.NameHash != treeObject.MetaClassHash) throw new InvalidOperationException("Meta Class name does not match class name of treeObject");

            // Create an instance of T and get its runtime type
            T metaClassObject = Activator.CreateInstance<T>();
            Type metaClassObjectType = metaClassObject.GetType();

            // Assign values to the object properties
            AssignMetaClassProperties(environment, metaClassObject, metaClassObjectType, treeObject.Properties);

            // Register the object in the environment for link resolving
            environment.RegisterObject(treeObject.PathHash, metaClassObject);

            return metaClassObject;
        }
        public static BinTreeObject Serialize<T>(MetaEnvironment environment, string path, T metaClass)
            where T : IMetaClass
        {
            return Serialize(environment, Fnv1a.HashLower(path), metaClass);
        }
        public static BinTreeObject Serialize<T>(MetaEnvironment environment, uint pathHash, T metaClass)
            where T : IMetaClass
        {
            Type metaClassType = metaClass.GetType();
            MetaClassAttribute metaClassAttribute = metaClassType.GetCustomAttribute(typeof(MetaClassAttribute)) as MetaClassAttribute;

            if (metaClassAttribute is null) throw new InvalidOperationException("The provided MetaClass does not have a MetaClass Attribute");

            // Create Tree Properties for meta properties
            List<BinTreeProperty> properties = new();
            foreach (PropertyInfo propertyInfo in metaClassType.GetProperties())
            {
                BinTreeProperty treeProperty = ConvertPropertyToTreeProperty(environment, metaClass, propertyInfo);

                if (treeProperty is not null) properties.Add(treeProperty);
            }

            BinTreeObject treeObject = new BinTreeObject(metaClassAttribute.NameHash, pathHash, properties);

            return treeObject;
        }

        // ------------ DESERIALIZATION ASSIGNMENT ------------ \\
        private static void AssignMetaClassProperties(MetaEnvironment environment, object metaClassObject, Type metaClassType, ICollection<BinTreeProperty> treeProperties)
        {
            PropertyInfo[] properties = metaClassType.GetProperties();

            foreach (PropertyInfo propertyInfo in properties)
            {
                MetaPropertyAttribute metaPropertyAttribute = propertyInfo.GetCustomAttribute(typeof(MetaPropertyAttribute)) as MetaPropertyAttribute;

                // Ignore non-meta properties
                if (metaPropertyAttribute is null) continue;

                // Find matching tree property
                BinTreeProperty treeProperty = treeProperties.FirstOrDefault(x => x.NameHash == metaPropertyAttribute.NameHash);
                if (treeProperty is not null) // Ignore missing properties
                {
                    // Assign values to properties
                    AssignMetaProperty(environment, metaClassObject, propertyInfo, treeProperty);
                }
            }
        }
        private static void AssignMetaProperty(MetaEnvironment environment, object metaClassObject, PropertyInfo propertyInfo, BinTreeProperty treeProperty)
        {
            propertyInfo.SetValue(metaClassObject, DeserializeTreeProperty(environment, treeProperty, propertyInfo.PropertyType));
        }

        // ------------ PROPERTY DESERIALIZATION ------------ \\
        private static object DeserializeTreeProperty(MetaEnvironment environment, BinTreeProperty treeProperty, Type propertyType = null)
        {
            BinPropertyType treePropertyType = treeProperty.Type;

            if (IsPrimitivePropertyType(treePropertyType))
            {
                return FetchPrimitivePropertyValue(treeProperty);
            }
            else if (treePropertyType == BinPropertyType.Container || treePropertyType == BinPropertyType.UnorderedContainer)
            {
                return DeserializeContainer(environment, propertyType, treeProperty as BinTreeContainer);
            }
            else if (treePropertyType == BinPropertyType.Structure)
            {
                return DeserializeStructure(environment, treeProperty as BinTreeStructure);
            }
            else if (treePropertyType == BinPropertyType.Embedded)
            {
                return DeserializeEmbedded(environment, treeProperty as BinTreeEmbedded);
            }
            else if (treePropertyType == BinPropertyType.Map)
            {
                return DeserializeMap(environment, propertyType, treeProperty as BinTreeMap);
            }
            else if (treePropertyType == BinPropertyType.Optional)
            {
                return DeserializeOptional(environment, propertyType, treeProperty as BinTreeOptional);
            }

            return null;
        }
        private static object DeserializeStructure(MetaEnvironment environment, BinTreeStructure structure)
        {
            Type metaClassType = environment.FindMetaClass(structure.MetaClassHash);
            if (metaClassType is null) return null; // Couldn't deserialize structure

            object metaClassObject = Activator.CreateInstance(metaClassType);

            AssignMetaClassProperties(environment, metaClassObject, metaClassObject.GetType(), structure.Properties);

            return metaClassObject;
        }
        private static object DeserializeEmbedded(MetaEnvironment environment, BinTreeEmbedded embedded)
        {
            Type metaClassType = environment.FindMetaClass(embedded.MetaClassHash);
            if (metaClassType is null) return null; // Couldn't deserialize structure

            Type embeddedWrapperType = typeof(MetaEmbedded<>).MakeGenericType(metaClassType);

            object metaClassObject = Activator.CreateInstance(metaClassType);
            AssignMetaClassProperties(environment, metaClassObject, metaClassObject.GetType(), embedded.Properties);

            object embeddedWrapperObject = Activator.CreateInstance(embeddedWrapperType, new[] { metaClassObject });
            return embeddedWrapperObject;
        }
        private static object DeserializeContainer(MetaEnvironment environment, Type propertyType, BinTreeContainer container)
        {
            object containerList = Activator.CreateInstance(propertyType);
            Type containerListType = containerList.GetType();
            MethodInfo addMethod = containerListType.GetMethod("Add");

            foreach (BinTreeProperty containerItem in container.Properties)
            {
                addMethod.Invoke(containerList, new[] { DeserializeTreeProperty(environment, containerItem) });
            }

            return containerList;
        }
        private static object DeserializeMap(MetaEnvironment environment, Type propertyType, BinTreeMap map)
        {
            // Invalid key type
            if (IsValidMapKey(map.KeyType) is false) return null;

            object mapDictionary = Activator.CreateInstance(propertyType);
            Type mapDictionaryType = mapDictionary.GetType();
            MethodInfo addMethod = mapDictionaryType.GetMethod("Add");

            foreach (var propertyPair in map.Map)
            {
                // Key types can only be primitive so we can fetch their value easily
                object keyValue = FetchPrimitivePropertyValue(propertyPair.Key);
                object valueValue = DeserializeTreeProperty(environment, propertyPair.Value);

                addMethod.Invoke(mapDictionary, new[] { keyValue, valueValue });
            }

            return mapDictionary;
        }
        private static object DeserializeOptional(MetaEnvironment environment, Type propertyType, BinTreeOptional optional)
        {
            bool isSome = optional.Value is not null;
            object value = isSome ? DeserializeTreeProperty(environment, optional.Value) : GetTypeDefault(propertyType);
            object optionalObject = Activator.CreateInstance(propertyType, new[] { value, isSome });

            return optionalObject;
        }

        // ------------ SERIALIZATION ------------ \\
        private static BinTreeProperty ConvertPropertyToTreeProperty(MetaEnvironment environment, object metaClassObject, PropertyInfo propertyInfo)
        {
            MetaPropertyAttribute metaPropertyAttribute = propertyInfo.GetCustomAttribute(typeof(MetaPropertyAttribute)) as MetaPropertyAttribute;
            if (metaPropertyAttribute is null) throw new InvalidOperationException("The specified property does not have a MetaProperty Attribute");

            object value = propertyInfo.GetValue(metaClassObject);

            return ConvertObjectToProperty(environment, metaPropertyAttribute.NameHash, value, propertyInfo.PropertyType);
        }
        private static BinTreeProperty ConvertObjectToProperty(MetaEnvironment environment, uint nameHash, object value, Type valueType)
        {
            // Handle primitives
            if (value is null) return null;
            else if (valueType == typeof(bool?)) return new BinTreeBool(null, nameHash, (bool)value);
            else if (valueType == typeof(sbyte?)) return new BinTreeSByte(null, nameHash, (sbyte)value);
            else if (valueType == typeof(byte?)) return new BinTreeByte(null, nameHash, (byte)value);
            else if (valueType == typeof(short?)) return new BinTreeInt16(null, nameHash, (short)value);
            else if (valueType == typeof(ushort?)) return new BinTreeUInt16(null, nameHash, (ushort)value);
            else if (valueType == typeof(int?)) return new BinTreeInt32(null, nameHash, (int)value);
            else if (valueType == typeof(uint?)) return new BinTreeUInt32(null, nameHash, (uint)value);
            else if (valueType == typeof(long?)) return new BinTreeInt64(null, nameHash, (long)value);
            else if (valueType == typeof(ulong?)) return new BinTreeUInt64(null, nameHash, (ulong)value);
            else if (valueType == typeof(float?)) return new BinTreeFloat(null, nameHash, (float)value);
            else if (valueType == typeof(Vector2?)) return new BinTreeVector2(null, nameHash, (Vector2)value);
            else if (valueType == typeof(Vector3?)) return new BinTreeVector3(null, nameHash, (Vector3)value);
            else if (valueType == typeof(Vector4?)) return new BinTreeVector4(null, nameHash, (Vector4)value);
            else if (valueType == typeof(Matrix4x4?)) return new BinTreeMatrix44(null, nameHash, (Matrix4x4)value);
            else if (valueType == typeof(Color?)) return new BinTreeColor(null, nameHash, (Color)value);
            else if (valueType == typeof(string)) return new BinTreeString(null, nameHash, (string)value);
            else if (valueType == typeof(MetaHash?)) return new BinTreeHash(null, nameHash, (MetaHash)value);
            else if (valueType == typeof(MetaWadEntryLink?)) return new BinTreeWadEntryLink(null, nameHash, (MetaWadEntryLink)value);
            else if (valueType == typeof(MetaObjectLink?)) return new BinTreeObjectLink(null, nameHash, (MetaObjectLink)value);
            else if (valueType == typeof(MetaBitBool?)) return new BinTreeBitBool(null, nameHash, (MetaBitBool)value);
            else
            {
                // Handle complex types
                if (valueType.IsGenericType)
                {
                    Type genericTypeDefinition = valueType.GetGenericTypeDefinition();

                    if (genericTypeDefinition == typeof(Dictionary<,>))
                    {
                        return CreateMapProperty(environment, nameHash, valueType.GenericTypeArguments[0], valueType.GenericTypeArguments[1], value as IDictionary);
                    }
                    else if (genericTypeDefinition == typeof(MetaUnorderedContainer<>))
                    {
                        return CreateUnorderedContainerProperty(environment, nameHash, valueType.GenericTypeArguments[0], value as IEnumerable);
                    }
                    else if (genericTypeDefinition == typeof(MetaContainer<>))
                    {
                        return CreateContainerProperty(environment, nameHash, valueType.GenericTypeArguments[0], value as IEnumerable);
                    }
                    else if (genericTypeDefinition == typeof(MetaOptional<>))
                    {
                        return CreateOptionalProperty(environment, nameHash, valueType.GenericTypeArguments[0], value as IMetaOptional);
                    }
                    else if (genericTypeDefinition == typeof(MetaEmbedded<>))
                    {
                        return CreateEmbeddedProperty(environment, nameHash, valueType.GenericTypeArguments[0], value as IMetaEmbedded);
                    }
                    else return null;
                }
                else
                {
                    // Check if we're dealing with a Structure type
                    if (valueType.IsValueType is false && valueType.GetInterface(nameof(IMetaClass)) is not null)
                    {
                        return CreateStructureProperty(environment, value, nameHash);
                    }
                    else return null;
                }
            }
        }

        private static BinTreeStructure CreateStructureProperty(MetaEnvironment environment, object structureObject, uint nameHash)
        {
            Type structureType = structureObject.GetType();
            MetaClassAttribute metaClassAttribute = structureType.GetCustomAttribute(typeof(MetaClassAttribute)) as MetaClassAttribute;
            if (metaClassAttribute is null) throw new InvalidOperationException("The specified property does not have a MetaClass Attribute");

            // Create properties
            List<BinTreeProperty> properties = new();
            foreach (PropertyInfo propertyInfo in structureType.GetProperties())
            {
                BinTreeProperty property = ConvertPropertyToTreeProperty(environment, structureObject, propertyInfo);
                if (property is not null) properties.Add(property);
            }

            return new BinTreeStructure(null, nameHash, metaClassAttribute.NameHash, properties);
        }
        private static BinTreeMap CreateMapProperty(MetaEnvironment environment, uint nameHash, Type keyType, Type valueType, IDictionary map)
        {
            // Get key and value types
            BinPropertyType keyPropertyType = GetPropertyTypeFromType(keyType);
            BinPropertyType valuePropertyType = GetPropertyTypeFromType(valueType);

            // Create keys and values
            Dictionary<BinTreeProperty, BinTreeProperty> convertedMap = new();
            foreach (DictionaryEntry entry in map)
            {
                BinTreeProperty key = ConvertObjectToProperty(environment, 0, entry.Key, keyType);
                BinTreeProperty value = ConvertObjectToProperty(environment, 0, entry.Value, valueType);

                if (key is not null && value is not null)
                {
                    convertedMap.Add(key, value);
                }
            }

            BinTreeMap treeMap = new BinTreeMap(null, nameHash, keyPropertyType, valuePropertyType, convertedMap);
            return treeMap;
        }
        private static BinTreeUnorderedContainer CreateUnorderedContainerProperty(MetaEnvironment environment, uint nameHash, Type itemType, IEnumerable unorderedContainer)
        {
            BinPropertyType itemPropertyType = GetPropertyTypeFromType(itemType);

            // Create properties
            List<BinTreeProperty> properties = new();
            foreach (object item in unorderedContainer)
            {
                BinTreeProperty property = ConvertObjectToProperty(environment, 0, item, itemType);
                if (property is not null) properties.Add(property);
            }

            BinTreeUnorderedContainer treeContainer = new BinTreeUnorderedContainer(null, nameHash, itemPropertyType, properties);
            return treeContainer;
        }
        private static BinTreeContainer CreateContainerProperty(MetaEnvironment environment, uint nameHash, Type itemType, IEnumerable container)
        {
            BinPropertyType itemPropertyType = GetPropertyTypeFromType(itemType);

            // Create properties
            List<BinTreeProperty> properties = new();
            foreach (object item in container)
            {
                BinTreeProperty property = ConvertObjectToProperty(environment, 0, item, itemType);
                if (property is not null) properties.Add(property);
            }

            BinTreeContainer treeContainer = new BinTreeContainer(null, nameHash, itemPropertyType, properties);
            return treeContainer;
        }
        private static BinTreeOptional CreateOptionalProperty(MetaEnvironment environment, uint nameHash, Type valueType, IMetaOptional optional)
        {
            BinPropertyType propertyType = GetPropertyTypeFromType(valueType);
            object value = optional.GetValue();
            BinTreeProperty optionalValue = ConvertObjectToProperty(environment, 0, value, valueType);

            if (optionalValue is null) return null;
            else return new BinTreeOptional(null, nameHash, propertyType, optionalValue);
        }
        private static BinTreeEmbedded CreateEmbeddedProperty(MetaEnvironment environment, uint nameHash, Type valueType, IMetaEmbedded embeddedObject)
        {
            MetaClassAttribute metaClassAttribute = valueType.GetCustomAttribute(typeof(MetaClassAttribute)) as MetaClassAttribute;
            if (metaClassAttribute is null) throw new InvalidOperationException("The specified property does not have a MetaClass Attribute");

            object embdeddedValue = embeddedObject.GetValue();

            // Create properties
            List<BinTreeProperty> properties = new();
            foreach (PropertyInfo propertyInfo in valueType.GetProperties())
            {
                BinTreeProperty property = ConvertPropertyToTreeProperty(environment, embdeddedValue, propertyInfo);

                if (property is not null) properties.Add(property);
            }

            return new BinTreeEmbedded(null, nameHash, metaClassAttribute.NameHash, properties);
        }

        // ------------ HELPER METHODS ------------ \\
        private static bool IsPrimitivePropertyType(BinPropertyType propertyType)
        {
            return propertyType switch
            {
                BinPropertyType.None => true,
                BinPropertyType.Bool => true,
                BinPropertyType.SByte => true,
                BinPropertyType.Byte => true,
                BinPropertyType.Int16 => true,
                BinPropertyType.UInt16 => true,
                BinPropertyType.Int32 => true,
                BinPropertyType.UInt32 => true,
                BinPropertyType.Int64 => true,
                BinPropertyType.UInt64 => true,
                BinPropertyType.Float => true,
                BinPropertyType.Vector2 => true,
                BinPropertyType.Vector3 => true,
                BinPropertyType.Vector4 => true,
                BinPropertyType.Matrix44 => true,
                BinPropertyType.Color => true,
                BinPropertyType.String => true,
                BinPropertyType.Hash => true,
                BinPropertyType.WadEntryLink => true,
                BinPropertyType.Container => false,
                BinPropertyType.UnorderedContainer => false,
                BinPropertyType.Structure => false,
                BinPropertyType.Embedded => false,
                BinPropertyType.ObjectLink => true,
                BinPropertyType.Optional => false,
                BinPropertyType.Map => false,
                BinPropertyType.BitBool => true,
                _ => throw new ArgumentException("Invalid property type", nameof(propertyType))
            };
        }
        private static bool IsValidMapKey(BinPropertyType propertyType) => propertyType switch
        {
            BinPropertyType.None => false,
            BinPropertyType.Bool => false,
            BinPropertyType.SByte => true,
            BinPropertyType.Byte => true,
            BinPropertyType.Int16 => true,
            BinPropertyType.UInt16 => true,
            BinPropertyType.Int32 => true,
            BinPropertyType.UInt32 => true,
            BinPropertyType.Int64 => true,
            BinPropertyType.UInt64 => true,
            BinPropertyType.Float => false,
            BinPropertyType.Vector2 => false,
            BinPropertyType.Vector3 => false,
            BinPropertyType.Vector4 => false,
            BinPropertyType.Matrix44 => false,
            BinPropertyType.Color => false,
            BinPropertyType.String => true,
            BinPropertyType.Hash => true,
            BinPropertyType.WadEntryLink => false,
            BinPropertyType.Container => false,
            BinPropertyType.UnorderedContainer => false,
            BinPropertyType.Structure => false,
            BinPropertyType.Embedded => false,
            BinPropertyType.ObjectLink => false,
            BinPropertyType.Optional => false,
            BinPropertyType.Map => false,
            BinPropertyType.BitBool => false,
            _ => throw new ArgumentException("Invalid property type", nameof(propertyType))
        };

        private static object FetchPrimitivePropertyValue(BinTreeProperty primitiveProperty)
        {
            return primitiveProperty switch
            {
                BinTreeNone _ => null,
                BinTreeBool property => property.Value,
                BinTreeSByte property => property.Value,
                BinTreeByte property => property.Value,
                BinTreeInt16 property => property.Value,
                BinTreeUInt16 property => property.Value,
                BinTreeInt32 property => property.Value,
                BinTreeUInt32 property => property.Value,
                BinTreeInt64 property => property.Value,
                BinTreeUInt64 property => property.Value,
                BinTreeFloat property => property.Value,
                BinTreeVector2 property => property.Value,
                BinTreeVector3 property => property.Value,
                BinTreeVector4 property => property.Value,
                BinTreeMatrix44 property => property.Value,
                BinTreeColor property => property.Value,
                BinTreeString property => property.Value,
                BinTreeHash property => new MetaHash(property.Value),
                BinTreeWadEntryLink property => new MetaWadEntryLink(property.Value),
                BinTreeObjectLink property => new MetaObjectLink(property.Value),
                BinTreeBitBool property => new MetaBitBool(property.Value),
                _ => null
            };
        }
        private static object GetTypeDefault(Type type)
        {
            return type.IsValueType switch
            {
                true => Activator.CreateInstance(type),
                false => null
            };
        }
        private static BinPropertyType GetPropertyTypeFromType(Type type)
        {
            // Primitive types
            if (type == typeof(bool)) return BinPropertyType.Bool;
            else if (type == typeof(sbyte)) return BinPropertyType.SByte;
            else if (type == typeof(byte)) return BinPropertyType.Byte;
            else if (type == typeof(short)) return BinPropertyType.Int16;
            else if (type == typeof(ushort)) return BinPropertyType.UInt16;
            else if (type == typeof(int)) return BinPropertyType.Int32;
            else if (type == typeof(uint)) return BinPropertyType.UInt32;
            else if (type == typeof(long)) return BinPropertyType.Int64;
            else if (type == typeof(ulong)) return BinPropertyType.UInt64;
            else if (type == typeof(float)) return BinPropertyType.Float;
            else if (type == typeof(Vector2)) return BinPropertyType.Vector2;
            else if (type == typeof(Vector3)) return BinPropertyType.Vector3;
            else if (type == typeof(Vector4)) return BinPropertyType.Vector4;
            else if (type == typeof(Matrix4x4)) return BinPropertyType.Matrix44;
            else if (type == typeof(Color)) return BinPropertyType.Color;
            else if (type == typeof(string)) return BinPropertyType.String;
            else if (type == typeof(MetaHash)) return BinPropertyType.Hash;
            else if (type == typeof(MetaWadEntryLink)) return BinPropertyType.WadEntryLink;
            else if (type == typeof(MetaObjectLink)) return BinPropertyType.ObjectLink;
            else if (type == typeof(MetaBitBool)) return BinPropertyType.BitBool;
            else
            {
                if (type.IsGenericType)
                {
                    Type genericTypeDefinition = type.GetGenericTypeDefinition();

                    if (genericTypeDefinition == typeof(Dictionary<,>)) return BinPropertyType.Map;
                    else if (genericTypeDefinition == typeof(MetaUnorderedContainer<>)) return BinPropertyType.UnorderedContainer;
                    else if (genericTypeDefinition == typeof(MetaContainer<>)) return BinPropertyType.Container;
                    else if (genericTypeDefinition == typeof(MetaOptional<>)) return BinPropertyType.Optional;
                    else if (genericTypeDefinition == typeof(MetaEmbedded<>)) return BinPropertyType.Embedded;
                    else throw new ArgumentException(nameof(type), "Failed to match with a valid property type");
                }
                else if (type.IsValueType is false && type.GetInterface(nameof(IMetaClass)) is not null)
                {
                    return BinPropertyType.Structure;
                }
                else
                {
                    throw new ArgumentException(nameof(type), "Failed to match with a valid property type");
                }
            }
        }
    }
}
