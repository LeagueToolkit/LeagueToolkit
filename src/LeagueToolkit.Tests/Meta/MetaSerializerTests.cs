using LeagueToolkit.Core.Meta;
using LeagueToolkit.Core.Meta.Properties;
using LeagueToolkit.Hashing;
using LeagueToolkit.Meta;
using LeagueToolkit.Meta.Attributes;

namespace LeagueToolkit.Tests.Meta;

public class MetaSerializerTests
{
    private static readonly uint TEST_PROPERTY_HASH = Fnv1a.HashLower("test");

    public class Serialize
    {
        [Fact]
        public void Throws_InvalidOperationException_If_T_Does_Not_Have_MetaClass_Attribute()
        {
            var metaEnvironment = CreateMockMetaEnvironment();

            Assert.Throws<InvalidOperationException>(() =>
            {
                _ = MetaSerializer.Serialize(metaEnvironment, "test", new TestMetaClassWithoutAttribute());
            });
        }

        [Fact]
        public void Throws_InvalidOperationException_If_Property_Of_T_Does_Not_Have_MetaProperty_Attribute()
        {
            var metaEnvironment = CreateMockMetaEnvironment();

            Assert.Throws<InvalidOperationException>(() =>
            {
                _ = MetaSerializer.Serialize(metaEnvironment, "test", new TestMetaClassWithPropertyWithoutAttribute());
            });
        }

        [Fact]
        public void Serializes_A_Bool_Property()
        {
            var value = true;

            var metaEnvironment = CreateMockMetaEnvironment();
            BinTreeObject treeObject = MetaSerializer.Serialize(
                metaEnvironment,
                "test",
                new TestBoolMetaClass() { Test = value }
            );

            Assert.Equal(
                new BinTreeBool(TEST_PROPERTY_HASH, value),
                treeObject.Properties.GetValueOrDefault(TEST_PROPERTY_HASH)
            );
        }

        [Fact]
        public void Serializes_A_String_Property()
        {
            var value = "test";

            var metaEnvironment = CreateMockMetaEnvironment();
            BinTreeObject treeObject = MetaSerializer.Serialize(
                metaEnvironment,
                "test",
                new TestStringMetaClass() { Test = value }
            );

            Assert.Equal(
                new BinTreeString(TEST_PROPERTY_HASH, value),
                treeObject.Properties.GetValueOrDefault(TEST_PROPERTY_HASH)
            );
        }
    }

    internal static MetaEnvironment CreateMockMetaEnvironment() =>
        MetaEnvironment.Create(
            new[]
            {
                typeof(TestBoolMetaClass),
                typeof(TestStringMetaClass),
                typeof(TestMetaClassWithPropertyWithoutAttribute)
            }
        );

    [MetaClass("TestStringMetaClass")]
    private class TestStringMetaClass : IMetaClass
    {
        [MetaProperty("test", BinPropertyType.String, "", BinPropertyType.None, BinPropertyType.None)]
        public string Test { get; set; }
    }

    [MetaClass(nameof(TestBoolMetaClass))]
    private class TestBoolMetaClass : IMetaClass
    {
        [MetaProperty("test", BinPropertyType.Bool, "", BinPropertyType.None, BinPropertyType.None)]
        public bool Test { get; set; }
    }

    private class TestMetaClassWithoutAttribute : IMetaClass { }

    [MetaClass(nameof(TestMetaClassWithPropertyWithoutAttribute))]
    private class TestMetaClassWithPropertyWithoutAttribute : IMetaClass
    {
        public string Test { get; set; }
    }
}
