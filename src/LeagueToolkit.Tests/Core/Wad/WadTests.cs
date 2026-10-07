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

            using var file1Data = wad.LoadChunkDecompressed(file1Chunk);
            Assert.Equal(file1Content, file1Data.Span.ToArray());

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
}
