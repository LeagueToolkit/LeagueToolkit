using LeagueToolkit.Core.Meta;
using LeagueToolkit.Core.Meta.Properties;

namespace LeagueToolkit.Tests.Core.Meta;

public class BinTreeHashWidthTests
{
    // Property names whose low byte is not a property type, so a 4-byte read of a preceding 8-byte
    // hash fails instead of happening to fit (the ambiguity the width search cannot rule out)
    private const uint Name = 0x1111117F;
    private const uint Type = 0x2222227F;
    private const uint Samplers = 0x3333337F;
    private const uint TextureName = 0x4444447F;
    private const uint Other = 0x5555557F;

    private const ulong WideValue = 0x0123456789ABCDEFUL;

    private static (byte[] Bytes, BinTree Tree) RoundTrip(params BinTreeObject[] objects)
    {
        using MemoryStream written = new();
        new BinTree(objects, Array.Empty<string>()).Write(written);

        byte[] bytes = written.ToArray();
        return (bytes, new BinTree(new MemoryStream(bytes)));
    }

    private static byte[] Write(BinTree tree)
    {
        using MemoryStream stream = new();
        tree.Write(stream);
        return stream.ToArray();
    }

    [Fact]
    public void Should_Read_An_8_Byte_Hash_Followed_By_Other_Properties()
    {
        // The shape of a StaticMaterialDef since 16.21: an 8-byte name, then its other properties
        BinTreeObject material = new(
            0x1,
            0x2,
            new BinTreeProperty[]
            {
                new BinTreeHash(Name, WideValue),
                new BinTreeU8(Type, 0),
                new BinTreeUnorderedContainer(
                    Samplers,
                    BinPropertyType.Embedded,
                    new[] { new BinTreeEmbedded(0, 0x3, new[] { new BinTreeString(TextureName, "DiffuseTexture") }) }
                )
            }
        );

        var (bytes, tree) = RoundTrip(material);

        BinTreeObject read = tree.Objects[0x1];
        BinTreeHash name = Assert.IsType<BinTreeHash>(read.Properties[Name]);
        Assert.Equal(WideValue, name.WideValue);
        Assert.Equal(0x89ABCDEFu, name.Value);
        Assert.Equal(0, Assert.IsType<BinTreeU8>(read.Properties[Type]).Value);
        BinTreeEmbedded sampler = Assert.IsType<BinTreeEmbedded>(
            Assert.Single(Assert.IsType<BinTreeUnorderedContainer>(read.Properties[Samplers]).Elements)
        );
        Assert.Equal("DiffuseTexture", Assert.IsType<BinTreeString>(sampler.Properties[TextureName]).Value);

        Assert.Equal(bytes, Write(tree));
    }

    [Fact]
    public void Should_Read_8_Byte_Hashes_Inside_Structs_Optionals_Containers_And_Maps()
    {
        BinTreeObject treeObject = new(
            0x1,
            0x2,
            new BinTreeProperty[]
            {
                new BinTreeEmbedded(
                    Name,
                    0x3,
                    new BinTreeProperty[] { new BinTreeHash(Other, WideValue), new BinTreeU32(Type, 7) }
                ),
                new BinTreeOptional(Type, new BinTreeHash(0, WideValue)),
                new BinTreeContainer(
                    Samplers,
                    BinPropertyType.Hash,
                    new[] { new BinTreeHash(0, WideValue), new BinTreeHash(0, WideValue + 1) }
                ),
                new BinTreeMap(
                    TextureName,
                    BinPropertyType.Hash,
                    BinPropertyType.String,
                    new[]
                    {
                        new KeyValuePair<BinTreeProperty, BinTreeProperty>(
                            new BinTreeHash(0, WideValue),
                            new BinTreeString(0, "a")
                        )
                    }
                ),
                new BinTreeHash(Other, 0x1234u)
            }
        );

        var (bytes, tree) = RoundTrip(treeObject);
        BinTreeObject read = tree.Objects[0x1];

        BinTreeEmbedded embedded = Assert.IsType<BinTreeEmbedded>(read.Properties[Name]);
        Assert.Equal(WideValue, Assert.IsType<BinTreeHash>(embedded.Properties[Other]).WideValue);
        Assert.Equal(7u, Assert.IsType<BinTreeU32>(embedded.Properties[Type]).Value);

        Assert.Equal(WideValue, Assert.IsType<BinTreeHash>(Assert.IsType<BinTreeOptional>(read.Properties[Type]).Value).WideValue);

        Assert.Collection(
            Assert.IsType<BinTreeContainer>(read.Properties[Samplers]).Elements,
            x => Assert.Equal(WideValue, Assert.IsType<BinTreeHash>(x).WideValue),
            x => Assert.Equal(WideValue + 1, Assert.IsType<BinTreeHash>(x).WideValue)
        );

        var entry = Assert.Single(Assert.IsType<BinTreeMap>(read.Properties[TextureName]));
        Assert.Equal(WideValue, Assert.IsType<BinTreeHash>(entry.Key).WideValue);

        BinTreeHash narrow = Assert.IsType<BinTreeHash>(read.Properties[Other]);
        Assert.Null(narrow.WideValue);
        Assert.Equal(0x1234u, narrow.Value);

        Assert.Equal(bytes, Write(tree));
    }

    [Fact]
    public void Should_Read_Objects_Around_One_With_8_Byte_Hashes()
    {
        BinTreeObject before = new(0x1, 0x2, new BinTreeProperty[] { new BinTreeHash(Name, 0xAAu) });
        BinTreeObject wide = new(
            0x2,
            0x2,
            new BinTreeProperty[] { new BinTreeHash(Name, WideValue), new BinTreeString(Other, "x") }
        );
        BinTreeObject after = new(0x3, 0x2, new BinTreeProperty[] { new BinTreeString(Other, "y") });

        var (bytes, tree) = RoundTrip(before, wide, after);

        Assert.Equal(0xAAu, Assert.IsType<BinTreeHash>(tree.Objects[0x1].Properties[Name]).Value);
        Assert.Equal(WideValue, Assert.IsType<BinTreeHash>(tree.Objects[0x2].Properties[Name]).WideValue);
        Assert.Equal("y", Assert.IsType<BinTreeString>(tree.Objects[0x3].Properties[Other]).Value);
        Assert.Equal(bytes, Write(tree));
    }

    [Fact]
    public void Should_Read_4_Byte_Hashes_As_Before()
    {
        BinTreeObject treeObject = new(
            0x1,
            0x2,
            new BinTreeProperty[] { new BinTreeHash(Name, 0xDEADBEEFu), new BinTreeString(Other, "x") }
        );

        var (bytes, tree) = RoundTrip(treeObject);

        BinTreeHash hash = Assert.IsType<BinTreeHash>(tree.Objects[0x1].Properties[Name]);
        Assert.Equal(0xDEADBEEFu, hash.Value);
        Assert.Null(hash.WideValue);
        Assert.Equal(bytes, Write(tree));
    }

    [Fact]
    public void Setting_Value_Should_Keep_The_High_Bits_Of_An_8_Byte_Hash()
    {
        BinTreeHash hash = new(Name, WideValue) { Value = 0x11111111 };

        Assert.Equal(0x0123456711111111UL, hash.WideValue);
        Assert.Equal(0x11111111u, hash.Value);
    }
}
