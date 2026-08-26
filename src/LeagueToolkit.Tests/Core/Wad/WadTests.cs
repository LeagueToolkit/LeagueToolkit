using LeagueToolkit.Core.Wad;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace LeagueToolkit.Tests.Core.Wad;

public class WadTests
{
    [Fact]
    public void TestBakeAndReadHeader()
    {
        // 1. Create dummy files/streams in memory
        var file1Content = Encoding.UTF8.GetBytes("Hello WAD file 1!");
        var file2Content = Encoding.UTF8.GetBytes("Some another test content here.");

        var entries = new List<WadBakeEntry>
        {
            new WadBakeEntry("assets/text/file1.txt", () => new MemoryStream(file1Content)),
            new WadBakeEntry("assets/text/file2.txt", () => new MemoryStream(file2Content))
        };

        // 2. Bake WAD to an in-memory stream
        using var wadStream = new MemoryStream();
        WadBakeSettings settings = new WadBakeSettings();
        WadBuilder.Bake(entries, wadStream, settings);

        // 3. Test TryReadHeader on the baked WAD
        // We write the WAD stream to a temp file to test TryReadHeader
        string tempPath = Path.GetTempFileName();
        try
        {
            File.WriteAllBytes(tempPath, wadStream.ToArray());

            // 4. Test opening the WAD using WadFile and verify contents
            using var wad = new WadFile(tempPath);
            Assert.Equal(2, wad.Chunks.Count);

            // Verify first file
            var file1Chunk = wad.FindChunk("assets/text/file1.txt");
            using var file1Stream = wad.OpenChunk(file1Chunk);
            using var ms1 = new MemoryStream();
            file1Stream.CopyTo(ms1);
            Assert.Equal("Hello WAD file 1!", Encoding.UTF8.GetString(ms1.ToArray()));

            // Verify second file
            var file2Chunk = wad.FindChunk("assets/text/file2.txt");
            using var file2Stream = wad.OpenChunk(file2Chunk);
            using var ms2 = new MemoryStream();
            file2Stream.CopyTo(ms2);
            Assert.Equal("Some another test content here.", Encoding.UTF8.GetString(ms2.ToArray()));
        }
        finally
        {
            if (File.Exists(tempPath))
                File.Delete(tempPath);
        }
    }

    [Fact]
    public void ReadsV3_4SubchunkIndexWithHighByte()
    {
        string tempPath = CreateVersionedWad(4, 1, 0x2345);
        try
        {
            using var wad = new WadFile(tempPath);
            WadChunk chunk = Assert.Single(wad.Chunks).Value;

            Assert.Equal(0x12345, chunk.StartSubChunk);
            Assert.Equal(2, chunk.SubChunkCount);
            Assert.False(chunk.IsDuplicated);
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    [Fact]
    public void PreservesV3_3DuplicateFlag()
    {
        string tempPath = CreateVersionedWad(3, 1, 0x2345);
        try
        {
            using var wad = new WadFile(tempPath);
            WadChunk chunk = Assert.Single(wad.Chunks).Value;

            Assert.Equal(0x2345, chunk.StartSubChunk);
            Assert.Equal(2, chunk.SubChunkCount);
            Assert.True(chunk.IsDuplicated);
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    private static string CreateVersionedWad(byte minor, byte subChunkIndexHighOrDuplicate, ushort subChunkIndexLow)
    {
        string tempPath = Path.GetTempFileName();
        using var stream = new MemoryStream();
        using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
        {
            writer.Write(Encoding.ASCII.GetBytes("RW"));
            writer.Write((byte)3);
            writer.Write(minor);
            writer.Write(new byte[256]);
            writer.Write(0UL);
            writer.Write(1);

            writer.Write(0x1234567890abcdefUL);
            writer.Write(304U);
            writer.Write(0);
            writer.Write(0);
            writer.Write((byte)((2 << 4) | (byte)WadChunkCompression.ZstdChunked));
            writer.Write(subChunkIndexHighOrDuplicate);
            writer.Write(subChunkIndexLow);
            writer.Write(0UL);
        }

        File.WriteAllBytes(tempPath, stream.ToArray());
        return tempPath;
    }
}
