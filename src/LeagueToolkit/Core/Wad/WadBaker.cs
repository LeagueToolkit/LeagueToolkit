using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance.Buffers;
using System.IO.Compression;
using XXHash3NET;

namespace LeagueToolkit.Core.Wad;

public static class WadBaker
{
    public static void BakeFiles(
        string rootDirectory,
        IEnumerable<string> files,
        string output,
        WadBakeSettings settings
    )
    {
        Guard.IsNotNull(files, nameof(files));

        BakeDirectory(
            new()
            {
                RootDirectory = rootDirectory,
                Files = files.ToArray(),
                Output = output,
                Setings = settings
            }
        );
    }

    private static void BakeDirectory(WadBakeContext context)
    {
        // Write the descriptor template
        using FileStream bakedWadStream = File.Create(context.Output);

        Span<byte> wadDescriptorTemplate =
            stackalloc byte[WadFile.HEADER_SIZE_V3 + (WadChunk.TOC_SIZE_V3 * context.Files.Length)];

        wadDescriptorTemplate.Clear();
        bakedWadStream.Write(wadDescriptorTemplate);

        foreach (string filePath in context.Files)
        {
            using FileStream fileStream = File.OpenRead(filePath);
            WadChunkCompression chunkCompression = WadUtils.GetExtensionCompression(Path.GetExtension(fileStream.Name));
            using Stream compressedFileStream = CreateChunkStream(fileStream, chunkCompression);

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
                using MemoryOwner<byte> chunkDataOwner = MemoryOwner<byte>.Allocate(compressedSize);

                compressedFileStream.Read(chunkDataOwner.Span);
                bakedWadStream.Write(chunkDataOwner.Span);
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

        // Seek to start and write actual descriptor
        bakedWadStream.Seek(0, SeekOrigin.Begin);

        WadFile bakedWad = new(context.Chunks);
        bakedWad.WriteDescriptor(bakedWadStream);
    }

    private static WadChunk CreateChunk()
    {
        return default;
    }

    private static Stream CreateChunkStream(FileStream fileStream, WadChunkCompression chunkCompression)
    {
        if (chunkCompression is WadChunkCompression.None)
            return fileStream;

        MemoryStream compressedStream = new();
        using Stream compressionStream = chunkCompression switch
        {
            WadChunkCompression.GZip => new GZipStream(compressedStream, CompressionMode.Compress),
            WadChunkCompression.Zstd => new ZstdNet.CompressionStream(compressedStream),
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

internal sealed class WadBakeContext
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
