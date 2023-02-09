using LeagueToolkit.Core.Meta;
using LeagueToolkit.Core.Meta.Properties;
using LeagueToolkit.Hashing;

namespace LeagueToolkit.Tests.Core.Meta;

public class BinTreeObjectTests
{
    public class ConstructorTests
    {
        [Fact]
        public void Should_Create_A_BinTreeObject_With_The_Specified_Path_MetaClass_And_Properties()
        {
            BinTreeString property = new(0x1, "test");
            BinTreeObject treeObject = new("path", "class", new[] { property });

            Assert.Equal(Fnv1a.HashLower("path"), treeObject.PathHash);
            Assert.Equal(Fnv1a.HashLower("class"), treeObject.ClassHash);
            Assert.Collection(
                treeObject.Properties,
                x =>
                {
                    Assert.Equal(property.NameHash, x.Key);
                    Assert.Equal(property, x.Value);
                }
            );
        }
    }

    public class EqualsTests
    {
        [Fact]
        public void Should_Return_True_If_PathHash_ClassHash_And_Properties_Are_Equal()
        {
            BinTreeString property = new(0x1, "test");
            BinTreeObject treeObject1 = new("path", "class", new[] { property });
            BinTreeObject treeObject2 = new("path", "class", new[] { property });

            Assert.True(treeObject1.Equals(treeObject2));
        }

        [Fact]
        public void Should_Return_False_If_PathHash_Is_Not_Equal()
        {
            BinTreeString property = new(0x1, "test");
            BinTreeObject treeObject1 = new("path", "class", new[] { property });
            BinTreeObject treeObject2 = new("path1", "class", new[] { property });

            Assert.False(treeObject1.Equals(treeObject2));
        }

        [Fact]
        public void Should_Return_False_If_ClassHash_Is_Not_Equal()
        {
            BinTreeString property = new(0x1, "test");
            BinTreeObject treeObject1 = new("path", "class", new[] { property });
            BinTreeObject treeObject2 = new("path", "class1", new[] { property });

            Assert.False(treeObject1.Equals(treeObject2));
        }

        [Fact]
        public void Should_Return_False_If_Properties_Is_Not_Equal()
        {
            BinTreeString property = new(0x1, "test");
            BinTreeObject treeObject1 = new("path", "class", new[] { property });
            BinTreeObject treeObject2 = new("path", "class", new BinTreeString[] { });

            Assert.False(treeObject1.Equals(treeObject2));
        }
    }
}
