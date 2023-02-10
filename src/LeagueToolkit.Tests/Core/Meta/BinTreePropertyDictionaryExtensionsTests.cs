using LeagueToolkit.Core.Meta;
using LeagueToolkit.Core.Meta.Properties;

namespace LeagueToolkit.Tests.Core.Meta;

public class BinTreePropertyDictionaryExtensionsTests
{
    public class GetValueOrDefaultTests
    {
        [Fact]
        public void Should_Return_Value_If_It_Exists()
        {
            BinTreeString testProperty = new(0x111, "test");
            Dictionary<uint, BinTreeProperty> properties = new() { { testProperty.NameHash, testProperty } };

            BinTreeString value = properties.GetValueOrDefault<BinTreeString>(testProperty.NameHash);

            Assert.Equal(testProperty, value);
        }

        [Fact]
        public void Should_Return_Default_If_Key_Not_Found()
        {
            BinTreeString testProperty = new(0x111, "test");
            Dictionary<uint, BinTreeProperty> properties = new();

            BinTreeString value = properties.GetValueOrDefault<BinTreeString>(testProperty.NameHash);

            Assert.Equal(default, value);
        }
    }

    public class TryGetValueTests
    {
        [Fact]
        public void Should_Return_True_And_Concrete_Property_If_It_Exists()
        {
            BinTreeString testProperty = new(0x111, "test");
            Dictionary<uint, BinTreeProperty> properties = new() { { testProperty.NameHash, testProperty } };

            bool result = properties.TryGetValue(testProperty.NameHash, out BinTreeString value);

            Assert.True(result);
            Assert.Equal(testProperty, value);
        }

        [Fact]
        public void Should_Return_False_And_Default_Value_If_Key_Not_Found()
        {
            BinTreeString testProperty = new(0x111, "test");
            Dictionary<uint, BinTreeProperty> properties = new();

            bool result = properties.TryGetValue(testProperty.NameHash, out BinTreeString value);

            Assert.False(result);
            Assert.Equal(default, value);
        }

        [Fact]
        public void Should_Return_False_And_Default_Value_If_Key_Exists_But_Value_Is_Of_Different_Type()
        {
            BinTreeU32 testProperty = new(0x111, 0x12345u);
            Dictionary<uint, BinTreeProperty> properties = new() { { testProperty.NameHash, testProperty } };

            bool result = properties.TryGetValue(testProperty.NameHash, out BinTreeString value);

            Assert.False(result);
            Assert.Equal(default, value);
        }
    }
}
