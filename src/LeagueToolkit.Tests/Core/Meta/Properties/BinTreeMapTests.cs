using LeagueToolkit.Core.Meta;
using LeagueToolkit.Core.Meta.Properties;

namespace LeagueToolkit.Tests.Core.Meta.Properties;

public class BinTreeMapTests
{
    public class ConstructorTests
    {
        [Fact]
        public void Should_Create_A_BinTreeMap_With_The_Specified_Parameters()
        {
            KeyValuePair<BinTreeProperty, BinTreeProperty> element =
                new(new BinTreeHash(0, 0x111), new BinTreeString(0, "hi"));
            BinTreeMap map = new(0, BinPropertyType.Hash, BinPropertyType.String, new[] { element });

            Assert.Equal(BinPropertyType.Map, map.Type);
            Assert.Equal(BinPropertyType.Hash, map.KeyType);
            Assert.Equal(BinPropertyType.String, map.ValueType);
            Assert.Collection(map, x => Assert.Equal(element, x));
        }

        [Fact]
        public void Should_Throw_If_Key_Type_Does_Not_Match()
        {
            KeyValuePair<BinTreeProperty, BinTreeProperty> element =
                new(new BinTreeHash(0, 0x111), new BinTreeString(0, "hi"));

            Assert.Throws<ArgumentException>(
                () => new BinTreeMap(0, BinPropertyType.U32, BinPropertyType.String, new[] { element })
            );
        }

        [Fact]
        public void Should_Throw_If_Value_Type_Does_Not_Match()
        {
            KeyValuePair<BinTreeProperty, BinTreeProperty> element =
                new(new BinTreeHash(0, 0x111), new BinTreeString(0, "hi"));

            Assert.Throws<ArgumentException>(
                () => new BinTreeMap(0, BinPropertyType.Hash, BinPropertyType.F32, new[] { element })
            );
        }
    }

    public class IndexerTests
    {
        [Fact]
        public void Getter_Should_Return_The_Correct_Value()
        {
            KeyValuePair<BinTreeProperty, BinTreeProperty> element =
                new(new BinTreeHash(0, 0x111), new BinTreeString(0, "hi"));
            BinTreeMap map = new(0, BinPropertyType.Hash, BinPropertyType.String, new[] { element });

            BinTreeProperty value = map[new BinTreeHash(0, 0x111)];
            if (value is BinTreeString stringValue)
            {
                Assert.Equal("hi", stringValue);
            }
            else
            {
                Assert.Fail("value type does not match");
            }
        }

        [Fact]
        public void Setter_Should_Throw_If_Key_Type_Doesnt_Match()
        {
            KeyValuePair<BinTreeProperty, BinTreeProperty> element =
                new(new BinTreeHash(0, 0x111), new BinTreeString(0, "hi"));
            BinTreeMap map = new(0, BinPropertyType.Hash, BinPropertyType.String, new[] { element });

            Assert.Throws<ArgumentException>(() =>
            {
                map[new BinTreeU32(0, 0x111)] = new BinTreeString(0, "hi2");
            });
        }

        [Fact]
        public void Setter_Should_Throw_If_Value_Type_Doesnt_Match()
        {
            KeyValuePair<BinTreeProperty, BinTreeProperty> element =
                new(new BinTreeHash(0, 0x111), new BinTreeString(0, "hi"));
            BinTreeMap map = new(0, BinPropertyType.Hash, BinPropertyType.String, new[] { element });

            Assert.Throws<ArgumentException>(() =>
            {
                map[new BinTreeHash(0, 0x222)] = new BinTreeU32(0, 5);
            });
        }

        [Fact]
        public void Setter_Should_Add_The_Value_If_KeyType_And_ValueType_Match()
        {
            KeyValuePair<BinTreeProperty, BinTreeProperty> element =
                new(new BinTreeHash(0, 0x111), new BinTreeString(0, "hi"));
            BinTreeMap map = new(0, BinPropertyType.Hash, BinPropertyType.String, new[] { element });

            map[new BinTreeHash(0, 0x222)] = new BinTreeString(0, "hi2");

            Assert.Equal(new BinTreeString(0, "hi2"), map[new BinTreeHash(0, 0x222)]);
        }
    }

    public class EqualsTests
    {
        [Fact]
        public void Should_Return_False_If_NameHash_Is_Different()
        {
            BinTreeMap map1 =
                new(
                    0x1,
                    BinPropertyType.Hash,
                    BinPropertyType.String,
                    Array.Empty<KeyValuePair<BinTreeProperty, BinTreeProperty>>()
                );
            BinTreeMap map2 =
                new(
                    0x2,
                    BinPropertyType.Hash,
                    BinPropertyType.String,
                    Array.Empty<KeyValuePair<BinTreeProperty, BinTreeProperty>>()
                );

            Assert.False(map1.Equals(map2));
        }

        [Fact]
        public void Should_Return_False_If_Other_Is_Not_Map()
        {
            BinTreeMap map1 =
                new(
                    0x1,
                    BinPropertyType.Hash,
                    BinPropertyType.String,
                    Array.Empty<KeyValuePair<BinTreeProperty, BinTreeProperty>>()
                );
            BinTreeU32 other = new(0x1, 0);

            Assert.False(map1.Equals(other));
        }

        [Fact]
        public void Should_Return_False_If_KeyType_Is_Different()
        {
            BinTreeMap map1 =
                new(
                    0x1,
                    BinPropertyType.Hash,
                    BinPropertyType.String,
                    Array.Empty<KeyValuePair<BinTreeProperty, BinTreeProperty>>()
                );
            BinTreeMap map2 =
                new(
                    0x1,
                    BinPropertyType.U32,
                    BinPropertyType.String,
                    Array.Empty<KeyValuePair<BinTreeProperty, BinTreeProperty>>()
                );

            Assert.False(map1.Equals(map2));
        }

        [Fact]
        public void Should_Return_False_If_ValueType_Is_Different()
        {
            BinTreeMap map1 =
                new(
                    0x1,
                    BinPropertyType.Hash,
                    BinPropertyType.String,
                    Array.Empty<KeyValuePair<BinTreeProperty, BinTreeProperty>>()
                );
            BinTreeMap map2 =
                new(
                    0x1,
                    BinPropertyType.Hash,
                    BinPropertyType.F32,
                    Array.Empty<KeyValuePair<BinTreeProperty, BinTreeProperty>>()
                );

            Assert.False(map1.Equals(map2));
        }

        [Fact]
        public void Should_Return_True_If_Other_Has_Same_NameHash_KeyType_ValueType_And_Elements()
        {
            KeyValuePair<BinTreeProperty, BinTreeProperty> element =
                new(new BinTreeHash(0, 0x111), new BinTreeString(0, "hi"));

            BinTreeMap map1 = new(0x1, BinPropertyType.Hash, BinPropertyType.String, new[] { element });
            BinTreeMap map2 = new(0x1, BinPropertyType.Hash, BinPropertyType.String, new[] { element });

            Assert.True(map1.Equals(map2));
        }
    }
}
