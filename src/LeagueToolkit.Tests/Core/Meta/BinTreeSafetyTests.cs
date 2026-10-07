using System;
using System.IO;
using LeagueToolkit.Core.Meta;
using LeagueToolkit.Core.Meta.Properties;
using LeagueToolkit.Toolkit.Ritobin;
using System.Collections.Generic;
using Xunit;

namespace LeagueToolkit.Tests.Core.Meta
{
    public sealed class BinTreeSafetyTests
    {
        [Fact]
        public void MaterialWideNamePreservesFollowingPropertiesAndAllBits()
        {
            const ulong name = 0xccdb6584d78a04f6;
            using var stream = CreateFile(writer =>
            {
                writer.Write(0x8d39bde6u);
                writer.Write((byte)BinPropertyType.Hash);
                writer.Write(name);
                writer.Write(0x0a6f0eb5u);
                writer.Write((byte)BinPropertyType.UnorderedContainer);
                writer.Write((byte)BinPropertyType.Embedded);
                writer.Write(4u);
                writer.Write(0u);
            }, propertyCount: 2, classHash: 0xff9d3409);

            BinTree tree = new(stream);
            Assert.Equal(name, Assert.IsType<BinTreeHash64>(tree.Objects[1].Properties[0x8d39bde6]).Value);
            Assert.Empty(Assert.IsType<BinTreeUnorderedContainer>(tree.Objects[1].Properties[0x0a6f0eb5]).Elements);
            Assert.Equal(stream.Length, stream.Position);
            using var roundTrip = new MemoryStream();
            tree.Write(roundTrip);
            Assert.Equal(stream.ToArray(), roundTrip.ToArray());
            using var writer = new RitobinWriter(
                Array.Empty<KeyValuePair<uint, string>>(), Array.Empty<KeyValuePair<uint, string>>(),
                new[] { new KeyValuePair<uint, string>(0x8d39bde6, "name") },
                Array.Empty<KeyValuePair<uint, string>>(), Array.Empty<KeyValuePair<ulong, string>>());
            Assert.Contains("name: hash = 0xccdb6584d78a04f6", writer.WritePropertyBin(tree));
        }

        [Fact]
        public void GenericHashesRemain32BitAndMaterialStringNamesRemainReadable()
        {
            var tree = new BinTree(new[]
            {
                new BinTreeObject(1, 123, new BinTreeProperty[] { new BinTreeHash(0x8d39bde6, 0x12345678) }),
                new BinTreeObject(2, 0xff9d3409, new BinTreeProperty[] { new BinTreeString(0x8d39bde6, "Original material name") })
            }, Array.Empty<string>());
            using var stream = new MemoryStream();
            tree.Write(stream);
            stream.Position = 0;

            BinTree parsed = new(stream);

            Assert.Equal(0x12345678u, Assert.IsType<BinTreeHash>(parsed.Objects[1].Properties[0x8d39bde6]).Value);
            Assert.Equal("Original material name", Assert.IsType<BinTreeString>(parsed.Objects[2].Properties[0x8d39bde6]).Value);
            Assert.Equal(stream.Length, stream.Position);
        }

        [Fact]
        public void EmbeddedMaterialWideNameRemainsReadable()
        {
            BinTree Create(ulong value) => new(new[]
            {
                new BinTreeObject(1, 123, new BinTreeProperty[]
                {
                    new BinTreeEmbedded(2, 0xff9d3409, new BinTreeProperty[] { new BinTreeHash64(0x8d39bde6, value) })
                })
            }, Array.Empty<string>());
            BinTree before = Create(0xccdb6584d78a04f6);

            using var stream = new MemoryStream();
            before.Write(stream);
            stream.Position = 0;

            BinTree parsed = new(stream);

            var material = Assert.IsType<BinTreeEmbedded>(parsed.Objects[1].Properties[2]);
            Assert.Equal(0xccdb6584d78a04f6UL, Assert.IsType<BinTreeHash64>(material.Properties[0x8d39bde6]).Value);
        }

        [Fact]
        public void UnknownModernTypeDoesNotRetryEarlierWadLinkAsHugeLegacyContainer()
        {
            using var stream = CreateFile(writer =>
            {
                writer.Write(1u);
                writer.Write((byte)BinPropertyType.WadChunkLink);
                writer.Write((byte)0);
                writer.Write(717455024u);
                writer.Write(new byte[] { 0x53, 0xd4, 0xe0 });
                writer.Write(0xadu);
                writer.Write((byte)0xb5);
            }, propertyCount: 2);

            var exception = Assert.Throws<InvalidPropertyTypeException>(() => new BinTree(stream));

            Assert.Equal((BinPropertyType)0xb5, exception.PropertyType);
            Assert.Equal(stream.Length, stream.Position);
        }

        [Theory]
        [InlineData(128)]
        [InlineData(134)]
        [InlineData(130)]
        public void DeclaredBlockBeyondFileIsRejectedBeforeReadingElements(byte type)
        {
            using var stream = CreateFile(writer =>
            {
                writer.Write(1u);
                writer.Write(type);
                if (type == 130) writer.Write(1u);
                else
                {
                    writer.Write((byte)BinPropertyType.None);
                    if (type == 134) writer.Write((byte)BinPropertyType.None);
                }
                writer.Write(717455024u);
                writer.Write(2917202515u);
            });

            Assert.Throws<InvalidDataException>(() => new BinTree(stream));
        }

        [Theory]
        [InlineData(128)]
        [InlineData(134)]
        public void EmptyValueCountCannotOverflowReaderLoop(byte type)
        {
            using var stream = CreateFile(writer =>
            {
                writer.Write(1u);
                writer.Write(type);
                writer.Write((byte)BinPropertyType.None);
                if (type == 134) writer.Write((byte)BinPropertyType.None);
                writer.Write(4u);
                writer.Write(uint.MaxValue);
            });

            Assert.Throws<InvalidDataException>(() => new BinTree(stream));
        }

        [Theory]
        [InlineData(19, BinPropertyType.Struct)]
        [InlineData(20, BinPropertyType.Embedded)]
        [InlineData(21, BinPropertyType.ObjectLink)]
        [InlineData(22, BinPropertyType.Optional)]
        [InlineData(23, BinPropertyType.Map)]
        [InlineData(24, BinPropertyType.BitBool)]
        public void ActualLegacyTypesRemainReadable(byte legacyType, BinPropertyType expectedType)
        {
            using var stream = CreateFile(writer =>
            {
                writer.Write(1u);
                writer.Write(legacyType);
                switch (legacyType)
                {
                    case 19:
                    case 20:
                        writer.Write(123u);
                        writer.Write(2u);
                        writer.Write((ushort)0);
                        break;
                    case 21:
                        writer.Write(456u);
                        break;
                    case 22:
                        writer.Write((byte)BinPropertyType.U8);
                        writer.Write(true);
                        writer.Write((byte)7);
                        break;
                    case 23:
                        writer.Write((byte)BinPropertyType.U8);
                        writer.Write((byte)BinPropertyType.U8);
                        writer.Write(6u);
                        writer.Write(1u);
                        writer.Write((byte)1);
                        writer.Write((byte)2);
                        break;
                    case 24:
                        writer.Write(true);
                        break;
                }
            });

            BinTree tree = new(stream);

            Assert.Equal(expectedType, tree.Objects[1].Properties[1].Type);
            Assert.Equal(stream.Length, stream.Position);
            if (expectedType == BinPropertyType.Map)
                Assert.Single(Assert.IsType<BinTreeMap>(tree.Objects[1].Properties[1]));
        }

        private static MemoryStream CreateFile(Action<BinaryWriter> writeProperties, ushort propertyCount = 1, uint classHash = 123)
        {
            using var properties = new MemoryStream();
            using (var writer = new BinaryWriter(properties, System.Text.Encoding.UTF8, leaveOpen: true))
                writeProperties(writer);

            var stream = new MemoryStream();
            using (var writer = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                writer.Write("PROP"u8);
                writer.Write(3u);
                writer.Write(0u);
                writer.Write(1u);
                writer.Write(classHash);
                writer.Write((uint)(6 + properties.Length));
                writer.Write(1u);
                writer.Write(propertyCount);
                writer.Write(properties.ToArray());
            }
            stream.Position = 0;
            return stream;
        }
    }
}
