using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Buffers;
using LeagueToolkit.Utils.Extensions;

namespace LeagueToolkit.Tests.Utilities.Extensions;

public class StreamExtensionsTests
{
    public class ReadExactTests
    {
        [Fact]
        public void Should_Read_Data_Into_The_Buffer()
        {
            Span<byte> dummyData = stackalloc byte[4] { 0, 1, 2, 3 };
            Span<byte> buffer = stackalloc byte[4];

            using var stream = MemoryOwner<byte>.Allocate(4).AsStream();

            stream.Write(dummyData);
            stream.Position = 0;

            stream.ReadExact(buffer);

            Assert.True(dummyData.SequenceEqual(buffer));
        }

        [Fact]
        public void Should_Throw_If_Failed_To_Read_Exact_Bytes()
        {
            Span<byte> dummyData = stackalloc byte[4] { 0, 1, 2, 3 };

            using var stream = MemoryOwner<byte>.Allocate(4).AsStream();

            stream.Write(dummyData);
            stream.Position = 2;

            Assert.Throws<IOException>(() =>
            {
                Span<byte> buffer = stackalloc byte[4];

                stream.ReadExact(buffer);
            });
        }
    }
}
