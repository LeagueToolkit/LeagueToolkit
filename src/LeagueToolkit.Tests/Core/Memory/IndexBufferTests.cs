using CommunityToolkit.HighPerformance.Buffers;
using LeagueToolkit.Core.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueToolkit.Tests.Core.Memory;

public class IndexBufferTests
{
    public class CreateTests
    {
        [Fact]
        public void Should_Create_An_IndexBuffer()
        {
            MemoryOwner<byte> bufferOwner = MemoryOwner<byte>.Allocate(6);
            using IndexBuffer buffer = IndexBuffer.Create(IndexFormat.U16, bufferOwner);

            Assert.Equal(IndexFormat.U16, buffer.Format);
            Assert.Equal(3, buffer.Count);
            Assert.Equal(2, buffer.Stride);
        }

        [Fact]
        public void Should_Throw_If_Buffer_Size_Is_Not_Multiple_Of_Format_Size()
        {
            MemoryOwner<byte> bufferOwner = MemoryOwner<byte>.Allocate(1);

            Assert.Throws<ArgumentException>(() => IndexBuffer.Create(IndexFormat.U16, bufferOwner));
        }
    }

    public class GetFormatSizeTests
    {
        [Fact]
        public void Should_Return_Sizeof_Ushort_For_U16()
        {
            Assert.Equal(sizeof(ushort), IndexBuffer.GetFormatSize(IndexFormat.U16));
        }

        [Fact]
        public void Should_Return_Sizeof_Uint_For_U32()
        {
            Assert.Equal(sizeof(uint), IndexBuffer.GetFormatSize(IndexFormat.U32));
        }

        [Fact]
        public void Should_Throw_Exception_For_Invalid_Format()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => IndexBuffer.GetFormatSize((IndexFormat)int.MaxValue));
        }
    }
}
