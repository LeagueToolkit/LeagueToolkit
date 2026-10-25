using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LeagueToolkit.IO.Inibin;
using Xunit;

namespace LeagueToolkit.Tests.IO;

public class InibinTests
{
    private static InibinFile Read(byte[] bytes) => new(new MemoryStream(bytes));
    private static byte[] Write(InibinFile file)
    {
        using var stream = new MemoryStream();
        file.Write(stream, true);
        return stream.ToArray();
    }
    private static byte[] Build(Action<BinaryWriter> action)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.UTF8, true);
        action(writer);
        writer.Flush();
        return stream.ToArray();
    }
    private static byte[] V1((uint Key, uint Offset)[] entries, byte[] data) => Build(w =>
    {
        w.Write(new byte[] { 1, 0, 0, 0 }); w.Write((uint)entries.Length); w.Write((uint)data.Length);
        foreach (var (key, offset) in entries) { w.Write(key); w.Write(offset); }
        w.Write(data);
    });
    private static byte[] V2(ushort flags, ushort length, Action<BinaryWriter> body) => Build(w =>
    {
        w.Write((byte)2); w.Write(length); w.Write(flags); body(w);
    });
    private static byte[] Padded(byte[] data) => data[..3].Concat(new byte[2]).Concat(data[3..]).ToArray();

    private static void EqualValues(InibinFile expected, InibinFile actual)
    {
        Assert.Equal(expected.Sets.Sum(s => s.Value.Properties.Count), actual.Sets.Sum(s => s.Value.Properties.Count));
        foreach (var (type, set) in expected.Sets)
            foreach (var (key, value) in set.Properties)
            {
                Assert.True(actual.Sets.TryGetValue(type, out var other));
                Assert.True(other.Properties.TryGetValue(key, out var read));
                if (value is byte[] bytes) Assert.Equal(bytes, Assert.IsType<byte[]>(read));
                else if (value is float[] floats) Assert.Equal(floats, Assert.IsType<float[]>(read));
                else Assert.Equal(value, read);
            }
    }

    [Fact]
    public void ReaderSupportsEveryStorageTypeAndNamedProperties()
    {
        var file = Read(EveryTypeBytes());
        for (int bit = 0; bit < 14; bit++)
            Assert.Single(file.Sets[(InibinFlags)(1 << bit)].Properties);
        Assert.Equal(100000, file.GetInt32(1));
        Assert.Equal(1.25f, file.GetFloat(2));
        Assert.Equal(1.5f, file.GetFloat(3));
        Assert.Equal(-50, file.GetInt32(4));
        Assert.Equal(200, file.GetInt32(5));
        Assert.Equal(true, file.GetBoolean(6));
        Assert.Equal(new float[] { 0, 20, 0 }, file.GetVector(7, 3));
        Assert.Equal(new float[] { 40, 80, 12 }, file.GetVector(8, 3));
        Assert.Equal(-7L, file.GetInt64(14));
        Assert.Equal(0x0616933au, InibinFile.HashKey("System", "GroupPart1"));
        Assert.Equal("glow", file.GetString("SYSTEM", "grouppart1"));
        EqualValues(file, Read(Write(file)));
    }

    [Fact]
    public void ReaderRoundTripsSharedStringsByteForByte()
    {
        byte[] bytes = V2(0x1000, 10, w =>
        {
            w.Write((ushort)3); w.Write(1u); w.Write(2u); w.Write(3u);
            w.Write((ushort)0); w.Write((ushort)5); w.Write((ushort)0);
            w.Write(Encoding.UTF8.GetBytes("glow\0core\0"));
        });
        var file = Read(bytes);
        Assert.Equal(3, file.Sets[InibinFlags.StringList].Properties.Count);
        Assert.Equal("glow", file.GetString(1));
        Assert.Equal("core", file.GetString(2));
        Assert.Equal("glow", file.GetString(3));
        Assert.Equal(bytes, Write(file));
    }

    [Fact]
    public void PaddedAndUnpaddedFilesReadToTheSameValues()
    {
        byte[] unpadded = EveryTypeBytes();
        byte[] padded = Padded(unpadded);
        var file = Read(padded);
        Assert.Equal(14, file.Sets.Sum(s => s.Value.Properties.Count));
        EqualValues(Read(unpadded), file);
        EqualValues(file, Read(Write(file)));
        Assert.Equal(Write(Read(unpadded)), Write(file));
    }

    [Fact]
    public void V1ReadsInterleavedKeysAnd32BitOffsetsThenWritesV2()
    {
        byte[] data = Encoding.UTF8.GetBytes("1 0.5 0\0" + new string('x', 70000) + "\0" + "450\0");
        var file = Read(V1(new[] { (0xbbu, 70009u), (0xaau, 0u), (0xccu, 70009u) }, data));
        Assert.Equal(450, file.GetInt32(0xbb));
        Assert.Equal("450", file.GetString(0xcc));
        Assert.Equal(new float[] { 1, 0.5f, 0 }, file.GetVector(0xaa, 3));
        byte[] written = Write(file);
        Assert.Equal(2, written[0]);
        EqualValues(file, Read(written));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void V2ReadsI64AfterStringOffsetsBeforeStringData(bool padded)
    {
        byte[] bytes = V2(0x3000, 5, w =>
        {
            w.Write((ushort)1); w.Write(0xaau); w.Write((ushort)0);
            w.Write((ushort)1); w.Write(0xbbu); w.Write(-7L);
            w.Write(Encoding.UTF8.GetBytes("glow\0"));
        });
        var file = Read(padded ? Padded(bytes) : bytes);
        Assert.Equal("glow", file.GetString(0xaa));
        Assert.Equal(-7L, file.GetInt64(0xbb));
        Assert.Equal(bytes, Write(file));
    }

    [Fact]
    public void WriterOrdersSetsAndKeysIndependentlyOfInsertionOrder()
    {
        var first = new InibinFile(); first.AddValue(3, "glow"); first.AddValue(2, 20000000000L); first.AddValue(8, 4); first.AddValue(1, 9);
        var second = new InibinFile(); second.AddValue(1, 9); second.AddValue(8, 4); second.AddValue(2, 20000000000L); second.AddValue(3, "glow");
        Assert.Equal(Write(first), Write(second));
        EqualValues(first, Read(Write(first)));
    }

    [Fact]
    public void WriterSharesUtf8SuffixesAndUsesByteLengths()
    {
        var file = new InibinFile();
        file.AddValue(1, "églow"); file.AddValue(2, "glow"); file.AddValue(3, "églow"); file.AddValue(4, "");
        byte[] bytes = Write(file);
        Assert.Equal(Encoding.UTF8.GetByteCount("églow\0"), BitConverter.ToUInt16(bytes, 1));
        EqualValues(file, Read(bytes));
    }

    [Theory]
    [InlineData(1)] [InlineData(7)] [InlineData(8)] [InlineData(9)] [InlineData(16)] [InlineData(17)]
    public void U1PacksLowBitsFirstAndZeroesUnusedBits(int count)
    {
        var file = new InibinFile();
        for (uint i = 0; i < count; i++) file.AddValue(i, i % 3 == 0);
        byte[] bytes = Write(file);
        int start = 7 + count * 4;
        for (int i = 0; i < (count + 7) / 8; i++)
        {
            byte expected = 0;
            for (int bit = 0; bit < 8 && i * 8 + bit < count; bit++)
                if ((i * 8 + bit) % 3 == 0) expected |= (byte)(1 << bit);
            Assert.Equal(expected, bytes[start + i]);
        }
        EqualValues(file, Read(bytes));
        Assert.Equal(1, file.GetInt32(0));
        Assert.Equal(1f, file.GetFloat(0));
    }

    [Fact]
    public void FixedPointScalarAndVectorsKeepAllByteValues()
    {
        var file = new InibinFile();
        for (uint i = 0; i < 256; i++) file.AddValue(i, i / 10.0);
        file.AddValue(1000, new byte[] { 0, 255 });
        file.AddValue(1001, new byte[] { 10, 15, 200 });
        file.AddValue(1002, new byte[] { 1, 2, 3, 4 });
        EqualValues(file, Read(Write(file)));
        Assert.Equal(new float[] { 0, 25.5f }, file.GetVector(1000, 2));
    }

    [Fact]
    public void ReaderSupportsNonSeekableShortReadsAndLeavesTrailingData()
    {
        var source = new InibinFile(); source.AddValue(1, "glow"); source.AddValue(2, long.MaxValue);
        byte[] bytes = Write(source).Concat(new byte[] { 0xa5 }).ToArray();
        using var stream = new ShortReadStream(bytes);
        var read = new InibinFile(stream, true);
        EqualValues(source, read);
        Assert.Equal(0xa5, stream.ReadByte());
        Assert.Equal(-1, stream.ReadByte());
    }

    [Theory]
    [InlineData(false)] [InlineData(true)]
    public void V1StopsAtDeclaredStringBlock(bool nonSeekable)
    {
        byte[] bytes = V1(new[] { (1u, 0u) }, Encoding.UTF8.GetBytes("glow\0")).Concat(new byte[] { 99 }).ToArray();
        using Stream stream = nonSeekable ? new ShortReadStream(bytes) : new MemoryStream(bytes);
        var file = new InibinFile(stream, true);
        Assert.Equal("glow", file.GetString(1));
        Assert.Equal(99, stream.ReadByte());
    }

    [Theory]
    [InlineData(false)] [InlineData(true)]
    public void ReadAndWriteRespectStreamOwnership(bool leaveOpen)
    {
        var input = new MemoryStream(new byte[] { 2, 0, 0, 0, 0 });
        var file = new InibinFile(input, leaveOpen);
        Assert.Equal(leaveOpen, input.CanRead);
        var output = new MemoryStream(); file.Write(output, leaveOpen);
        Assert.Equal(leaveOpen, output.CanWrite);
        input.Dispose(); output.Dispose();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ReaderRejectsEveryTruncationOfEveryTypeFile(bool padded)
    {
        byte[] bytes = EveryTypeBytes();
        if (padded) bytes = Padded(bytes);
        for (int i = 0; i < bytes.Length; i++) Assert.ThrowsAny<IOException>(() => Read(bytes[..i]));
    }

    [Fact]
    public void ReaderRejectsEveryTruncationOfV1()
    {
        byte[] bytes = V1(new[] { (1u, 0u), (2u, 5u) }, Encoding.UTF8.GetBytes("glow\0core\0"));
        for (int i = 0; i < bytes.Length; i++) Assert.ThrowsAny<IOException>(() => Read(bytes[..i]));
    }

    [Theory]
    [InlineData(0)] [InlineData(3)] [InlineData(255)]
    public void ReaderRejectsUnsupportedVersions(byte version) => Assert.Throws<InvalidDataException>(() => Read(new[] { version }));

    [Fact]
    public void ReaderHandlesEmptyFilesPaddingAndUnknownFlags()
    {
        Assert.Empty(Read(new byte[] { 2, 0, 0, 0, 0 }).Sets);
        Assert.Empty(Read(new byte[] { 2, 0, 0, 0, 0, 0, 0 }).Sets);
        Assert.Empty(Read(V1(Array.Empty<(uint, uint)>(), Array.Empty<byte>())).Sets);
        Assert.Empty(Read(new byte[] { 2, 0, 0, 0, 0xc0 }).Sets);
        Assert.Throws<EndOfStreamException>(() => Read(new byte[] { 2, 0, 0, 0, 0, 0 }));
    }

    [Fact]
    public void ReaderRejectsDuplicateKeysWithinAndAcrossSetsAndV1()
    {
        Assert.Throws<InvalidDataException>(() => Read(V1(new[] { (7u, 0u), (7u, 0u) }, new byte[] { 97, 0 })));
        Assert.Throws<InvalidDataException>(() => Read(V2(0x10, 0, w => { w.Write((ushort)2); w.Write(7u); w.Write(7u); w.Write(new byte[] { 1, 2 }); })));
        Assert.Throws<InvalidDataException>(() => Read(V2(0x1010, 2, w =>
        {
            w.Write((ushort)1); w.Write(7u); w.Write((byte)1);
            w.Write((ushort)1); w.Write(7u); w.Write((ushort)0); w.Write(new byte[] { 97, 0 });
        })));
    }

    [Theory]
    [InlineData(0, new byte[] { 97 })]
    [InlineData(1, new byte[] { 0 })]
    [InlineData(500, new byte[] { 97, 0 })]
    [InlineData(0, new byte[] { 0xff, 0 })]
    public void ReaderRejectsInvalidStrings(ushort offset, byte[] data)
    {
        Assert.Throws<InvalidDataException>(() => Read(V2(0x1000, (ushort)data.Length, w =>
        {
            w.Write((ushort)1); w.Write(1u); w.Write(offset); w.Write(data);
        })));
        Assert.Throws<InvalidDataException>(() => Read(V1(new[] { (1u, (uint)offset) }, data)));
    }

    [Fact]
    public void ReaderAcceptsUnsortedKeysAndSharedSuffixOffsets()
    {
        var file = Read(V2(0x1000, 5, w =>
        {
            w.Write((ushort)3); w.Write(3u); w.Write(1u); w.Write(2u);
            w.Write((ushort)0); w.Write((ushort)1); w.Write((ushort)1); w.Write(Encoding.UTF8.GetBytes("glow\0"));
        }));
        Assert.Equal("glow", file.GetString(3)); Assert.Equal("low", file.GetString(1)); Assert.Equal("low", file.GetString(2));
    }

    [Fact]
    public void WriterRejectsOverflowNulAndInvalidUtf16()
    {
        var file = new InibinFile(); file.AddValue(1, new string('a', 65535));
        Assert.Throws<InvalidDataException>(() => Write(file));
        file.Sets[InibinFlags.StringList].Properties[1] = "a\0b";
        Assert.Throws<InvalidDataException>(() => Write(file));
        file.Sets[InibinFlags.StringList].Properties[1] = "\ud800";
        Assert.Throws<InvalidDataException>(() => Write(file));
        var large = new InibinFile();
        large.Sets.Add(InibinFlags.Int32List, new(InibinFlags.Int32List, Enumerable.Range(0, 65536).ToDictionary(i => (uint)i, i => (object)i)));
        Assert.Throws<InvalidDataException>(() => Write(large));
    }

    [Fact]
    public void WriterFitsTheMaximumStringBlockAndCompressesRepeatedLongStrings()
    {
        var file = new InibinFile(); file.AddValue(1, new string('x', 65534)); file.AddValue(2, new string('x', 65534));
        byte[] bytes = Write(file);
        Assert.Equal(65535, BitConverter.ToUInt16(bytes, 1));
        EqualValues(file, Read(bytes));
    }

    [Fact]
    public void WriterRejectsInconsistentPublicSetsAndDuplicateKeys()
    {
        var file = new InibinFile(); file.AddValue(1, 1);
        file.Sets.Add(InibinFlags.StringList, new(InibinFlags.StringList, new() { [1] = "a" }));
        Assert.Throws<InvalidDataException>(() => Write(file));
        Assert.Throws<ArgumentException>(() => file.AddValue(1, 2f));
        file.Sets.Clear(); file.Sets.Add(InibinFlags.Int32List, new(InibinFlags.Int16List));
        Assert.Throws<InvalidDataException>(() => Write(file));
        file.Sets.Clear(); file.Sets.Add((InibinFlags)3, new((InibinFlags)3));
        Assert.Throws<InvalidDataException>(() => Write(file));
    }

    [Theory]
    [InlineData(-0.1)] [InlineData(25.6)] [InlineData(1.25)] [InlineData(double.NaN)] [InlineData(double.PositiveInfinity)]
    public void FixedPointRejectsLossyValues(double value) => Assert.Throws<FixedPointFloatOverflowException>(() => new InibinFile().AddValue(1, value));

    [Theory]
    [InlineData("  -17abc", -17)] [InlineData("3.9", 3)] [InlineData("Yes", 0)]
    [InlineData("999999999999999999999", long.MaxValue)] [InlineData("-999999999999999999999", long.MinValue)]
    public void IntegerConversionsMatchClientPrefixAndSaturation(string text, long expected)
    {
        var file = new InibinFile(); file.AddValue(1, text);
        Assert.Equal(expected, file.GetInt64(1));
    }

    [Theory]
    [InlineData(" -.5 text", -0.5f)] [InlineData("2e", 2f)] [InlineData("5.5.000", 5.5f)] [InlineData("true", 0f)]
    [InlineData("1E+09", 1e9f)] [InlineData("-inf", float.NegativeInfinity)] [InlineData("INFINITY", float.PositiveInfinity)]
    public void FloatConversionsMatchClientPrefix(string text, float expected)
    {
        var file = new InibinFile(); file.AddValue(1, text);
        Assert.Equal(expected, file.GetFloat(1));
    }

    [Fact]
    public void ConversionsPreserveClientBooleanAndVectorRules()
    {
        var file = new InibinFile();
        file.AddValue(1, "true"); file.AddValue(2, "1text"); file.AddValue(3, 1); file.AddValue(4, "1 .5 0 trailing");
        file.AddValue(5, "NaN"); file.AddValue(6, long.MaxValue); file.AddValue(7, float.PositiveInfinity);
        Assert.False(file.GetBoolean(1)); Assert.True(file.GetBoolean(2)); Assert.Null(file.GetBoolean(3));
        Assert.Equal(new float[] { 1, 0.5f, 0 }, file.GetVector(4, 3));
        Assert.Null(file.GetVector(4, 4)); Assert.Null(file.GetString(3)); Assert.Null(file.GetFloat(99));
        Assert.True(float.IsNaN(file.GetFloat(5)!.Value));
        Assert.Equal(-1, file.GetInt32(6)); Assert.Equal(int.MaxValue, file.GetInt32(7));
    }

    [Fact]
    public void CompactValuesSelectSmallestExactStorageAndPreserveNegativeZero()
    {
        var file = new InibinFile();
        file.AddCompactValue(1, 1L); file.AddCompactValue(2, 200L); file.AddCompactValue(3, -1L);
        file.AddCompactValue(4, 70000L); file.AddCompactValue(5, long.MaxValue);
        file.AddCompactValue(6, 1.5f); file.AddCompactValue(7, 1.25f); file.AddCompactValue(8, -0.0f);
        file.AddCompactValue(9, new float[] { 0, 1.5f }); file.AddCompactValue(10, new float[] { 1.25f, 0 });
        Assert.True(file.Sets[InibinFlags.BitList].Properties.ContainsKey(1));
        Assert.True(file.Sets[InibinFlags.Int8List].Properties.ContainsKey(2));
        Assert.True(file.Sets[InibinFlags.Int16List].Properties.ContainsKey(3));
        Assert.True(file.Sets[InibinFlags.Int32List].Properties.ContainsKey(4));
        Assert.True(file.Sets[InibinFlags.Int64List].Properties.ContainsKey(5));
        Assert.True(file.Sets[InibinFlags.FixedPointFloatList].Properties.ContainsKey(6));
        Assert.True(file.Sets[InibinFlags.Float32List].Properties.ContainsKey(7));
        Assert.True(float.IsNegative(Read(Write(file)).GetFloat(8)!.Value));
        Assert.Equal(new byte[] { 0, 15 }, file.Sets[InibinFlags.FixedPointFloatListVec2].Properties[9]);
        EqualValues(file, Read(Write(file)));
    }

    [Fact]
    public void HashKeyFoldsOnlyAsciiAndSignExtendsUtf8Bytes()
    {
        Assert.Equal(InibinFile.HashKey("System", "GroupPart1"), InibinFile.HashKey("SYSTEM", "GROUPPART1"));
        Assert.NotEqual(InibinFile.HashKey("Ä", "x"), InibinFile.HashKey("ä", "x"));
        uint expected = 0;
        foreach (sbyte value in new sbyte[] { -61, -92, 42, 120 }) expected = unchecked(expected * 65599 + (uint)value);
        Assert.Equal(expected, InibinFile.HashKey("ä", "x"));
    }

    [Fact]
    public void WriterPreservesEveryTypeFileBytes()
    {
        byte[] bytes = EveryTypeBytes();
        var file = Read(bytes);
        byte[] written = Write(file);
        Assert.Equal(bytes, written);
    }

    private static byte[] EveryTypeBytes() => V2(0x3fff, 5, w =>
    {
        for (int bit = 0; bit < 14; bit++)
        {
            w.Write((ushort)1);
            w.Write(bit == 12 ? 0x0616933au : (uint)(bit + 1));
            switch (bit)
            {
                case 0: w.Write(100000); break;
                case 1: w.Write(1.25f); break;
                case 2: w.Write((byte)15); break;
                case 3: w.Write((short)-50); break;
                case 4: w.Write((byte)200); break;
                case 5: w.Write((byte)1); break;
                case 6: w.Write(new byte[] { 0, 200, 0 }); break;
                case 7: w.Write(40f); w.Write(80f); w.Write(12f); break;
                case 8: w.Write(new byte[] { 20, 10 }); break;
                case 9: w.Write(0.5f); w.Write(-45f); break;
                case 10: w.Write(new byte[] { 10, 10, 10, 5 }); break;
                case 11: w.Write(0.75f); w.Write(3f); w.Write(5f); w.Write(0f); break;
                case 12: w.Write((ushort)0); break;
                case 13: w.Write(-7L); break;
            }
        }
        w.Write(Encoding.UTF8.GetBytes("glow\0"));
    });

    [Fact]
    public void StandaloneSetReadersAndWritersPreserveLegacySignatures()
    {
        var set = new InibinSet(InibinFlags.Int64List, new() { [8] = long.MaxValue, [1] = long.MinValue });
        byte[] bytes = Build(set.Write);
        using var reader = new BinaryReader(new MemoryStream(bytes));
        var read = new InibinSet(reader, InibinFlags.Int64List);
        Assert.Equal(new uint[] { 1, 8 }, read.Properties.Keys);
        Assert.Equal(long.MinValue, read.Properties[1]);
        Assert.Equal(long.MaxValue, read.Properties[8]);
        var strings = new InibinSet(InibinFlags.StringList, new() { [5] = "églow", [3] = "glow" });
        byte[] stringBytes = Build(strings.Write);
        using var stringReader = new BinaryReader(new MemoryStream(stringBytes));
        var stringRead = new InibinSet(stringReader, InibinFlags.StringList, 14);
        Assert.Equal("églow", stringRead.Properties[5]);
        Assert.Equal("glow", stringRead.Properties[3]);
        Assert.Equal(14, stringReader.BaseStream.Position);
        byte[] v1 = V1(new[] { (7u, 0u), (8u, 0u) }, Encoding.UTF8.GetBytes("text\0"));
        using var v1Reader = new BinaryReader(new MemoryStream(v1));
        v1Reader.BaseStream.Position = 12;
        var legacy = new InibinSet(v1Reader, InibinFlags.StringList, 28, 2);
        Assert.Equal("text", legacy.Properties[7]);
        Assert.Equal("text", legacy.Properties[8]);
        Assert.Equal(28, v1Reader.BaseStream.Position);
    }

    [Fact]
    public void WriterAcceptsMaximumSetCountAndOmitsEmptySets()
    {
        var file = new InibinFile();
        file.Sets.Add(InibinFlags.Int32List, new(InibinFlags.Int32List, Enumerable.Range(0, 65535).ToDictionary(i => (uint)i, i => (object)i)));
        file.Sets.Add(InibinFlags.Int64List, new(InibinFlags.Int64List));
        byte[] bytes = Write(file);
        Assert.Equal((ushort)InibinFlags.Int32List, BitConverter.ToUInt16(bytes, 3));
        Assert.Equal(65535, Read(bytes).Sets[InibinFlags.Int32List].Properties.Count);
    }

    [Fact]
    public void WriterRejectsDirectDictionaryValueTypeAndVectorLengthErrors()
    {
        var file = new InibinFile();
        file.Sets.Add(InibinFlags.Int32List, new(InibinFlags.Int32List, new() { [1] = (short)1 }));
        Assert.Throws<InvalidDataException>(() => Write(file));
        file.Sets.Clear();
        file.Sets.Add(InibinFlags.Float32ListVec3, new(InibinFlags.Float32ListVec3, new() { [1] = new float[] { 1, 2 } }));
        Assert.Throws<InvalidDataException>(() => Write(file));
    }

    [Fact]
    public void ReadWritePreservesExtremeScalarAndFloatBitPatterns()
    {
        var file = new InibinFile();
        file.AddValue(1, int.MinValue); file.AddValue(2, int.MaxValue);
        file.AddValue(3, short.MinValue); file.AddValue(4, short.MaxValue);
        file.AddValue(5, byte.MaxValue); file.AddValue(6, long.MinValue); file.AddValue(7, long.MaxValue);
        file.AddValue(8, float.NegativeInfinity); file.AddValue(9, float.PositiveInfinity);
        float nan = BitConverter.Int32BitsToSingle(unchecked((int)0xffc00001));
        file.AddValue(10, nan);
        var copy = Read(Write(file));
        EqualValues(file, copy);
        Assert.Equal(BitConverter.SingleToInt32Bits(nan), BitConverter.SingleToInt32Bits(copy.GetFloat(10)!.Value));
    }

    private sealed class ShortReadStream(byte[] bytes) : Stream
    {
        private readonly MemoryStream _stream = new(bytes);
        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException();
        public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
        public override int Read(byte[] buffer, int offset, int count) => _stream.Read(buffer, offset, Math.Min(1, count));
        public override int Read(Span<byte> buffer) => _stream.Read(buffer[..Math.Min(1, buffer.Length)]);
        public override int ReadByte() => _stream.ReadByte();
        public override void Flush() { }
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
        protected override void Dispose(bool disposing) { if (disposing) _stream.Dispose(); base.Dispose(disposing); }
    }
}
