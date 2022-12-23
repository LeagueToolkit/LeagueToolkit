using LeagueToolkit.Helpers.Hashing;
using LeagueToolkit.Helpers.Structures;
using LeagueToolkit.IO.PropertyBin;
using LeagueToolkit.Meta.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

        /// <value>
        /// The name of the namespace declaration which contains the meta class declarations
        /// </value>
        private const string META_CLASSES_NAMESPACE = $"{nameof(LeagueToolkit)}.{nameof(LeagueToolkit.Meta)}.Classes";

        /* -------------------------------- PUBLIC DUMPING API -------------------------------- */
        #region Public Dumping API
        public void WriteMetaClasses(string file, IEnumerable<string> classes, IEnumerable<string> properties) =>
            WriteMetaClasses(File.OpenWrite(file), classes, properties);

        public void WriteMetaClasses(Stream stream, IEnumerable<string> classes, IEnumerable<string> properties)
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

            WriteMetaClasses(stream, classesMap, propertiesMap);
        }

        public void WriteMetaClasses(
            string file,
            IReadOnlyDictionary<uint, string> classes,
            IReadOnlyDictionary<uint, string> properties
        ) => WriteMetaClasses(File.OpenWrite(file), classes, properties);

        public void WriteMetaClasses(
            Stream stream,
            IReadOnlyDictionary<uint, string> classes,
            IReadOnlyDictionary<uint, string> properties
        )
        {
            CompilationUnitSyntax compilationUnit = CompilationUnit();

            // Create compilation unit
            compilationUnit = compilationUnit
                .WithUsings(new(TakeRequiredUsingDirectives(GetRequiredTypes())))
                .WithMembers(
                    SingletonList<MemberDeclarationSyntax>(
                        NamespaceDeclaration(ParseName(META_CLASSES_NAMESPACE, consumeFullText: true))
                            .WithMembers(new(TakeMetaClassDeclarations(classes, properties)))
                    )
                );

            //using AdhocWorkspace workspace = new();
            //SyntaxNode metaSyntax = Formatter.Format(compilationUnit, workspace);

            using StreamWriter syntaxWriter = new(stream);
            compilationUnit.NormalizeWhitespace(eol: "\n", elasticTrivia: true).WriteTo(syntaxWriter);
        }
        #endregion
        /* -------------------------------- PUBLIC DUMPING API -------------------------------- */

        private IEnumerable<TypeDeclarationSyntax> TakeMetaClassDeclarations(
            IReadOnlyDictionary<uint, string> classes,
            IReadOnlyDictionary<uint, string> properties
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
            IReadOnlyDictionary<uint, string> classes,
            IReadOnlyDictionary<uint, string> properties
        )
        {
            // Get the name of the declaration
            string className = GetClassNameOrDefault(classHash, classes);

            // Create the declaration syntax
            TypeDeclarationSyntax metaClassDeclaration = @class.Is.Interface
                ? InterfaceDeclaration(className)
                : ClassDeclaration(className);

            return metaClassDeclaration
                // Add attribute
                .WithAttributeLists(SingletonList(CreateMetaClassAttributeList(classHash, classes)))
                // Add visibility modifier
                .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                // Add base types
                .WithBaseList(CreateMetaClassBaseList(@class, classes))
                // Add members
                .WithMembers(
                    List<MemberDeclarationSyntax>(
                        TakeMetaClassPropertyDeclarations(classHash, @class, classes, properties)
                    )
                );
        }

        private AttributeListSyntax CreateMetaClassAttributeList(
            string classHash,
            IReadOnlyDictionary<uint, string> classes
        )
        {
            bool hasAttributeClassName = classes.TryGetValue(
                Convert.ToUInt32(classHash, 16),
                out string attributeClassName
            );

            return AttributeList(
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
            );
        }

        private BaseListSyntax CreateMetaClassBaseList(MetaDumpClass @class, IReadOnlyDictionary<uint, string> classes)
        {
            return BaseList(
                SeparatedList<BaseTypeSyntax>(
                    @class
                        .TakeInterfaces(this.Classes, false)
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
            );
        }

        private IEnumerable<PropertyDeclarationSyntax> TakeMetaClassPropertyDeclarations(
            string classHash,
            MetaDumpClass @class,
            IReadOnlyDictionary<uint, string> classes,
            IReadOnlyDictionary<uint, string> properties
        )
        {
            // TODO: Cleanup
            // Create properties of inherited members
            if (@class.Is.Interface is false)
            {
                foreach (string interfaceHash in @class.TakeInterfacesRecursive(this.Classes, true))
                {
                    if (
                        this.Classes.TryGetValue(interfaceHash, out MetaDumpClass interfaceClass)
                        && interfaceClass.Is.Interface
                    )
                    {
                        foreach (var (hash, property) in interfaceClass.Properties.Select(x => (x.Key, x.Value)))
                        {
                            yield return CreateMetaClassPropertyDeclaration(
                                interfaceHash,
                                @class,
                                hash,
                                property,
                                true,
                                classes,
                                properties
                            );
                        }
                    }
                }
            }

            // Create properties of non-inherited members
            foreach (var (hash, property) in @class.Properties.Select(x => (x.Key, x.Value)))
            {
                yield return CreateMetaClassPropertyDeclaration(
                    classHash,
                    @class,
                    hash,
                    property,
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
            IReadOnlyDictionary<uint, string> classes,
            IReadOnlyDictionary<uint, string> properties
        )
        {
            TypeSyntax typeSyntax = CreatePropertyTypeDeclaration(property, classes);
            string name = StylizePropertyName(GetPropertyNameOrDefault(propertyHash, properties));

            // Check that property name isn't the same as the class name
            string className = GetClassNameOrDefault(classHash, classes);
            if (className == name)
            {
                name = $"m{name}";
            }

            PropertyDeclarationSyntax propertyDeclaration = PropertyDeclaration(typeSyntax, name)
                // Add attribute
                .WithAttributeLists(CreatePropertyAttributesSyntax(propertyHash, property, classes, properties))
                // Add accessor
                .WithAccessorList(
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

            // Add visibility
            if (isPublic)
            {
                propertyDeclaration = propertyDeclaration.WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)));
            }

            // Add initializer
            if (@class.Is.Interface is false)
            {
                propertyDeclaration = propertyDeclaration
                    .WithInitializer(CreatePropertyInitializer(@class.Defaults[propertyHash], property, classes))
                    .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
            }

            return propertyDeclaration;
        }

        /* ---------------------------- PROPERTY ATTRIBUTE CREATION ----------------------------- */
        #region Property Attribute Creation
        private SyntaxList<AttributeListSyntax> CreatePropertyAttributesSyntax(
            string propertyHash,
            MetaDumpProperty property,
            IReadOnlyDictionary<uint, string> classes,
            IReadOnlyDictionary<uint, string> properties
        )
        {
            return SingletonList(
                AttributeList(
                    SingletonSeparatedList(
                        Attribute(
                            IdentifierName(nameof(MetaPropertyAttribute)),
                            AttributeArgumentList(
                                SeparatedList(
                                    new AttributeArgumentSyntax[]
                                    {
                                        CreatePropertyNameAttributeArgument(propertyHash, properties),
                                        CreatePropertyTypeAttributeArgument(property),
                                        CreatePropertyOtherClassAttributeArgument(property, classes),
                                        CreatePropertyPrimaryTypeAttributeArgument(property),
                                        CreatePropertySecondaryTypeAttributeArgument(property)
                                    }
                                )
                            )
                        )
                    )
                )
            );
        }

        private AttributeArgumentSyntax CreatePropertyNameAttributeArgument(
            string propertyHash,
            IReadOnlyDictionary<uint, string> properties
        )
        {
            bool hasName = properties.TryGetValue(Convert.ToUInt32(propertyHash, 16), out string name);

            return AttributeArgument(
                LiteralExpression(
                    hasName ? SyntaxKind.StringLiteralExpression : SyntaxKind.NumericLiteralExpression,
                    hasName ? Literal(name) : Literal(Convert.ToUInt32(propertyHash, 16))
                )
            );
        }

        private AttributeArgumentSyntax CreatePropertyTypeAttributeArgument(MetaDumpProperty property)
        {
            BinPropertyType propertyType = BinUtilities.UnpackType(property.Type);

            return AttributeArgument(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName(nameof(BinPropertyType)),
                    IdentifierName(Enum.GetName(typeof(BinPropertyType), propertyType))
                )
            );
        }

        private AttributeArgumentSyntax CreatePropertyOtherClassAttributeArgument(
            MetaDumpProperty property,
            IReadOnlyDictionary<uint, string> classes
        )
        {
            string otherClass = property.OtherClass is null ? "" : GetClassNameOrDefault(property.OtherClass, classes);

            return AttributeArgument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(otherClass)));
        }

        private AttributeArgumentSyntax CreatePropertyPrimaryTypeAttributeArgument(MetaDumpProperty property)
        {
            BinPropertyType secondaryType = property switch
            {
                MetaDumpProperty { Map: not null } notNullMapProperty => notNullMapProperty.Map.KeyType,
                MetaDumpProperty { Container: not null } notNullContainerProperty
                    => notNullContainerProperty.Container.Type,
                _ => BinPropertyType.None
            };

            return AttributeArgument(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName(nameof(BinPropertyType)),
                    IdentifierName(Enum.GetName(typeof(BinPropertyType), secondaryType))
                )
            );
        }

        private AttributeArgumentSyntax CreatePropertySecondaryTypeAttributeArgument(MetaDumpProperty property)
        {
            BinPropertyType primaryType = property switch
            {
                MetaDumpProperty { Map: not null } notNullMapProperty => notNullMapProperty.Map.ValueType,
                _ => BinPropertyType.None
            };

            return AttributeArgument(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName(nameof(BinPropertyType)),
                    IdentifierName(Enum.GetName(typeof(BinPropertyType), primaryType))
                )
            );
        }

        #endregion
        /* ---------------------------- PROPERTY ATTRIBUTE CREATION ----------------------------- */

        /* ------------------------- PROPERTY TYPE DECLARATION CREATORS ------------------------- */
        #region Property Type Declaration Creators
        private TypeSyntax CreatePropertyTypeDeclaration(
            MetaDumpProperty property,
            IReadOnlyDictionary<uint, string> classes
        )
        {
            return BinUtilities.UnpackType(property.Type) switch
            {
                BinPropertyType.Container
                    => CreateContainerTypeDeclaration(property.OtherClass, property.Container, false, classes),
                BinPropertyType.UnorderedContainer
                    => CreateContainerTypeDeclaration(property.OtherClass, property.Container, true, classes),
                BinPropertyType.Structure => CreateStructureTypeDeclaration(property.OtherClass, classes),
                BinPropertyType.Embedded => CreateEmbeddedTypeDeclaration(property.OtherClass, classes),
                BinPropertyType.Optional
                    => CreateOptionalTypeDeclaration(property.OtherClass, property.Container, classes),
                BinPropertyType.Map => CreateMapTypeDeclaration(property.OtherClass, property.Map, classes),
                BinPropertyType type => CreatePrimitivePropertyTypeDeclaration(type, false)
            };
        }

        private TypeSyntax CreatePrimitivePropertyTypeDeclaration(BinPropertyType type, bool nullable)
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

        private GenericNameSyntax CreateContainerTypeDeclaration(
            string elementClass,
            MetaDumpContainer container,
            bool isUnorderedContainer,
            IReadOnlyDictionary<uint, string> classes
        )
        {
            TypeSyntax argumentTypeSyntax = BinUtilities.UnpackType(container.Type) switch
            {
                BinPropertyType.Structure => CreateStructureTypeDeclaration(elementClass, classes),
                BinPropertyType.Embedded => CreateEmbeddedTypeDeclaration(elementClass, classes),
                BinPropertyType type => CreatePrimitivePropertyTypeDeclaration(type, false)
            };

            SyntaxToken identifier = isUnorderedContainer
                ? Identifier(typeof(MetaUnorderedContainer<IMetaClass>).Name.Split('`')[0])
                : Identifier(typeof(MetaContainer<IMetaClass>).Name.Split('`')[0]);
            return GenericName(identifier, TypeArgumentList(SingletonSeparatedList(argumentTypeSyntax)));
        }

        private TypeSyntax CreateStructureTypeDeclaration(
            string classNameHash,
            IReadOnlyDictionary<uint, string> classes
        ) => ParseTypeName(GetClassNameOrDefault(classNameHash, classes));

        private GenericNameSyntax CreateEmbeddedTypeDeclaration(
            string classNameHash,
            IReadOnlyDictionary<uint, string> classes
        )
        {
            string argumentTypeIdentifier = GetClassNameOrDefault(classNameHash, classes);
            return GenericName(
                Identifier(typeof(MetaEmbedded<IMetaClass>).Name.Split('`')[0]),
                TypeArgumentList(SingletonSeparatedList(ParseTypeName(argumentTypeIdentifier, consumeFullText: true)))
            );
        }

        private GenericNameSyntax CreateOptionalTypeDeclaration(
            string otherClass,
            MetaDumpContainer container,
            IReadOnlyDictionary<uint, string> classes
        )
        {
            TypeSyntax argumentTypeSyntax = container.Type switch
            {
                BinPropertyType.Structure => CreateStructureTypeDeclaration(otherClass, classes),
                BinPropertyType.Embedded => CreateEmbeddedTypeDeclaration(otherClass, classes),
                BinPropertyType type => CreatePrimitivePropertyTypeDeclaration(type, false)
            };

            return GenericName(
                Identifier(typeof(MetaOptional<object>).Name.Split('`')[0]),
                TypeArgumentList(SingletonSeparatedList(argumentTypeSyntax))
            );
        }

        private GenericNameSyntax CreateMapTypeDeclaration(
            string otherClass,
            MetaDumpMap map,
            IReadOnlyDictionary<uint, string> classes
        )
        {
            TypeSyntax keyDeclaration = CreatePrimitivePropertyTypeDeclaration(
                BinUtilities.UnpackType(map.KeyType),
                false
            );
            TypeSyntax valueDeclaration = BinUtilities.UnpackType(map.ValueType) switch
            {
                BinPropertyType.Structure => CreateStructureTypeDeclaration(otherClass, classes),
                BinPropertyType.Embedded => CreateEmbeddedTypeDeclaration(otherClass, classes),
                BinPropertyType type => CreatePrimitivePropertyTypeDeclaration(type, false)
            };

            return GenericName(
                Identifier(typeof(Dictionary<object, object>).Name.Split('`')[0]),
                TypeArgumentList(SeparatedList(new TypeSyntax[] { keyDeclaration, valueDeclaration }))
            );
        }
        #endregion
        /* ------------------------- PROPERTY TYPE DECLARATION CREATORS ------------------------- */

        /* ----------------------------------- NAME UTILITIES ----------------------------------- */
        #region Name Utilities
        private string GetClassNameOrDefault(string hash, IReadOnlyDictionary<uint, string> classNames)
        {
            return classNames.GetValueOrDefault(Convert.ToUInt32(hash, 16), "Class" + hash);
        }

        private string GetPropertyNameOrDefault(string hash, IReadOnlyDictionary<uint, string> propertyNames)
        {
            return propertyNames.GetValueOrDefault(Convert.ToUInt32(hash, 16), "m" + Convert.ToUInt32(hash, 16));
        }

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
        #endregion
        /* ----------------------------------- NAME UTILITIES ----------------------------------- */

        /* -------------------------------- INITIALIZER CREATORS -------------------------------- */
        #region Initializer Creators
        private EqualsValueClauseSyntax CreatePropertyInitializer(
            object defaultValue,
            MetaDumpProperty property,
            IReadOnlyDictionary<uint, string> classes
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
            TypeSyntax argumentTypeDeclaration = CreatePrimitivePropertyTypeDeclaration(property.Container.Type, false);

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

        /* ----------------------- NUMERIC PRIMITIVE INITIALIZER CREATORS ----------------------- */
        #region Numeric Primitive Initializer Creators
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
        #endregion
        /* ----------------------- NUMERIC PRIMITIVE INITIALIZER CREATORS ----------------------- */
        #endregion
        /* -------------------------------- INITIALIZER CREATORS -------------------------------- */

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

        public static MetaDump Deserialize(string dump) => JsonConvert.DeserializeObject<MetaDump>(dump);
    }
}
