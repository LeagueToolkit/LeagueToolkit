using LeagueToolkit.Hashing;
using System.IO.Hashing;

namespace LeagueToolkit.Tests.Hashing;

public class XxHash64ExtTests
{
    public class HashTests 
    {
        [Fact]
        public void Should_Return_Correct_Hash_When_Source_Is_Smaller_Than_512() {
            byte[] sourceUtf8 = new byte[256];
            char[] sourceUtf16 = new char[256];

            Array.Fill(sourceUtf8, (byte)'A');
            Array.Fill(sourceUtf16, 'A');

            Assert.Equal(XxHash64.HashToUInt64(sourceUtf8), XxHash64Ext.Hash(sourceUtf16));
        }

        [Fact]
        public void Should_Return_Correct_Hash_When_Source_Is_Larger_Than_512() 
        {
            byte[] sourceUtf8 = new byte[1024];
            char[] sourceUtf16 = new char[1024];

            Array.Fill(sourceUtf8, (byte)'A');
            Array.Fill(sourceUtf16, 'A');

            Assert.Equal(XxHash64.HashToUInt64(sourceUtf8), XxHash64Ext.Hash(sourceUtf16));
        }
    }
}
