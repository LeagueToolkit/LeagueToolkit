using LeagueToolkit.Core.Meta;
using LeagueToolkit.Core.Meta.Properties;

namespace LeagueToolkit.Tests.Core.Meta.Properties;

public class BinTreeContainerTests
{
    public class ConstructorTests
    {
        [Fact]
        public void Should_Create_A_BinTreeContainer_With_The_Specified_Parameters()
        {
            BinTreeU8 firstElement = new(0, 1);
            BinTreeU8 secondElement = new(0, 2);
            BinTreeContainer container = new(0x12345678u, BinPropertyType.U8, new[] { firstElement, secondElement });

            Assert.Equal(BinPropertyType.Container, container.Type);
            Assert.Equal(BinPropertyType.U8, container.ElementType);
            Assert.Collection(
                container.Elements,
                x => Assert.Equal(firstElement, x),
                y => Assert.Equal(secondElement, y)
            );
        }

        [Fact]
        public void Should_Throw_If_Elements_Type_Does_Not_Match()
        {
            Assert.Throws<ArgumentException>(
                () => new BinTreeContainer(0x12345678u, BinPropertyType.U8, new[] { new BinTreeU32(0, 1) })
            );
        }
    }

    public class EqualsTests
    {
        [Fact]
        public void Should_Return_False_If_NameHash_Is_Different()
        {
            BinTreeContainer container1 = new(0x1, BinPropertyType.U8, Array.Empty<BinTreeU8>());
            BinTreeContainer container2 = new(0x2, BinPropertyType.U8, Array.Empty<BinTreeU8>());

            Assert.False(container1.Equals(container2));
        }

        [Fact]
        public void Should_Return_False_If_Other_Is_UnorderedContainer()
        {
            BinTreeContainer container1 = new(0x1, BinPropertyType.U8, Array.Empty<BinTreeU8>());
            BinTreeUnorderedContainer container2 = new(0x1, BinPropertyType.U8, Array.Empty<BinTreeU8>());

            Assert.False(container1.Equals(container2));
        }

        [Fact]
        public void Should_Return_False_If_Other_Is_Not_Container()
        {
            BinTreeContainer container1 = new(0x1, BinPropertyType.U8, Array.Empty<BinTreeU8>());
            BinTreeU32 other = new(0x1, 0);

            Assert.False(container1.Equals(other));
        }

        [Fact]
        public void Should_Return_True_If_Other_Has_Same_NameHash_And_Elements()
        {
            BinTreeU8 firstElement = new(0, 1);
            BinTreeU8 secondElement = new(0, 2);

            BinTreeContainer container1 = new(0x1, BinPropertyType.U8, new[] { firstElement, secondElement });
            BinTreeContainer container2 = new(0x1, BinPropertyType.U8, new[] { firstElement, secondElement });

            Assert.True(container1.Equals(container2));
        }
    }
}
