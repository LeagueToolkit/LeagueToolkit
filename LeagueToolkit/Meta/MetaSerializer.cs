using LeagueToolkit.IO.PropertyBin;
using LeagueToolkit.IO.PropertyBin.Properties;
using LeagueToolkit.Meta.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
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

            // Registered the object in the environment for link resolving
            environment.RegisterObject(treeObject.PathHash, metaClassObject);

            return metaClassObject;
        }

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
    }
}
