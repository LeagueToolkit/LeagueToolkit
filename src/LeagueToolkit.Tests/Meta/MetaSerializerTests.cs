using LeagueToolkit.Core.Meta;
using LeagueToolkit.Meta;
using LeagueToolkit.Meta.Attributes;

namespace LeagueToolkit.Tests.Meta;

public class MetaSerializerTests
{
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
    }

    internal static MetaEnvironment CreateMockMetaEnvironment() =>
        MetaEnvironment.Create(
            new[] { typeof(TestStringMetaClass), typeof(TestMetaClassWithPropertyWithoutAttribute) }
        );

    [MetaClass("TestStringMetaClass")]
    private class TestStringMetaClass : IMetaClass
    {
        [MetaProperty("test", BinPropertyType.String, null, BinPropertyType.None, BinPropertyType.None)]
        public string Test { get; set; }
    }

    private class TestMetaClassWithoutAttribute : IMetaClass { }

    [MetaClass(nameof(TestMetaClassWithPropertyWithoutAttribute))]
    private class TestMetaClassWithPropertyWithoutAttribute : IMetaClass
    {
        public string Test { get; set; }
    }
}
