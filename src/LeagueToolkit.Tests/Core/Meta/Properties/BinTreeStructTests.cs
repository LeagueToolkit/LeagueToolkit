using LeagueToolkit.Core.Meta;
using LeagueToolkit.Core.Meta.Properties;

namespace LeagueToolkit.Tests.Core.Meta.Properties;

public class BinTreeStructTests
{
    public class ConstructorTests
    {
        [Fact]
        public void Should_Create_A_BinTreeStruct_With_The_Specified_Parameters()
        {
            BinTreeU8 property1 = new(0x111, 1);
            BinTreeU8 property2 = new(0x222, 2);
            BinTreeStruct structure = new(0x12345678u, 0xb00b1e5u, new[] { property1, property2 });

            Assert.Equal(BinPropertyType.Struct, structure.Type);
            Assert.Equal(0xb00b1e5u, structure.ClassHash);
            Assert.Collection(
                structure.Properties,
                x => Assert.Equal(new(property1.NameHash, property1), x),
                y => Assert.Equal(new(property2.NameHash, property2), y)
            );
        }
    }

    public class EqualsTests
    {
        [Fact]
        public void Should_Return_False_If_NameHash_Is_Different()
        {
            BinTreeStruct struct1 = new(0x1, 0xb00b1e5u, Array.Empty<BinTreeU8>());
            BinTreeStruct struct2 = new(0x2, 0xb00b1e5u, Array.Empty<BinTreeU8>());

            Assert.False(struct1.Equals(struct2));
        }

        [Fact]
        public void Should_Return_False_If_Other_Is_BinTreeEmbedded()
        {
            BinTreeStruct struct1 = new(0x1, 0xb00b1e5u, Array.Empty<BinTreeU8>());
            BinTreeEmbedded embedded = new(0x1, 0xb00b1e5u, Array.Empty<BinTreeU8>());

            Assert.False(struct1.Equals(embedded));
        }

        [Fact]
        public void Should_Return_False_If_Other_Is_Not_Struct()
        {
            BinTreeStruct struct1 = new(0x1, 0xb00b1e5u, Array.Empty<BinTreeU8>());
            BinTreeU32 other = new(0x1, 0);

            Assert.False(struct1.Equals(other));
        }

        [Fact]
        public void Should_Return_False_If_ClassHash_Is_Different()
        {
            BinTreeStruct struct1 = new(0x1, 0xb00b1e5u, Array.Empty<BinTreeU8>());
            BinTreeStruct struct2 = new(0x1, 0xdeadbeef, Array.Empty<BinTreeU8>());

            Assert.False(struct1.Equals(struct2));
        }

        [Fact]
        public void Should_Return_True_If_Other_Has_Same_NameHash_And_Elements()
        {
            BinTreeU8 firstElement = new(0x111, 1);
            BinTreeU8 secondElement = new(0x222, 1);

            BinTreeStruct struct1 = new(0x1, 0xb00b1e5u, new[] { firstElement, secondElement });
            BinTreeStruct struct2 = new(0x1, 0xb00b1e5u, new[] { firstElement, secondElement });

            Assert.True(struct1.Equals(struct2));
        }
    }
}
