using Fantome.Libraries.League.Helpers.Extensions;
using Fantome.Libraries.League.Helpers.Hashing;
using Fantome.Libraries.League.IO.PropertyBin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Fantome.Libraries.League.Meta.Dump
{
    public sealed class MetaDump
    {
        public string Version { get; set; }
        public List<MetaDumpClass> Classes { get; set; }

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
                sw.WriteLine("namespace Fantome.Libraries.League.Meta.Classes");
                sw.WriteLine("{");

                WriteClasses(sw, classNames, propertyNames);

                // End namespace
                sw.WriteLine("}");
                sw.WriteLine();
            }
        }

        private void WriteClasses(StreamWriter sw, Dictionary<uint, string> classNames, Dictionary<uint, string> propertyNames)
        {
            foreach (MetaDumpClass dumpClass in this.Classes)
            {
                WriteClass(sw, dumpClass, classNames, propertyNames);
            }
        }
        private void WriteClassAttribute(StreamWriter sw, MetaDumpClass dumpClass, Dictionary<uint, string> classNames)
        {
            if (classNames.TryGetValue(dumpClass.Hash, out string className))
            {
                sw.WriteLineIndented(1, @"[MetaClass(""{0}"")]", className);
            }
            else
            {
                sw.WriteLineIndented(1, "[MetaClass({0})]", dumpClass.Hash);
            }
        }
        private void WriteClass(StreamWriter sw, MetaDumpClass dumpClass, Dictionary<uint, string> classNames, Dictionary<uint, string> propertyNames)
        {
            WriteClassAttribute(sw, dumpClass, classNames);

            sw.Write("    public ");
            sw.Write(dumpClass.IsInterface ? "interface" : "class");
            sw.Write(" {0}", GetClassNameOrDefault(dumpClass.Hash, classNames));

            MetaDumpClass mainParent = this.Classes.FirstOrDefault(x => x.Hash == dumpClass.ParentClass);
            if (mainParent is null)
            {
                sw.Write(" : ");
                sw.Write("IMetaClass");
            }
            else
            {
                sw.Write(" : ");
                sw.Write(GetClassNameOrDefault(mainParent.Hash, classNames));
            }

            // Write interfaces
            List<uint> interfaces = dumpClass.GetInterfaces(this.Classes, false);
            if (interfaces.Count != 0)
            {
                sw.Write(", ");

                for (int i = 0; i < interfaces.Count; i++)
                {
                    uint interfaceHash = interfaces[i];
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

            WriteClassProperties(sw, dumpClass, classNames, propertyNames);

            // End members
            sw.WriteLineIndented(1, "}");
        }
        private void WriteClassProperties(StreamWriter sw, MetaDumpClass dumpClass, Dictionary<uint, string> classNames, Dictionary<uint, string> propertyNames)
        {
            // Write properties of interfaces
            if(dumpClass.IsInterface is false)
            {
                foreach (uint interfaceHash in dumpClass.GetInterfacesRecursive(this.Classes, true))
                {
                    if (this.Classes.FirstOrDefault(x => x.Hash == interfaceHash && x.IsInterface) is MetaDumpClass interaceClass)
                    {
                        foreach (MetaDumpProperty property in interaceClass.Properties)
                        {
                            WriteProperty(sw, interaceClass, property, true, classNames, propertyNames);
                        }
                    }
                }
            }

            foreach (MetaDumpProperty property in dumpClass.Properties)
            {
                WriteProperty(sw, dumpClass, property, !dumpClass.IsInterface, classNames, propertyNames);
            }
        }

        private void WriteProperty(StreamWriter sw, MetaDumpClass dumpClass, MetaDumpProperty property, bool isPublic, Dictionary<uint, string> classNames, Dictionary<uint, string> propertyNames)
        {
            WritePropertyAttribute(sw, property, propertyNames);

            string visibility = isPublic ? "public " : string.Empty;
            string typeDeclaration = GetPropertyTypeDeclaration(property, classNames);
            string propertyName = StylizePropertyName(GetPropertyNameOrDefault(property.Hash, propertyNames));

            // Check that property name isn't the same as the class name
            string className = GetClassNameOrDefault(dumpClass.Hash, classNames);
            if (className == propertyName)
            {
                propertyName = 'm' + propertyName;
            }

            string formatted = string.Format("{0}{1} {2}", visibility, typeDeclaration, propertyName);
            sw.WriteLineIndented(2, formatted + " { get; set; }");
        }
        private void WritePropertyAttribute(StreamWriter sw, MetaDumpProperty property, Dictionary<uint, string> propertyNames)
        {
            BinPropertyType propertyType = BinUtilities.UnpackType(property.Type);

            if (propertyNames.TryGetValue(property.Hash, out string propertyName))
            {
                sw.WriteLineIndented(2, @"[MetaProperty(""{0}"", BinPropertyType.{1})]", propertyName, propertyType);
            }
            else
            {
                sw.WriteLineIndented(2, @"[MetaProperty({0}, BinPropertyType.{1})]", property.Hash, propertyType);
            }
        }

        private string GetPropertyTypeDeclaration(MetaDumpProperty property, Dictionary<uint, string> classNames)
        {
            return BinUtilities.UnpackType(property.Type) switch
            {
                BinPropertyType.Container => GetContainerTypeDeclaration(property.OtherClass, property.Container, classNames),
                BinPropertyType.Container2 => GetContainerTypeDeclaration(property.OtherClass, property.Container, classNames),
                BinPropertyType.Structure => GetStructureTypeDeclaration(property.OtherClass, classNames),
                BinPropertyType.Embedded => GetStructureTypeDeclaration(property.OtherClass, classNames),
                BinPropertyType.Optional => GetOptionalTypeDeclaration(property.OtherClass, property.Container, classNames),
                BinPropertyType.Map => GetMapTypeDeclaration(property.OtherClass, property.Map, classNames),
                BinPropertyType type => GetPrimitivePropertyTypeDeclaration(type)
            };
        }
        private string GetPrimitivePropertyTypeDeclaration(BinPropertyType type)
        {
            return BinUtilities.UnpackType(type) switch
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
        }
        private string GetContainerTypeDeclaration(uint elementClass, MetaDumpContainerI container, Dictionary<uint, string> classNames)
        {
            string typeDeclarationFormat = "List<{0}>";
            string elementName = BinUtilities.UnpackType(container.Type) switch
            {
                BinPropertyType.Structure => GetStructureTypeDeclaration(elementClass, classNames),
                BinPropertyType.Embedded => GetStructureTypeDeclaration(elementClass, classNames),
                BinPropertyType type => GetPrimitivePropertyTypeDeclaration(type)
            };

            return string.Format(typeDeclarationFormat, elementName);
        }
        private string GetStructureTypeDeclaration(uint classNameHash, Dictionary<uint, string> classNames)
        {
            return GetClassNameOrDefault(classNameHash, classNames);
        }
        private string GetOptionalTypeDeclaration(uint otherClass, MetaDumpContainerI container, Dictionary<uint, string> classNames)
        {
            string optionalFormat = "{0}?";

            return container.Type switch
            {
                BinPropertyType.Structure => GetStructureTypeDeclaration(otherClass, classNames),
                BinPropertyType.Embedded => GetStructureTypeDeclaration(otherClass, classNames),
                BinPropertyType type => string.Format(optionalFormat, GetPrimitivePropertyTypeDeclaration(type))
            };
        }
        private string GetMapTypeDeclaration(uint otherClass, MetaDumpMapI map, Dictionary<uint, string> classNames)
        {
            string mapFormat = "Dictionary<{0}, {1}>";
            string keyDeclaration = GetPrimitivePropertyTypeDeclaration(BinUtilities.UnpackType(map.KeyType));
            string valueDeclaration = BinUtilities.UnpackType(map.ValueType) switch
            {
                BinPropertyType.Structure => GetStructureTypeDeclaration(otherClass, classNames),
                BinPropertyType.Embedded => GetStructureTypeDeclaration(otherClass, classNames),
                BinPropertyType type => GetPrimitivePropertyTypeDeclaration(type)
            };

            return string.Format(mapFormat, keyDeclaration, valueDeclaration);
        }

        private string GetClassNameOrDefault(uint hash, Dictionary<uint, string> classNames)
        {
            return classNames.GetValueOrDefault(hash, "Class" + hash);
        }
        private string GetPropertyNameOrDefault(uint hash, Dictionary<uint, string> propertyNames)
        {
            return propertyNames.GetValueOrDefault(hash, "m" + hash);
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
                typeof(Fantome.Libraries.League.Helpers.Structures.Color),
                typeof(Fantome.Libraries.League.Meta.MetaHash),
                typeof(Fantome.Libraries.League.Meta.MetaObjectLink),
                typeof(Fantome.Libraries.League.Meta.MetaWadEntryLink),
                typeof(Fantome.Libraries.League.Meta.MetaBitBool),

                typeof(System.Collections.Generic.List<>),
                typeof(System.Collections.Generic.Dictionary<,>),

                typeof(Fantome.Libraries.League.Meta.IMetaClass),
                typeof(Fantome.Libraries.League.Meta.Attributes.MetaClassAttribute),
                typeof(Fantome.Libraries.League.Meta.Attributes.MetaPropertyAttribute),
                typeof(Fantome.Libraries.League.IO.PropertyBin.BinPropertyType)
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
