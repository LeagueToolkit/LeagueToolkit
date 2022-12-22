using LeagueToolkit.Helpers.Extensions;
using LeagueToolkit.Helpers.Hashing;
using LeagueToolkit.Helpers.Structures;
using LeagueToolkit.IO.PropertyBin;
using LeagueToolkit.Meta.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace LeagueToolkit.Meta.Dump
{
    public sealed class MetaDump
    {
        public string Version { get; set; }
        public Dictionary<string, MetaDumpClass> Classes { get; set; }

        private const string META_CLASSES_NAMESPACE = $"{nameof(LeagueToolkit)}.{nameof(LeagueToolkit.Meta)}.Classes";

        public void WriteMetaClasses(
            string fileLocation,
            ICollection<string> classNames,
            ICollection<string> propertyNames
        )
        {
            WriteMetaClasses(File.Create(fileLocation), classNames, propertyNames);
        }

        public void WriteMetaClasses(
            string fileLocation,
            Dictionary<uint, string> classNames,
            Dictionary<uint, string> propertyNames
        )
        {
            WriteMetaClasses(File.Create(fileLocation), classNames, propertyNames);
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

        public void WriteMetaClasses(
            Stream stream,
            Dictionary<uint, string> classNames,
            Dictionary<uint, string> propertyNames
        )
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

        public void WriteMetaClassesRoslyn(string file, IEnumerable<string> classes, IEnumerable<string> properties)
        {
            // Create dictionaries from collections
            Dictionary<uint, string> classesMap = new();
            Dictionary<uint, string> propertiesMap = new();

            foreach (string className in classes)
            {
                classesMap.Add(Fnv1a.HashLower(className), className);
            }

            foreach (string propertyName in properties)
            {
                propertiesMap.Add(Fnv1a.HashLower(propertyName), propertyName);
            }

            WriteMetaClassesRoslyn(File.OpenWrite(file), classesMap, propertiesMap);
        }

        public void WriteMetaClassesRoslyn(
            Stream stream,
            Dictionary<uint, string> classes,
            Dictionary<uint, string> properties
        )
        {
            CompilationUnitSyntax compilationUnit = CompilationUnit();

            // Add required using directives
            compilationUnit = compilationUnit.WithUsings(new(TakeRequiredUsingDirectives(GetRequiredTypes())));

            // Create namespace
            WithNamespaceDeclaration(ref compilationUnit, classes, properties);

            //using AdhocWorkspace workspace = new();
            //SyntaxNode metaSyntax = Formatter.Format(compilationUnit, workspace);

            using StreamWriter syntaxWriter = new(stream);
            compilationUnit.NormalizeWhitespace(eol: "\n", elasticTrivia: true).WriteTo(syntaxWriter);
        }

        private void WithNamespaceDeclaration(
            ref CompilationUnitSyntax metaSyntax,
            Dictionary<uint, string> classes,
            Dictionary<uint, string> properties
        )
        {
            NamespaceDeclarationSyntax namespaceDeclaration = NamespaceDeclaration(
                ParseName(META_CLASSES_NAMESPACE, consumeFullText: true)
            );

            WithNamespaceMemberDeclarations(ref namespaceDeclaration, classes, properties);

            metaSyntax = metaSyntax.WithMembers(SingletonList<MemberDeclarationSyntax>(namespaceDeclaration));
        }

        private void WithNamespaceMemberDeclarations(
            ref NamespaceDeclarationSyntax namespaceSyntax,
            Dictionary<uint, string> classes,
            Dictionary<uint, string> properties
        )
        {
            namespaceSyntax = namespaceSyntax.WithMembers(new(TakeMetaClassDeclarations(classes, properties)));
        }

        private IEnumerable<TypeDeclarationSyntax> TakeMetaClassDeclarations(
            Dictionary<uint, string> classes,
            Dictionary<uint, string> properties
        )
        {
            foreach (var (classHash, @class) in this.Classes.Select(x => (x.Key, x.Value)))
            {
                yield return CreateMetaClassDeclaration(classHash, @class, classes, properties);
            }
        }

        private TypeDeclarationSyntax CreateMetaClassDeclaration(
            string classHash,
            MetaDumpClass @class,
            Dictionary<uint, string> classes,
            Dictionary<uint, string> properties
        )
        {
            // Get the name of the declaration
            string className = GetClassNameOrDefault(classHash, classes);

            // Create the declaration syntax
            TypeDeclarationSyntax metaClassDeclaration = @class.Is.Interface
                ? InterfaceDeclaration(className)
                : ClassDeclaration(className);

            // Add MetaClass attribute
            bool hasAttributeClassName = classes.TryGetValue(
                Convert.ToUInt32(classHash, 16),
                out string attributeClassName
            );
            metaClassDeclaration = metaClassDeclaration
                .WithAttributeLists(
                    SingletonList(
                        AttributeList(
                            SingletonSeparatedList(
                                Attribute(IdentifierName(nameof(MetaClassAttribute)))
                                    .WithArgumentList(
                                        AttributeArgumentList(
                                            SingletonSeparatedList(
                                                AttributeArgument(
                                                    LiteralExpression(
                                                        hasAttributeClassName
                                                            ? SyntaxKind.StringLiteralExpression
                                                            : SyntaxKind.NumericLiteralExpression,
                                                        hasAttributeClassName
                                                            ? Literal(attributeClassName)
                                                            : Literal(Convert.ToUInt32(classHash, 16))
                                                    )
                                                )
                                            )
                                        )
                                    )
                            )
                        )
                    )
                )
                .WithAdditionalAnnotations(Formatter.Annotation);

            // Add public modifier
            metaClassDeclaration = metaClassDeclaration.WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)));

            // Add base types
            List<string> interfaceHashes = @class.GetInterfaces(this.Classes, false);
            metaClassDeclaration = metaClassDeclaration.WithBaseList(
                BaseList(
                    SeparatedList<BaseTypeSyntax>(
                        interfaceHashes
                            .Select(
                                interfaceHash =>
                                    SimpleBaseType(IdentifierName(GetClassNameOrDefault(interfaceHash, classes)))
                            )
                            .Prepend(
                                SimpleBaseType(
                                    IdentifierName(
                                        @class.ParentClass is not null && this.Classes.ContainsKey(@class.ParentClass)
                                            ? GetClassNameOrDefault(@class.ParentClass, classes)
                                            : nameof(IMetaClass)
                                    )
                                )
                            )
                    )
                )
            );

            metaClassDeclaration = metaClassDeclaration.WithMembers(
                List<MemberDeclarationSyntax>(TakeMetaClassPropertyDeclarations(classHash, @class, classes, properties))
            );

            return metaClassDeclaration;
        }

        private IEnumerable<PropertyDeclarationSyntax> TakeMetaClassPropertyDeclarations(
            string classHash,
            MetaDumpClass @class,
            Dictionary<uint, string> classes,
            Dictionary<uint, string> properties
        )
        {
            // Write properties of interfaces
            if (@class.Is.Interface is false)
            {
                foreach (string interfaceHash in @class.GetInterfacesRecursive(this.Classes, true))
                {
                    if (
                        this.Classes.TryGetValue(interfaceHash, out MetaDumpClass interfaceClass)
                        && interfaceClass.Is.Interface
                    )
                    {
                        foreach (var property in interfaceClass.Properties)
                        {
                            yield return CreateMetaClassPropertyDeclaration(
                                interfaceHash,
                                @class,
                                property.Key,
                                property.Value,
                                true,
                                classes,
                                properties
                            );
                        }
                    }
                }
            }

            foreach (var property in @class.Properties)
            {
                yield return CreateMetaClassPropertyDeclaration(
                    classHash,
                    @class,
                    property.Key,
                    property.Value,
                    !@class.Is.Interface,
                    classes,
                    properties
                );
            }
        }

        private PropertyDeclarationSyntax CreateMetaClassPropertyDeclaration(
            string classHash,
            MetaDumpClass @class,
            string propertyHash,
            MetaDumpProperty property,
            bool isPublic,
            Dictionary<uint, string> classes,
            Dictionary<uint, string> properties
        )
        {
            TypeSyntax typeSyntax = CreatePropertyTypeSyntax(property, classes);
            string name = StylizePropertyName(GetPropertyNameOrDefault(propertyHash, properties));

            // Check that property name isn't the same as the class name
            string className = GetClassNameOrDefault(classHash, classes);
            if (className == name)
            {
                name = $"m{name}";
            }

            PropertyDeclarationSyntax propertyDeclaration = PropertyDeclaration(typeSyntax, name);

            // Add attribute
            propertyDeclaration = propertyDeclaration.WithAttributeLists(
                CreatePropertyAttributesSyntax(propertyHash, property, classes, properties)
            );

            // Add visibility
            if (isPublic)
            {
                propertyDeclaration = propertyDeclaration.WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)));
            }

            // Add accessor
            propertyDeclaration = propertyDeclaration.WithAccessorList(
                AccessorList(
                    List(
                        new AccessorDeclarationSyntax[]
                        {
                            AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                                .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
                            AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                                .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                        }
                    )
                )
            );

            // Add initializer
            if (@class.Is.Interface is false)
            {
                propertyDeclaration = propertyDeclaration
                    .WithInitializer(CreatePropertyInitializer(@class.Defaults[propertyHash], property, classes))
                    .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
            }

            return propertyDeclaration;
        }

        private SyntaxList<AttributeListSyntax> CreatePropertyAttributesSyntax(
            string propertyHash,
            MetaDumpProperty property,
            Dictionary<uint, string> classes,
            Dictionary<uint, string> properties
        )
        {
            bool hasName = properties.TryGetValue(Convert.ToUInt32(propertyHash, 16), out string name);

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

            string otherClass = property.OtherClass is null ? "" : GetClassNameOrDefault(property.OtherClass, classes);

            return SingletonList(
                AttributeList(
                    SingletonSeparatedList(
                        Attribute(
                            IdentifierName(nameof(MetaPropertyAttribute)),
                            AttributeArgumentList(
                                SeparatedList(
                                    new AttributeArgumentSyntax[]
                                    {
                                        AttributeArgument(
                                            LiteralExpression(
                                                hasName
                                                    ? SyntaxKind.StringLiteralExpression
                                                    : SyntaxKind.NumericLiteralExpression,
                                                hasName ? Literal(name) : Literal(Convert.ToUInt32(propertyHash, 16))
                                            )
                                        ),
                                        AttributeArgument(
                                            MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                IdentifierName(nameof(BinPropertyType)),
                                                IdentifierName(Enum.GetName(typeof(BinPropertyType), propertyType))
                                            )
                                        ),
                                        AttributeArgument(
                                            LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(otherClass))
                                        ),
                                        AttributeArgument(
                                            MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                IdentifierName(nameof(BinPropertyType)),
                                                IdentifierName(
                                                    Enum.GetName(
                                                        typeof(BinPropertyType),
                                                        primaryType ?? BinPropertyType.None
                                                    )
                                                )
                                            )
                                        ),
                                        AttributeArgument(
                                            MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                IdentifierName(nameof(BinPropertyType)),
                                                IdentifierName(
                                                    Enum.GetName(
                                                        typeof(BinPropertyType),
                                                        secondaryType ?? BinPropertyType.None
                                                    )
                                                )
                                            )
                                        )
                                    }
                                )
                            )
                        )
                    )
                )
            );
        }

        private TypeSyntax CreatePropertyTypeSyntax(MetaDumpProperty property, Dictionary<uint, string> classes)
        {
            return BinUtilities.UnpackType(property.Type) switch
            {
                BinPropertyType.Container
                    => GetContainerTypeDeclarationRoslyn(property.OtherClass, property.Container, false, classes),
                BinPropertyType.UnorderedContainer
                    => GetContainerTypeDeclarationRoslyn(property.OtherClass, property.Container, true, classes),
                BinPropertyType.Structure => GetStructureTypeDeclarationRoslyn(property.OtherClass, classes),
                BinPropertyType.Embedded => GetEmbeddedTypeDeclarationRoslyn(property.OtherClass, classes),
                BinPropertyType.Optional
                    => GetOptionalTypeDeclarationRoslyn(property.OtherClass, property.Container, classes),
                BinPropertyType.Map => GetMapTypeDeclarationRoslyn(property.OtherClass, property.Map, classes),
                BinPropertyType type => GetPrimitivePropertyTypeDeclarationRoslyn(type, false)
            };
        }

        private TypeSyntax GetPrimitivePropertyTypeDeclarationRoslyn(BinPropertyType type, bool nullable)
        {
            TypeSyntax typeDeclaration = BinUtilities.UnpackType(type) switch
            {
                BinPropertyType.Bool => PredefinedType(Token(SyntaxKind.BoolKeyword)),
                BinPropertyType.SByte => PredefinedType(Token(SyntaxKind.SByteKeyword)),
                BinPropertyType.Byte => PredefinedType(Token(SyntaxKind.ByteKeyword)),
                BinPropertyType.Int16 => PredefinedType(Token(SyntaxKind.ShortKeyword)),
                BinPropertyType.UInt16 => PredefinedType(Token(SyntaxKind.UShortKeyword)),
                BinPropertyType.Int32 => PredefinedType(Token(SyntaxKind.IntKeyword)),
                BinPropertyType.UInt32 => PredefinedType(Token(SyntaxKind.UIntKeyword)),
                BinPropertyType.Int64 => PredefinedType(Token(SyntaxKind.LongKeyword)),
                BinPropertyType.UInt64 => PredefinedType(Token(SyntaxKind.ULongKeyword)),
                BinPropertyType.Float => PredefinedType(Token(SyntaxKind.FloatKeyword)),
                BinPropertyType.Vector2 => ParseTypeName(nameof(Vector2)),
                BinPropertyType.Vector3 => ParseTypeName(nameof(Vector3)),
                BinPropertyType.Vector4 => ParseTypeName(nameof(Vector4)),
                BinPropertyType.Matrix44 => ParseTypeName(nameof(Matrix4x4)),
                BinPropertyType.Color => ParseTypeName(nameof(Color)),
                BinPropertyType.String => PredefinedType(Token(SyntaxKind.StringKeyword)),
                BinPropertyType.Hash => ParseTypeName(nameof(MetaHash)),
                BinPropertyType.WadEntryLink => ParseTypeName(nameof(MetaWadEntryLink)),
                BinPropertyType.ObjectLink => ParseTypeName(nameof(MetaObjectLink)),
                BinPropertyType.BitBool => ParseTypeName(nameof(MetaBitBool)),
                BinPropertyType propertyType
                    => throw new InvalidOperationException("Invalid Primitive Property type: " + propertyType)
            };

            return nullable switch
            {
                true => NullableType(typeDeclaration),
                false => typeDeclaration
            };
        }

        private GenericNameSyntax GetContainerTypeDeclarationRoslyn(
            string elementClass,
            MetaDumpContainer container,
            bool isUnorderedContainer,
            Dictionary<uint, string> classes
        )
        {
            TypeSyntax argumentTypeSyntax = BinUtilities.UnpackType(container.Type) switch
            {
                BinPropertyType.Structure => GetStructureTypeDeclarationRoslyn(elementClass, classes),
                BinPropertyType.Embedded => GetEmbeddedTypeDeclarationRoslyn(elementClass, classes),
                BinPropertyType type => GetPrimitivePropertyTypeDeclarationRoslyn(type, false)
            };

            SyntaxToken identifier = isUnorderedContainer
                ? Identifier(typeof(MetaUnorderedContainer<IMetaClass>).Name.Split('`')[0])
                : Identifier(typeof(MetaContainer<IMetaClass>).Name.Split('`')[0]);
            return GenericName(identifier, TypeArgumentList(SingletonSeparatedList(argumentTypeSyntax)));
        }

        private TypeSyntax GetStructureTypeDeclarationRoslyn(string classNameHash, Dictionary<uint, string> classes) =>
            ParseTypeName(GetClassNameOrDefault(classNameHash, classes));

        private GenericNameSyntax GetEmbeddedTypeDeclarationRoslyn(
            string classNameHash,
            Dictionary<uint, string> classes
        )
        {
            string argumentTypeIdentifier = GetClassNameOrDefault(classNameHash, classes);
            return GenericName(
                Identifier(typeof(MetaEmbedded<IMetaClass>).Name.Split('`')[0]),
                TypeArgumentList(SingletonSeparatedList(ParseTypeName(argumentTypeIdentifier, consumeFullText: true)))
            );
        }

        private GenericNameSyntax GetOptionalTypeDeclarationRoslyn(
            string otherClass,
            MetaDumpContainer container,
            Dictionary<uint, string> classes
        )
        {
            TypeSyntax argumentTypeSyntax = container.Type switch
            {
                BinPropertyType.Structure => GetStructureTypeDeclarationRoslyn(otherClass, classes),
                BinPropertyType.Embedded => GetEmbeddedTypeDeclarationRoslyn(otherClass, classes),
                BinPropertyType type => GetPrimitivePropertyTypeDeclarationRoslyn(type, false)
            };

            return GenericName(
                Identifier(typeof(MetaOptional<object>).Name.Split('`')[0]),
                TypeArgumentList(SingletonSeparatedList(argumentTypeSyntax))
            );
        }

        private GenericNameSyntax GetMapTypeDeclarationRoslyn(
            string otherClass,
            MetaDumpMap map,
            Dictionary<uint, string> classes
        )
        {
            TypeSyntax keyDeclaration = GetPrimitivePropertyTypeDeclarationRoslyn(
                BinUtilities.UnpackType(map.KeyType),
                false
            );
            TypeSyntax valueDeclaration = BinUtilities.UnpackType(map.ValueType) switch
            {
                BinPropertyType.Structure => GetStructureTypeDeclarationRoslyn(otherClass, classes),
                BinPropertyType.Embedded => GetEmbeddedTypeDeclarationRoslyn(otherClass, classes),
                BinPropertyType type => GetPrimitivePropertyTypeDeclarationRoslyn(type, false)
            };

            return GenericName(
                Identifier(typeof(Dictionary<object, object>).Name.Split('`')[0]),
                TypeArgumentList(SeparatedList(new TypeSyntax[] { keyDeclaration, valueDeclaration }))
            );
        }

        private void WriteClasses(
            StreamWriter sw,
            Dictionary<uint, string> classNames,
            Dictionary<uint, string> propertyNames
        )
        {
            foreach (var dumpClass in this.Classes)
            {
                WriteClass(sw, dumpClass.Key, dumpClass.Value, classNames, propertyNames);
            }
        }

        private void WriteClassAttribute(
            StreamWriter sw,
            string dumpClassHash,
            MetaDumpClass dumpClass,
            Dictionary<uint, string> classNames
        )
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

        private void WriteClass(
            StreamWriter sw,
            string classHash,
            MetaDumpClass dumpClass,
            Dictionary<uint, string> classNames,
            Dictionary<uint, string> propertyNames
        )
        {
            WriteClassAttribute(sw, classHash, dumpClass, classNames);

            sw.Write("    public ");
            sw.Write(dumpClass.Is.Interface ? "interface" : "class");
            sw.Write(" {0}", GetClassNameOrDefault(classHash, classNames));

            if (dumpClass.ParentClass is not null && this.Classes.ContainsKey(dumpClass.ParentClass))
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

        private void WriteClassProperties(
            StreamWriter sw,
            string classHash,
            MetaDumpClass dumpClass,
            Dictionary<uint, string> classNames,
            Dictionary<uint, string> propertyNames
        )
        {
            // Write properties of interfaces
            if (dumpClass.Is.Interface is false)
            {
                foreach (string interfaceHash in dumpClass.GetInterfacesRecursive(this.Classes, true))
                {
                    if (
                        this.Classes.TryGetValue(interfaceHash, out MetaDumpClass interfaceClass)
                        && interfaceClass.Is.Interface
                    )
                    {
                        foreach (var property in interfaceClass.Properties)
                        {
                            WriteProperty(
                                sw,
                                interfaceHash,
                                dumpClass,
                                property.Key,
                                property.Value,
                                true,
                                classNames,
                                propertyNames
                            );
                        }
                    }
                }
            }

            foreach (var property in dumpClass.Properties)
            {
                WriteProperty(
                    sw,
                    classHash,
                    dumpClass,
                    property.Key,
                    property.Value,
                    !dumpClass.Is.Interface,
                    classNames,
                    propertyNames
                );
            }
        }

        private void WriteProperty(
            StreamWriter sw,
            string classHash,
            MetaDumpClass dumpClass,
            string propertyHash,
            MetaDumpProperty property,
            bool isPublic,
            Dictionary<uint, string> classNames,
            Dictionary<uint, string> propertyNames
        )
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
            if (dumpClass.Is.Interface is false)
            {
                string defaultValue = GetPropertyDefaultValue(dumpClass.Defaults[propertyHash], property, classNames);
                propertyLine = $"{propertyLine} = {defaultValue};";
            }

            sw.WriteLineIndented(2, propertyLine);
        }

        private void WritePropertyAttribute(
            StreamWriter sw,
            string propertyHash,
            MetaDumpProperty property,
            Dictionary<uint, string> classNames,
            Dictionary<uint, string> propertyNames
        )
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

            string primaryTypeString = primaryType is null
                ? "BinPropertyType.None"
                : "BinPropertyType." + primaryType.ToString();
            string secondaryTypeString = secondaryType is null
                ? "BinPropertyType.None"
                : "BinPropertyType." + secondaryType.ToString();
            string otherClass = property.OtherClass is null
                ? ""
                : GetClassNameOrDefault(property.OtherClass, classNames);

            if (propertyNames.TryGetValue(Convert.ToUInt32(propertyHash, 16), out string propertyName))
            {
                sw.WriteLineIndented(
                    2,
                    $"[MetaProperty(\"{propertyName}\", BinPropertyType.{propertyType}, \"{otherClass}\", {primaryTypeString}, {secondaryTypeString})]"
                );
            }
            else
            {
                sw.WriteLineIndented(
                    2,
                    $"[MetaProperty({Convert.ToUInt32(propertyHash, 16)}, BinPropertyType.{propertyType}, \"{otherClass}\", {primaryTypeString}, {secondaryTypeString})]"
                );
            }
        }

        private string GetPropertyTypeDeclaration(MetaDumpProperty property, Dictionary<uint, string> classNames)
        {
            return BinUtilities.UnpackType(property.Type) switch
            {
                BinPropertyType.Container
                    => GetContainerTypeDeclaration(property.OtherClass, property.Container, false, classNames),
                BinPropertyType.UnorderedContainer
                    => GetContainerTypeDeclaration(property.OtherClass, property.Container, true, classNames),
                BinPropertyType.Structure => GetStructureTypeDeclaration(property.OtherClass, classNames),
                BinPropertyType.Embedded => GetEmbeddedTypeDeclaration(property.OtherClass, classNames),
                BinPropertyType.Optional
                    => GetOptionalTypeDeclaration(property.OtherClass, property.Container, classNames),
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
                BinPropertyType propertyType
                    => throw new InvalidOperationException("Invalid Primitive Property type: " + propertyType)
            };

            return nullable switch
            {
                true => typeDeclaration + "?",
                false => typeDeclaration
            };
        }

        private string GetContainerTypeDeclaration(
            string elementClass,
            MetaDumpContainer container,
            bool isUnorderedContainer,
            Dictionary<uint, string> classNames
        )
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

        private string GetOptionalTypeDeclaration(
            string otherClass,
            MetaDumpContainer container,
            Dictionary<uint, string> classNames
        )
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

        private string GetPropertyDefaultValue(
            object defaultValue,
            MetaDumpProperty property,
            Dictionary<uint, string> classNames
        )
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
                        JArray jArray
                            => property.Type switch
                            {
                                BinPropertyType.Vector2 => $"new Vector2({jArray[0]}f, {jArray[1]}f)",
                                BinPropertyType.Vector3 => $"new Vector3({jArray[0]}f, {jArray[1]}f, {jArray[2]}f)",
                                BinPropertyType.Vector4
                                    => $"new Vector4({jArray[0]}f, {jArray[1]}f, {jArray[2]}f, {jArray[3]}f)",
                                BinPropertyType.Color
                                    => $"new Color({jArray[0]}f, {jArray[1]}f, {jArray[2]}f, {jArray[3]}f)",
                                BinPropertyType.Matrix44
                                    => $"new R3DMatrix44({jArray[0][0]}f, {jArray[0][1]}f, {jArray[0][2]}f, {jArray[0][3]}f,"
                                        + $" {jArray[1][0]}f, {jArray[1][1]}f, {jArray[1][2]}f, {jArray[1][3]}f,"
                                        + $" {jArray[2][0]}f, {jArray[2][1]}f, {jArray[2][2]}f, {jArray[2][3]}f,"
                                        + $" {jArray[3][0]}f, {jArray[3][1]}f, {jArray[3][2]}f, {jArray[3][3]}f)",
                                BinPropertyType.Container => "new()",
                                BinPropertyType.UnorderedContainer => "new()",
                                _ => throw new NotImplementedException()
                            },
                        _ => throw new NotImplementedException()
                    };
                }
            }
        }

        /* -------------------------------- INITIALIZER CREATORS -------------------------------- */
        private EqualsValueClauseSyntax CreatePropertyInitializer(
            object defaultValue,
            MetaDumpProperty property,
            Dictionary<uint, string> classes
        )
        {
            ExpressionSyntax expression = defaultValue switch
            {
                // true | false
                bool boolValue when (property.Type == BinPropertyType.Bool)
                    => boolValue
                        ? LiteralExpression(SyntaxKind.TrueLiteralExpression)
                        : LiteralExpression(SyntaxKind.FalseLiteralExpression),
                // new(true) | new(false)
                bool boolValue when (property.Type == BinPropertyType.BitBool)
                    => boolValue
                        ? ImplicitObjectCreationExpression(
                            ArgumentList(
                                SingletonSeparatedList(Argument(LiteralExpression(SyntaxKind.TrueLiteralExpression)))
                            ),
                            null
                        )
                        : ImplicitObjectCreationExpression(
                            ArgumentList(
                                SingletonSeparatedList(Argument(LiteralExpression(SyntaxKind.FalseLiteralExpression)))
                            ),
                            null
                        ),
                // new(0U)
                string stringValue when (property.Type == BinPropertyType.Hash)
                    => ImplicitObjectCreationExpression(
                        ArgumentList(
                            SingletonSeparatedList(
                                Argument(
                                    LiteralExpression(
                                        SyntaxKind.NumericLiteralExpression,
                                        Literal(Convert.ToUInt32(stringValue, 16))
                                    )
                                )
                            )
                        ),
                        null
                    ),
                // new(0U)
                string stringValue when (property.Type == BinPropertyType.ObjectLink)
                    => ImplicitObjectCreationExpression(
                        ArgumentList(
                            SingletonSeparatedList(
                                Argument(
                                    LiteralExpression(
                                        SyntaxKind.NumericLiteralExpression,
                                        Literal(Convert.ToUInt32(stringValue, 16))
                                    )
                                )
                            )
                        ),
                        null
                    ),
                // new(0UL)
                string stringValue when (property.Type == BinPropertyType.WadEntryLink)
                    => ImplicitObjectCreationExpression(
                        ArgumentList(
                            SingletonSeparatedList(
                                Argument(
                                    LiteralExpression(
                                        SyntaxKind.NumericLiteralExpression,
                                        Literal(Convert.ToUInt64(stringValue, 16))
                                    )
                                )
                            )
                        ),
                        null
                    ),
                // new(new())
                JObject when (property.Type == BinPropertyType.Embedded)
                    => ImplicitObjectCreationExpression(
                        ArgumentList(SingletonSeparatedList(Argument(ImplicitObjectCreationExpression()))),
                        null
                    ),
                // null
                null when (property.Type == BinPropertyType.Structure)
                    => LiteralExpression(SyntaxKind.NullLiteralExpression),
                // new MetaOptional<T>(default(T), false)
                null when (property.Type == BinPropertyType.Optional)
                    => CreateNullableInitializerSyntax(property, null),
                // new MetaOptional<T>(nullableValue, true)
                object nullableValue when (property.Type == BinPropertyType.Optional)
                    => CreateNullableInitializerSyntax(property, nullableValue),
                _ => CreateCommonInitializerSyntax(property.Type, defaultValue)
            };

            return EqualsValueClause(expression);
        }

        private ExpressionSyntax CreateNullableInitializerSyntax(MetaDumpProperty property, object value)
        {
            TypeSyntax argumentTypeDeclaration = GetPrimitivePropertyTypeDeclarationRoslyn(
                property.Container.Type,
                false
            );

            // new MetaOptional<T>(default(T), false) | new MetaOptional<T>(nullableValue, true)
            return ObjectCreationExpression(
                GenericName(
                    Identifier(SanitizeNameOfGenericType(typeof(MetaOptional<object>))),
                    TypeArgumentList(SingletonSeparatedList(argumentTypeDeclaration))
                ),
                ArgumentList(
                    SeparatedList(
                        new ArgumentSyntax[]
                        {
                            Argument(
                                value is null
                                    ? DefaultExpression(argumentTypeDeclaration)
                                    : CreateCommonInitializerSyntax(property.Container.Type, value)
                            ),
                            Argument(
                                LiteralExpression(
                                    value is null ? SyntaxKind.FalseLiteralExpression : SyntaxKind.TrueLiteralExpression
                                )
                            )
                        }
                    )
                ),
                null
            );
        }

        private ExpressionSyntax CreateCommonInitializerSyntax(BinPropertyType valueType, object defaultValue)
        {
            return defaultValue switch
            {
                long longValue
                    => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal($"{longValue}", longValue)),
                string stringValue => LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(stringValue)),
                double doubleValue
                    => LiteralExpression(
                        SyntaxKind.NumericLiteralExpression,
                        Literal($"{doubleValue}f", (float)doubleValue)
                    ),
                JObject _ => ImplicitObjectCreationExpression(),
                JArray jArray
                    => valueType switch
                    {
                        BinPropertyType.Vector2 => CreateVector2InitializerSyntax(jArray),
                        BinPropertyType.Vector3 => CreateVector3InitializerSyntax(jArray),
                        BinPropertyType.Vector4 => CreateVector4InitializerSyntax(jArray),
                        BinPropertyType.Color => CreateColorInitializerSyntax(jArray),
                        BinPropertyType.Matrix44 => CreateMatrix4x4InitializerSyntax(jArray),
                        BinPropertyType.Container => ImplicitObjectCreationExpression(),
                        BinPropertyType.UnorderedContainer => ImplicitObjectCreationExpression(),
                        _ => throw new NotImplementedException()
                    },
                _ => throw new NotImplementedException()
            };
        }

        /* ----------------------- NUMERIC PRIMITIVE INITIALIZE CREATORS ----------------------- */

        private ExpressionSyntax CreateVector2InitializerSyntax(JArray jArray)
        {
            return ObjectCreationExpression(
                IdentifierName(nameof(Vector2)),
                ArgumentList(
                    SeparatedList(
                        new ArgumentSyntax[]
                        {
                            Argument(LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal((float)jArray[0]))),
                            Argument(LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal((float)jArray[1])))
                        }
                    )
                ),
                null
            );
        }

        private ExpressionSyntax CreateVector3InitializerSyntax(JArray jArray)
        {
            return ObjectCreationExpression(
                IdentifierName(nameof(Vector3)),
                ArgumentList(
                    SeparatedList(
                        new ArgumentSyntax[]
                        {
                            Argument(LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal((float)jArray[0]))),
                            Argument(LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal((float)jArray[1]))),
                            Argument(LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal((float)jArray[2])))
                        }
                    )
                ),
                null
            );
        }

        private ExpressionSyntax CreateVector4InitializerSyntax(JArray jArray)
        {
            return ObjectCreationExpression(
                IdentifierName(nameof(Vector4)),
                ArgumentList(
                    SeparatedList(
                        new ArgumentSyntax[]
                        {
                            Argument(LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal((float)jArray[0]))),
                            Argument(LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal((float)jArray[1]))),
                            Argument(LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal((float)jArray[2]))),
                            Argument(LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal((float)jArray[3])))
                        }
                    )
                ),
                null
            );
        }

        private ExpressionSyntax CreateColorInitializerSyntax(JArray jArray)
        {
            return ObjectCreationExpression(
                IdentifierName(nameof(Color)),
                ArgumentList(
                    SeparatedList(
                        new ArgumentSyntax[]
                        {
                            Argument(LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal((float)jArray[0]))),
                            Argument(LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal((float)jArray[1]))),
                            Argument(LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal((float)jArray[2]))),
                            Argument(LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal((float)jArray[3])))
                        }
                    )
                ),
                null
            );
        }

        private ExpressionSyntax CreateMatrix4x4InitializerSyntax(JArray jArray)
        {
            return ObjectCreationExpression(
                IdentifierName(nameof(Matrix4x4)),
                ArgumentList(
                    SeparatedList(
                        new ArgumentSyntax[]
                        {
                            Argument(
                                LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal((float)jArray[0][0]))
                            ),
                            Argument(
                                LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal((float)jArray[0][1]))
                            ),
                            Argument(
                                LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal((float)jArray[0][2]))
                            ),
                            Argument(
                                LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal((float)jArray[0][3]))
                            ),
                            Argument(
                                LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal((float)jArray[1][0]))
                            ),
                            Argument(
                                LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal((float)jArray[1][1]))
                            ),
                            Argument(
                                LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal((float)jArray[1][2]))
                            ),
                            Argument(
                                LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal((float)jArray[1][3]))
                            ),
                            Argument(
                                LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal((float)jArray[2][0]))
                            ),
                            Argument(
                                LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal((float)jArray[2][1]))
                            ),
                            Argument(
                                LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal((float)jArray[2][2]))
                            ),
                            Argument(
                                LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal((float)jArray[2][3]))
                            ),
                            Argument(
                                LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal((float)jArray[3][0]))
                            ),
                            Argument(
                                LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal((float)jArray[3][1]))
                            ),
                            Argument(
                                LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal((float)jArray[3][2]))
                            ),
                            Argument(
                                LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal((float)jArray[3][3]))
                            )
                        }
                    )
                ),
                null
            );
        }

        /* ----------------------- NUMERIC PRIMITIVE INITIALIZER CREATORS ----------------------- */
        /* -------------------------------- INITIALIZER CREATORS -------------------------------- */

        private string StylizePropertyName(string propertyName)
        {
            if (propertyName[0] == 'm' && char.IsUpper(propertyName[1]))
            {
                return propertyName[1..];
            }
            else if (char.IsLower(propertyName[0]) && !char.IsNumber(propertyName[1]))
            {
                return char.ToUpper(propertyName[0]) + propertyName[1..];
            }
            else
            {
                return propertyName;
            }
        }

        /* --------------------------- GENERIC TYPE NAME SANITIZATION --------------------------- */
        #region Generic Type Name Sanitization
        private string SanitizeNameOfGenericType(Type genericType) => SanitizeNameOfGenericType(genericType.Name);

        private string SanitizeNameOfGenericType(string nameOfGenericType) => nameOfGenericType.Split('`')[0];
        #endregion
        /* --------------------------- GENERIC TYPE NAME SANITIZATION --------------------------- */

        private List<Type> GetRequiredTypes() =>
            new()
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

        private List<string> GetRequiredNamespaces(List<Type> requiredTypes)
        {
            return requiredTypes.Select(x => x.Namespace).Distinct().ToList();
        }

        private IEnumerable<UsingDirectiveSyntax> TakeRequiredUsingDirectives(IEnumerable<Type> requiredTypes)
        {
            return requiredTypes
                .Select(x => x.Namespace)
                .Distinct()
                .OrderByDescending(x => x)
                .Select(requiredNamespace => UsingDirective(ParseName(requiredNamespace, consumeFullText: true)));
        }

        public static MetaDump Deserialize(string dump)
        {
            return JsonConvert.DeserializeObject<MetaDump>(dump);
        }
    }
}
