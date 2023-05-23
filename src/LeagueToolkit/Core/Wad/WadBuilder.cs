using CommunityToolkit.Diagnostics;
using System.IO.Compression;
using XXHash3NET;

namespace LeagueToolkit.Core.Wad;

/// <summary>
/// Provides an interface for building a <see cref="WadFile"/>
/// </summary>
public static class WadBuilder
{
    /// <summary>
    /// Creates a new <see cref="WadFile"/> by "baking" it using the specified parameters
    /// </summary>
    /// <param name="files">The files to add to the <see cref="WadFile"/></param>
    /// <param name="rootDirectory">The root directory of the files to add</param>
    /// <param name="output">The path to the created <see cref="WadFile"/></param>
    /// <param name="settings">The settings to use for the baking process</param>
    public static void BakeFiles(
        IEnumerable<string> files,
        string rootDirectory,
        string output,
        WadBakeSettings settings
    )
    {
        Guard.IsNotNull(files, nameof(files));
        Guard.IsNotNull(rootDirectory, nameof(rootDirectory));
        Guard.IsNotNullOrEmpty(output, nameof(output));

        BakeFiles(
            new()
            {
                RootDirectory = rootDirectory,
                Files = files.ToArray(),
                Output = output,
                Setings = settings
            }
        );
    }

    private static void BakeFiles(WadBuildContext context)
    {
        // Write the descriptor template
        using FileStream bakedWadStream = File.Create(context.Output);
        bakedWadStream.Seek(WadFile.HEADER_SIZE_V3 + (WadChunk.TOC_SIZE_V3 * context.Files.Length), SeekOrigin.Begin);

        // Create chunks
        foreach (string filePath in context.Files)
            CreateChunk(filePath, bakedWadStream, context);

        // Seek to start and write actual descriptor
        bakedWadStream.Seek(0, SeekOrigin.Begin);
        WadFile bakedWad = new(context.Chunks);
        bakedWad.WriteDescriptor(bakedWadStream);
    }

    private static void CreateChunk(string filePath, Stream bakedWadStream, WadBuildContext context)
    {
        using FileStream fileStream = File.OpenRead(filePath);
        WadChunkCompression chunkCompression = WadUtils.GetExtensionCompression(Path.GetExtension(fileStream.Name));
        using Stream compressedFileStream = CreateChunkStream(fileStream, chunkCompression);

        // Get the stream checksum and check for duplication
        ulong streamChecksum = CreateChunkChecksum(compressedFileStream);
        var (dataOffset, isDuplicated) = context.ChecksumOffsetLookup.TryGetValue(
            streamChecksum,
            out long existingChunkOffset
        ) switch
        {
            true => (existingChunkOffset, true),
            false => (bakedWadStream.Position, false)
        };

        // Write chunk data if it's not a duplicate
        int uncompressedSize = (int)fileStream.Length;
        int compressedSize = (int)compressedFileStream.Length;
        if (isDuplicated is false)
        {
            compressedFileStream.CopyTo(bakedWadStream);
        }

        // Create chunk
        string chunkPath = Path.GetRelativePath(context.RootDirectory, filePath)
            .Replace(Path.DirectorySeparatorChar, '/')
            .ToLower();

        WadChunk chunk =
            new(
                XXHash64.Compute(chunkPath),
                dataOffset,
                compressedSize,
                uncompressedSize,
                chunkCompression,
                isDuplicated,
                0,
                0,
                streamChecksum
            );

        if (isDuplicated is false)
            context.ChecksumOffsetLookup.Add(streamChecksum, dataOffset);

        context.Chunks.Add(chunk);
    }

    private static Stream CreateChunkStream(FileStream fileStream, WadChunkCompression chunkCompression)
    {
        if (chunkCompression is WadChunkCompression.None)
            return fileStream;

        MemoryStream compressedStream = new();
        using Stream compressionStream = chunkCompression switch
        {
            WadChunkCompression.GZip => new GZipStream(compressedStream, CompressionMode.Compress),
            WadChunkCompression.Zstd => new ZstdSharp.CompressionStream(compressedStream),
            _ => throw new InvalidOperationException($"Invalid chunk compression: {chunkCompression}")
        };

        fileStream.CopyTo(compressionStream);

        return compressedStream;
    }

    private static ulong CreateChunkChecksum(Stream compressedStream)
    {
        compressedStream.Seek(0, SeekOrigin.Begin);
        ulong checksum = XXHash3.Hash64(compressedStream);
        compressedStream.Seek(0, SeekOrigin.Begin);

        return checksum;
    }
}

internal sealed class WadBuildContext
{
    public string RootDirectory { get; init; }
    public string[] Files { get; init; }
    public string Output { get; init; }
    public WadBakeSettings Setings { get; init; }

    public List<WadChunk> Chunks { get; } = new();
    public Dictionary<ulong, long> ChecksumOffsetLookup = new();
}

public struct WadBakeSettings
{
    public bool DetectDuplicateChunkData { get; set; }

    public WadBakeSettings()
    {
        this.DetectDuplicateChunkData = true;
    }
}
