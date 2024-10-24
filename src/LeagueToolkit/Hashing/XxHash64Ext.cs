using System.IO.Hashing;
using System.Text;
using CommunityToolkit.HighPerformance.Buffers;

namespace LeagueToolkit.Hashing;

public static class XxHash64Ext
{
    /// <summary>Computes the XxHash64 hash of the provided data.</summary>
    /// <param name="source">The data to hash.</param>
    /// <param name="seed">The seed value for this hash computation.</param>
    /// <returns>The computed XxHash64 hash.</returns>
    public static ulong Hash(ReadOnlySpan<char> source, long seed = 0)
    {
        int utf8ByteCount = Encoding.UTF8.GetByteCount(source);
        if (utf8ByteCount > 512)
        {
            using var sourceUtf8Owner = MemoryOwner<byte>.Allocate(utf8ByteCount);

            return HashUtf8Data(source, sourceUtf8Owner.Span, seed);
        }
        else
        {
            Span<byte> sourceUtf8 = stackalloc byte[utf8ByteCount];

            return HashUtf8Data(source, sourceUtf8, seed);
        }
    }

    private static ulong HashUtf8Data(ReadOnlySpan<char> source, Span<byte> utf8Destination, long seed)
    {
        int encodedByteCount = Encoding.UTF8.GetBytes(source, utf8Destination);

        return XxHash64.HashToUInt64(utf8Destination[..encodedByteCount], seed);
    }
}
