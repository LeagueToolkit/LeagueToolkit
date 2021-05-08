using LeagueToolkit.Helpers.Extensions;
using LeagueToolkit.Helpers.Hashing;
using LeagueToolkit.IO.PropertyBin;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LeagueToolkit.Meta.Dump
{
    public sealed class MetaDump
    {
        public string Version { get; set; }
        public Dictionary<string, MetaDumpClass> Classes { get; set; }

        public void WriteMetaClasses(string fileLocation, ICollection<string> classNames, ICollection<string> propertyNames)
        {
            WriteMetaClasses(File.OpenWrite(fileLocation), classNames, propertyNames);
        }
        public void WriteMetaClasses(string fileLocation, Dictionary<uint, string> classNames, Dictionary<uint, string> propertyNames)
        {
            WriteMetaClasses(File.OpenWrite(fileLocation), classNames, propertyNames);
        }
        public void WriteMetaClasses(Stream stream, ICollection<string> classNames, ICollection<string> propertyNames)
        {
            // Create dictionaries from collections
            Dictionary<uint, string> classNameMap = new(classNames.Count);
            Dictionary<uint, string> propertyNameMap = new(propertyNames.Count);

            foreach (string className in classNames)
            {
                classNameMap.Add(Fnv1a.HashLower(className), className);
            }

            foreach (string propertyName in propertyNames)
            {
                propertyNameMap.Add(Fnv1a.HashLower(propertyName), propertyName);
            }

            WriteMetaClasses(stream, classNameMap, propertyNameMap);
        }
        public void WriteMetaClasses(Stream stream, Dictionary<uint, string> classNames, Dictionary<uint, string> propertyNames)
        {
            using (StreamWriter sw = new StreamWriter(stream))
            {
                // Get required types and include their namespaces
                foreach (string requiredNamespace in GetRequiredNamespaces(GetRequiredTypes()))
                {
                    sw.WriteLine("using {0};", requiredNamespace);
                }

                // Start Namespace
                sw.WriteLine("namespace LeagueToolkit.Meta.Classes");
                sw.WriteLine("{");

                WriteClasses(sw, classNames, propertyNames);

                // End namespace
                sw.WriteLine("}");
                sw.WriteLine();
            }
        }

        private void WriteClasses(StreamWriter sw, Dictionary<uint, string> classNames, Dictionary<uint, string> propertyNames)
        {
            foreach (var dumpClass in this.Classes)
            {
                WriteClass(sw, dumpClass.Key, dumpClass.Value, classNames, propertyNames);
            }
        }
        private void WriteClassAttribute(StreamWriter sw, string dumpClassHash, MetaDumpClass dumpClass, Dictionary<uint, string> classNames)
        {
            if (classNames.TryGetValue(Convert.ToUInt32(dumpClassHash, 16), out string className))
            {
                sw.WriteLineIndented(1, @"[MetaClass(""{0}"")]", className);
            }
            else
            {
                sw.WriteLineIndented(1, "[MetaClass({0})]", Convert.ToUInt32(dumpClassHash, 16));
            }
        }
        private void WriteClass(StreamWriter sw, string classHash, MetaDumpClass dumpClass, Dictionary<uint, string> classNames, Dictionary<uint, string> propertyNames)
        {
            WriteClassAttribute(sw, classHash, dumpClass, classNames);

            sw.Write("    public ");
            sw.Write(dumpClass.Is.Interface ? "interface" : "class");
            sw.Write(" {0}", GetClassNameOrDefault(classHash, classNames));

            if(dumpClass.ParentClass is not null && this.Classes.ContainsKey(dumpClass.ParentClass))
            {
                sw.Write(" : ");
                sw.Write(GetClassNameOrDefault(dumpClass.ParentClass, classNames));
            }
            else
            {
                sw.Write(" : ");
                sw.Write("IMetaClass");
            }

            // Write interfaces
            List<string> interfaces = dumpClass.GetInterfaces(this.Classes, false);
            if (interfaces.Count != 0)
            {
                sw.Write(", ");

                for (int i = 0; i < interfaces.Count; i++)
                {
                    string interfaceHash = interfaces[i];
                    string interfaceName = GetClassNameOrDefault(interfaceHash, classNames);

                    sw.Write(" {0}", interfaceName);

                    if (i + 1 != dumpClass.Implements.Count)
                    {
                        sw.Write(',');
                    }
                }
            }

            // End declaration
            sw.WriteLine();

            // Start members
            sw.WriteLineIndented(1, "{");

            WriteClassProperties(sw, classHash, dumpClass, classNames, propertyNames);

            // End members
            sw.WriteLineIndented(1, "}");
        }
        private void WriteClassProperties(StreamWriter sw, string classHash, MetaDumpClass dumpClass, Dictionary<uint, string> classNames, Dictionary<uint, string> propertyNames)
        {
            // Write properties of interfaces
            if(dumpClass.Is.Interface is false)
            {
                foreach (string interfaceHash in dumpClass.GetInterfacesRecursive(this.Classes, true))
                {
                    if(this.Classes.TryGetValue(interfaceHash, out MetaDumpClass interfaceClass) && interfaceClass.Is.Interface)
                    {
                        foreach (var property in interfaceClass.Properties)
                        {
                            WriteProperty(sw, interfaceHash, dumpClass, property.Key, property.Value, true, classNames, propertyNames);
                        }
                    }
                }
            }

            foreach (var property in dumpClass.Properties)
            {
                WriteProperty(sw, classHash, dumpClass, property.Key, property.Value, !dumpClass.Is.Interface, classNames, propertyNames);
            }
        }

        private void WriteProperty(StreamWriter sw,
            string classHash, MetaDumpClass dumpClass, 
            string propertyHash, MetaDumpProperty property,
            bool isPublic, 
            Dictionary<uint, string> classNames, Dictionary<uint, string> propertyNames)
        {
            WritePropertyAttribute(sw, propertyHash, property, classNames, propertyNames);

            string visibility = isPublic ? "public " : string.Empty;
            string typeDeclaration = GetPropertyTypeDeclaration(property, classNames);
            string propertyName = StylizePropertyName(GetPropertyNameOrDefault(propertyHash, propertyNames));

            // Check that property name isn't the same as the class name
            string className = GetClassNameOrDefault(classHash, classNames);
            if (className == propertyName)
            {
                propertyName = 'm' + propertyName;
            }

            string propertyLine = $"{visibility}{typeDeclaration} {propertyName} {{ get; set; }}";
            if(dumpClass.Is.Interface is false)
            {
                string defaultValue = GetPropertyDefaultValue(dumpClass.Defaults[propertyHash], property, classNames);
                propertyLine = $"{propertyLine} = {defaultValue};";
            }

            sw.WriteLineIndented(2, propertyLine);
        }
        private void WritePropertyAttribute(StreamWriter sw, string propertyHash, MetaDumpProperty property, Dictionary<uint, string> classNames, Dictionary<uint, string> propertyNames)
        {
            BinPropertyType propertyType = BinUtilities.UnpackType(property.Type);
            BinPropertyType? primaryType = null;
            BinPropertyType? secondaryType = null;

            if (property.Map is not null)
            {
                primaryType = property.Map.KeyType;
                secondaryType = property.Map.ValueType;
            }
            else if (property.Container is not null)
            {
                primaryType = property.Container.Type;
            }

            string primaryTypeString = primaryType is null ? "BinPropertyType.None" : "BinPropertyType." + primaryType.ToString();
            string secondaryTypeString = secondaryType is null ? "BinPropertyType.None" : "BinPropertyType." + secondaryType.ToString();
            string otherClass = property.OtherClass is null ? "" : GetClassNameOrDefault(property.OtherClass, classNames);

            if (propertyNames.TryGetValue(Convert.ToUInt32(propertyHash, 16), out string propertyName))
            {
                sw.WriteLineIndented(2, $"[MetaProperty(\"{propertyName}\", BinPropertyType.{propertyType}, \"{otherClass}\", {primaryTypeString}, {secondaryTypeString})]");
            }
            else
            {
                sw.WriteLineIndented(2, $"[MetaProperty({Convert.ToUInt32(propertyHash, 16)}, BinPropertyType.{propertyType}, \"{otherClass}\", {primaryTypeString}, {secondaryTypeString})]");
            }
        }

        private string GetPropertyTypeDeclaration(MetaDumpProperty property, Dictionary<uint, string> classNames)
        {
            return BinUtilities.UnpackType(property.Type) switch
            {
                BinPropertyType.Container => GetContainerTypeDeclaration(property.OtherClass, property.Container, false, classNames),
                BinPropertyType.UnorderedContainer => GetContainerTypeDeclaration(property.OtherClass, property.Container, true, classNames),
                BinPropertyType.Structure => GetStructureTypeDeclaration(property.OtherClass, classNames),
                BinPropertyType.Embedded => GetEmbeddedTypeDeclaration(property.OtherClass, classNames),
                BinPropertyType.Optional => GetOptionalTypeDeclaration(property.OtherClass, property.Container, classNames),
                BinPropertyType.Map => GetMapTypeDeclaration(property.OtherClass, property.Map, classNames),
                BinPropertyType type => GetPrimitivePropertyTypeDeclaration(type, false)
            };
        }
        private string GetPrimitivePropertyTypeDeclaration(BinPropertyType type, bool nullable)
        {
            string typeDeclaration = BinUtilities.UnpackType(type) switch
            {
                BinPropertyType.Bool => "bool",
                BinPropertyType.SByte => "sbyte",
                BinPropertyType.Byte => "byte",
                BinPropertyType.Int16 => "short",
                BinPropertyType.UInt16 => "ushort",
                BinPropertyType.Int32 => "int",
                BinPropertyType.UInt32 => "uint",
                BinPropertyType.Int64 => "long",
                BinPropertyType.UInt64 => "ulong",
                BinPropertyType.Float => "float",
                BinPropertyType.Vector2 => "Vector2",
                BinPropertyType.Vector3 => "Vector3",
                BinPropertyType.Vector4 => "Vector4",
                BinPropertyType.Matrix44 => "R3DMatrix44",
                BinPropertyType.Color => "Color",
                BinPropertyType.String => "string",
                BinPropertyType.Hash => "MetaHash",
                BinPropertyType.WadEntryLink => "MetaWadEntryLink",
                BinPropertyType.ObjectLink => "MetaObjectLink",
                BinPropertyType.BitBool => "MetaBitBool",
                BinPropertyType propertyType => throw new InvalidOperationException("Invalid Primitive Property type: " + propertyType)
            };

            return nullable switch
            {
                true => typeDeclaration + "?",
                false => typeDeclaration
            };
        }
        private string GetContainerTypeDeclaration(string elementClass, MetaDumpContainer container, bool isUnorderedContainer, Dictionary<uint, string> classNames)
        {
            string typeDeclarationFormat = isUnorderedContainer ? "MetaUnorderedContainer<{0}>" : "MetaContainer<{0}>";
            string elementName = BinUtilities.UnpackType(container.Type) switch
            {
                BinPropertyType.Structure => GetStructureTypeDeclaration(elementClass, classNames),
                BinPropertyType.Embedded => GetEmbeddedTypeDeclaration(elementClass, classNames),
                BinPropertyType type => GetPrimitivePropertyTypeDeclaration(type, false)
            };

            return string.Format(typeDeclarationFormat, elementName);
        }
        private string GetStructureTypeDeclaration(string classNameHash, Dictionary<uint, string> classNames)
        {
            return GetClassNameOrDefault(classNameHash, classNames);
        }
        private string GetEmbeddedTypeDeclaration(string classNameHash, Dictionary<uint, string> classNames)
        {
            return string.Format("MetaEmbedded<{0}>", GetClassNameOrDefault(classNameHash, classNames));
        }
        private string GetOptionalTypeDeclaration(string otherClass, MetaDumpContainer container, Dictionary<uint, string> classNames)
        {
            string optionalFormat = "MetaOptional<{0}>";

            return container.Type switch
            {
                BinPropertyType.Structure => GetStructureTypeDeclaration(otherClass, classNames),
                BinPropertyType.Embedded => GetEmbeddedTypeDeclaration(otherClass, classNames),
                BinPropertyType type => string.Format(optionalFormat, GetPrimitivePropertyTypeDeclaration(type, false))
            };
        }
        private string GetMapTypeDeclaration(string otherClass, MetaDumpMap map, Dictionary<uint, string> classNames)
        {
            string mapFormat = "Dictionary<{0}, {1}>";
            string keyDeclaration = GetPrimitivePropertyTypeDeclaration(BinUtilities.UnpackType(map.KeyType), false);
            string valueDeclaration = BinUtilities.UnpackType(map.ValueType) switch
            {
                BinPropertyType.Structure => GetStructureTypeDeclaration(otherClass, classNames),
                BinPropertyType.Embedded => GetEmbeddedTypeDeclaration(otherClass, classNames),
                BinPropertyType type => GetPrimitivePropertyTypeDeclaration(type, false)
            };

            return string.Format(mapFormat, keyDeclaration, valueDeclaration);
        }

        private string GetClassNameOrDefault(string hash, Dictionary<uint, string> classNames)
        {
            return classNames.GetValueOrDefault(Convert.ToUInt32(hash, 16), "Class" + hash);
        }
        private string GetPropertyNameOrDefault(string hash, Dictionary<uint, string> propertyNames)
        {
            return propertyNames.GetValueOrDefault(Convert.ToUInt32(hash, 16), "m" + Convert.ToUInt32(hash, 16));
        }

        private string GetPropertyDefaultValue(object defaultValue, MetaDumpProperty property, Dictionary<uint, string> classNames)
        {
            switch (defaultValue)
            {
                case bool boolValue when (property.Type == BinPropertyType.Bool):
                {
                    return boolValue ? "true" : "false";
                }
                case bool boolValue when (property.Type == BinPropertyType.BitBool):
                {
                    return boolValue switch
                    {
                        true => "new (1)",
                        false => "new (0)",
                    };
                }
                case string stringValue when (property.Type == BinPropertyType.Hash):
                {
                    return $"new({Convert.ToUInt32(stringValue, 16)})";
                }
                case string stringValue when (property.Type == BinPropertyType.ObjectLink):
                {
                    return $"new({Convert.ToUInt32(stringValue, 16)})";
                }
                case string stringValue when (property.Type == BinPropertyType.WadEntryLink):
                {
                    return $"new({Convert.ToUInt64(stringValue, 16)})";
                }
                case JObject when (property.Type == BinPropertyType.Embedded):
                {
                    return $"new (new ())";
                }
                case null when (property.Type == BinPropertyType.Structure):
                {
                    return "null";
                }
                case object when property.Type == BinPropertyType.Optional:
                case null:
                {
                    string valueType = GetPrimitivePropertyTypeDeclaration(property.Container.Type, false);

                    return $"new MetaOptional<{valueType}>(default({valueType}), false)";
                }
                default:
                {
                    return defaultValue switch
                    {
                        long _ => defaultValue.ToString(),
                        string _ => $"\"{defaultValue}\"",
                        double _ => $"{defaultValue}f",
                        JObject _ => "new()",
                        JArray jArray => property.Type switch
                        {
                            BinPropertyType.Vector2 => $"new Vector2({jArray[0]}f, {jArray[1]}f)",
                            BinPropertyType.Vector3 => $"new Vector3({jArray[0]}f, {jArray[1]}f, {jArray[2]}f)",
                            BinPropertyType.Vector4 => $"new Vector4({jArray[0]}f, {jArray[1]}f, {jArray[2]}f, {jArray[3]}f)",
                            BinPropertyType.Color => $"new Color({jArray[0]}f, {jArray[1]}f, {jArray[2]}f, {jArray[3]}f)",
                            BinPropertyType.Matrix44 => $"new R3DMatrix44({jArray[0][0]}f, {jArray[0][1]}f, {jArray[0][2]}f, {jArray[0][3]}f," +
                            $" {jArray[1][0]}f, {jArray[1][1]}f, {jArray[1][2]}f, {jArray[1][3]}f," +
                            $" {jArray[2][0]}f, {jArray[2][1]}f, {jArray[2][2]}f, {jArray[2][3]}f," +
                            $" {jArray[3][0]}f, {jArray[3][1]}f, {jArray[3][2]}f, {jArray[3][3]}f)",
                            BinPropertyType.Container => "new()",
                            BinPropertyType.UnorderedContainer => "new()",
                            _ => throw new NotImplementedException()
                        },
                        _ => throw new NotImplementedException()
                    };
                }
            }
        }

        private string StylizePropertyName(string propertyName)
        {
            if (propertyName[0] == 'm' && char.IsUpper(propertyName[1]))
            {
                return propertyName.Substring(1);
            }
            else if (char.IsLower(propertyName[0]) && !char.IsNumber(propertyName[1]))
            {
                return char.ToUpper(propertyName[0]) + propertyName.Substring(1);
            }
            else
            {
                return propertyName;
            }
        }

        private List<Type> GetRequiredTypes()
        {
            return new List<Type>()
            {
                typeof(System.Numerics.Vector2),
                typeof(System.Numerics.Vector3),
                typeof(System.Numerics.Vector4),
                typeof(System.Numerics.Matrix4x4),
                typeof(LeagueToolkit.Helpers.Structures.Color),
                typeof(LeagueToolkit.Meta.MetaHash),
                typeof(LeagueToolkit.Meta.MetaObjectLink),
                typeof(LeagueToolkit.Meta.MetaWadEntryLink),
                typeof(LeagueToolkit.Meta.MetaBitBool),
                typeof(LeagueToolkit.Meta.MetaOptional<>),
                typeof(LeagueToolkit.Meta.MetaContainer<>),
                typeof(LeagueToolkit.Meta.MetaUnorderedContainer<>),
                typeof(LeagueToolkit.Meta.MetaEmbedded<>),

                typeof(System.Collections.Generic.Dictionary<,>),

                typeof(LeagueToolkit.Meta.IMetaClass),
                typeof(LeagueToolkit.Meta.Attributes.MetaClassAttribute),
                typeof(LeagueToolkit.Meta.Attributes.MetaPropertyAttribute),
                typeof(LeagueToolkit.IO.PropertyBin.BinPropertyType)
            };
        }
        private List<string> GetRequiredNamespaces(List<Type> requiredTypes)
        {
            return requiredTypes
                .Select(x => x.Namespace)
                .Distinct()
                .ToList();
        }

        public static MetaDump Deserialize(string dump)
        {
            return JsonConvert.DeserializeObject<MetaDump>(dump);
        }
    }
}
