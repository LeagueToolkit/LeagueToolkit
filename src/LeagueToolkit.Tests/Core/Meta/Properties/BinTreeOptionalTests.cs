using LeagueToolkit.Core.Meta;
using LeagueToolkit.Core.Meta.Properties;

namespace LeagueToolkit.Tests.Core.Meta.Properties;

public class BinTreeOptionalTests
{
    public class EqualsTests
    {
        [Fact]
        public void Should_Return_True_When_Both_Values_Are_Null()
        {
            BinTreeOptional first = new(1, null);
            BinTreeOptional second = new(1, null);

            Assert.True(first.Equals(second));
        }

        [Fact]
        public void Should_Return_False_When_Only_One_Value_Is_Null()
        {
            BinTreeOptional empty = new(1, null);
            BinTreeOptional populated = new(1, new BinTreeString(0, "value"));

            Assert.False(empty.Equals(populated));
            Assert.False(populated.Equals(empty));
        }

        [Fact]
        public void Should_Compare_Null_Optionals_Inside_Containers()
        {
            BinTreeOptional firstOptional = new(0, null);
            BinTreeOptional secondOptional = new(0, null);
            BinTreeContainer first = new(1, BinPropertyType.Optional, new[] { firstOptional });
            BinTreeContainer second = new(1, BinPropertyType.Optional, new[] { secondOptional });

            Assert.True(first.Equals(second));
        }

        [Fact]
        public void Should_Compare_Null_Optionals_Inside_Structs()
        {
            BinTreeStruct first = new(1, 2, new[] { new BinTreeOptional(3, null) });
            BinTreeStruct second = new(1, 2, new[] { new BinTreeOptional(3, null) });

            Assert.True(first.Equals(second));
        }
    }
}
