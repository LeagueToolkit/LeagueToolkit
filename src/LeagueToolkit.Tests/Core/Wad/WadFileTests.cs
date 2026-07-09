using LeagueToolkit.Core.Wad;

namespace LeagueToolkit.Tests.Core.Wad;

public class WadFileTests
{
    [Theory]
    [InlineData(1, 0ul)]
    [InlineData(2, 0x1122334455667788ul)]
    [InlineData(3, 0x1122334455667788ul)]
    public void Should_Read_Chunk_Metadata(byte major, ulong expectedChecksum)
    {
        string path = CreateWad(
            major,
            new ChunkData(
                0x0102030405060708,
                0x10203040,
                0x11223344,
                0x55667788,
                WadChunkCompression.Zstd,
                true,
                3,
                0x1234,
                0x1122334455667788
            )
        );

        try
        {
            using WadFile wad = new(path);
            WadChunk chunk = Assert.Single(wad.Chunks).Value;

            Assert.Equal(0x0102030405060708ul, chunk.PathHash);
            Assert.Equal(0x10203040, chunk.DataOffset);
            Assert.Equal(0x11223344, chunk.CompressedSize);
            Assert.Equal(0x55667788, chunk.UncompressedSize);
            Assert.Equal(WadChunkCompression.Zstd, chunk.Compression);
            Assert.True(chunk.IsDuplicated);
            Assert.Equal(3, chunk.SubChunkCount);
            Assert.Equal(0x1234, chunk.StartSubChunk);
            Assert.Equal(expectedChecksum, chunk.Checksum);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void Should_Read_Multiple_Toc_Entries()
    {
        string path = CreateWad(
            3,
            new ChunkData(1, 10, 20, 30, WadChunkCompression.None, false, 0, 0, 100),
            new ChunkData(2, 40, 50, 60, WadChunkCompression.GZip, true, 2, 3, 200)
        );

        try
        {
            using WadFile wad = new(path);

            Assert.Equal(2, wad.Chunks.Count);
            Assert.Equal(100ul, wad.Chunks[1].Checksum);
            Assert.Equal(200ul, wad.Chunks[2].Checksum);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void Should_Throw_When_Toc_Is_Truncated()
    {
        string path = Path.GetTempFileName();
        try
        {
            using (FileStream stream = File.Create(path))
            using (BinaryWriter writer = new(stream))
            {
                WriteHeader(writer, 3, 1);
                writer.Write(new byte[31]);
            }

            using FileStream readStream = File.OpenRead(path);
            Assert.Throws<EndOfStreamException>(() => new WadFile(readStream));
        }
        finally
        {
            File.Delete(path);
        }
    }

    private static string CreateWad(byte major, params ChunkData[] chunks)
    {
        string path = Path.GetTempFileName();
        using FileStream stream = File.Create(path);
        using BinaryWriter writer = new(stream);

        WriteHeader(writer, major, chunks.Length);
        foreach (ChunkData chunk in chunks)
            WriteChunk(writer, major, chunk);

        return path;
    }

    private static void WriteHeader(BinaryWriter writer, byte major, int chunkCount)
    {
        writer.Write("RW"u8);
        writer.Write(major);
        writer.Write((byte)0);

        if (major is 2)
        {
            writer.Write((byte)0);
            writer.Write(new byte[83]);
            writer.Write(0ul);
        }
        else if (major is 3)
        {
            writer.Write(new byte[256]);
            writer.Write(0ul);
        }

        if (major is 1 or 2)
        {
            writer.Write((ushort)0);
            writer.Write((ushort)(major >= 2 ? 32 : 24));
        }

        writer.Write(chunkCount);
    }

    private static void WriteChunk(BinaryWriter writer, byte major, ChunkData chunk)
    {
        writer.Write(chunk.PathHash);
        writer.Write((uint)chunk.DataOffset);
        writer.Write(chunk.CompressedSize);
        writer.Write(chunk.UncompressedSize);
        writer.Write((byte)(((byte)chunk.Compression & 0xF) | (chunk.SubChunkCount << 4)));
        writer.Write(chunk.IsDuplicated);
        writer.Write((ushort)chunk.StartSubChunk);

        if (major >= 2)
            writer.Write(chunk.Checksum);
    }

    private readonly record struct ChunkData(
        ulong PathHash,
        long DataOffset,
        int CompressedSize,
        int UncompressedSize,
        WadChunkCompression Compression,
        bool IsDuplicated,
        int SubChunkCount,
        int StartSubChunk,
        ulong Checksum
    );
}
