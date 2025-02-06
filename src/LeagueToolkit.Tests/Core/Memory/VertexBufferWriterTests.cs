using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.HighPerformance.Buffers;
using LeagueToolkit.Core.Memory;
using LeagueToolkit.Core.Primitives;

namespace LeagueToolkit.Tests.Core.Memory
{
    public class VertexBufferWriterTests
    {
        public class ConstructorTests
        {
            [Fact]
            public void Should_Return_A_Correctly_Initialized_VertexBufferWriter_Instance()
            {
                VertexElement[] elements = new VertexElement[]
                {
                    VertexElement.POSITION,
                    VertexElement.NORMAL,
                    VertexElement.TEXCOORD_0
                };
                MemoryOwner<byte> vertexBufferOwner = VertexBuffer.AllocateForElements(elements, 3);
                VertexBufferWriter vertexBufferWriter = new(elements, vertexBufferOwner.Memory);

                Assert.Equal(VertexElement.POSITION, vertexBufferWriter.Elements[ElementName.Position].Element);
                Assert.Equal(VertexElement.NORMAL, vertexBufferWriter.Elements[ElementName.Normal].Element);
                Assert.Equal(VertexElement.TEXCOORD_0, vertexBufferWriter.Elements[ElementName.Texcoord0].Element);

                Assert.Equal(0, vertexBufferWriter.Elements[ElementName.Position].Offset);
                Assert.Equal(12, vertexBufferWriter.Elements[ElementName.Normal].Offset);
                Assert.Equal(24, vertexBufferWriter.Elements[ElementName.Texcoord0].Offset);

                Assert.Equal(32, vertexBufferWriter.VertexStride);
            }
        }

        #region Writing
        public class WriteFloatMethod
        {
            [Fact]
            public void Should_Write_The_Value()
            {
                VertexElement[] elements = new VertexElement[] { VertexElement.FOG_COORDINATE };
                MemoryOwner<byte> vertexBufferOwner = VertexBuffer.AllocateForElements(elements, 3);
                VertexBufferWriter vertexBufferWriter = new(elements, vertexBufferOwner.Memory);

                vertexBufferWriter.WriteFloat(0, ElementName.FogCoordinate, 0f);
                vertexBufferWriter.WriteFloat(1, ElementName.FogCoordinate, 1f);
                vertexBufferWriter.WriteFloat(2, ElementName.FogCoordinate, 2f);

                Assert.Equal(0f, MemoryMarshal.Read<float>(vertexBufferOwner.Span[0..]));
                Assert.Equal(1f, MemoryMarshal.Read<float>(vertexBufferOwner.Span[4..]));
                Assert.Equal(2f, MemoryMarshal.Read<float>(vertexBufferOwner.Span[8..]));
            }
        }

        public class WriteVector2Method
        {
            [Fact]
            public void Should_Write_The_Value()
            {
                VertexElement[] elements = new VertexElement[] { VertexElement.TEXCOORD_0 };
                MemoryOwner<byte> vertexBufferOwner = VertexBuffer.AllocateForElements(elements, 3);
                VertexBufferWriter vertexBufferWriter = new(elements, vertexBufferOwner.Memory);

                vertexBufferWriter.WriteVector2(0, ElementName.Texcoord0, new(0f, 0f));
                vertexBufferWriter.WriteVector2(1, ElementName.Texcoord0, new(1f, 1f));
                vertexBufferWriter.WriteVector2(2, ElementName.Texcoord0, new(2f, 2f));

                Assert.Equal(new(0f, 0f), MemoryMarshal.Read<Vector2>(vertexBufferOwner.Span[0..]));
                Assert.Equal(new(1f, 1f), MemoryMarshal.Read<Vector2>(vertexBufferOwner.Span[8..]));
                Assert.Equal(new(2f, 2f), MemoryMarshal.Read<Vector2>(vertexBufferOwner.Span[16..]));
            }
        }

        public class WriteVector3Method
        {
            [Fact]
            public void Should_Write_The_Value()
            {
                VertexElement[] elements = new VertexElement[] { VertexElement.POSITION };
                MemoryOwner<byte> vertexBufferOwner = VertexBuffer.AllocateForElements(elements, 3);
                VertexBufferWriter vertexBufferWriter = new(elements, vertexBufferOwner.Memory);

                vertexBufferWriter.WriteVector3(0, ElementName.Position, new(0f, 0f, 0f));
                vertexBufferWriter.WriteVector3(1, ElementName.Position, new(1f, 1f, 1f));
                vertexBufferWriter.WriteVector3(2, ElementName.Position, new(2f, 2f, 2f));

                Assert.Equal(new(0f, 0f, 0f), MemoryMarshal.Read<Vector3>(vertexBufferOwner.Span[0..]));
                Assert.Equal(new(1f, 1f, 1f), MemoryMarshal.Read<Vector3>(vertexBufferOwner.Span[12..]));
                Assert.Equal(new(2f, 2f, 2f), MemoryMarshal.Read<Vector3>(vertexBufferOwner.Span[24..]));
            }
        }

        public class WriteVector4Method
        {
            [Fact]
            public void Should_Write_The_Value()
            {
                VertexElement[] elements = new VertexElement[] { VertexElement.BLEND_WEIGHT };
                MemoryOwner<byte> vertexBufferOwner = VertexBuffer.AllocateForElements(elements, 3);
                VertexBufferWriter vertexBufferWriter = new(elements, vertexBufferOwner.Memory);

                vertexBufferWriter.WriteVector4(0, ElementName.BlendWeight, new(0f, 0f, 0f, 0f));
                vertexBufferWriter.WriteVector4(1, ElementName.BlendWeight, new(1f, 1f, 1f, 1f));
                vertexBufferWriter.WriteVector4(2, ElementName.BlendWeight, new(2f, 2f, 2f, 2f));

                Assert.Equal(new(0f, 0f, 0f, 0f), MemoryMarshal.Read<Vector4>(vertexBufferOwner.Span[0..]));
                Assert.Equal(new(1f, 1f, 1f, 1f), MemoryMarshal.Read<Vector4>(vertexBufferOwner.Span[16..]));
                Assert.Equal(new(2f, 2f, 2f, 2f), MemoryMarshal.Read<Vector4>(vertexBufferOwner.Span[32..]));
            }
        }

        public class WriteColorBgraU8Method
        {
            [Fact]
            public void Should_Write_The_Value()
            {
                VertexElement[] elements = new VertexElement[]
                {
                    new(ElementName.PrimaryColor, ElementFormat.BGRA_Packed8888)
                };
                MemoryOwner<byte> vertexBufferOwner = VertexBuffer.AllocateForElements(elements, 3);
                VertexBufferWriter vertexBufferWriter = new(elements, vertexBufferOwner.Memory);

                vertexBufferWriter.WriteColorBgraU8(0, ElementName.PrimaryColor, new(0, 1, 2, 3));
                vertexBufferWriter.WriteColorBgraU8(1, ElementName.PrimaryColor, new(4, 5, 6, 7));
                vertexBufferWriter.WriteColorBgraU8(2, ElementName.PrimaryColor, new(8, 9, 10, 11));

                Assert.Equal(new(0, 1, 2, 3), Color.Read(vertexBufferOwner.Span[0..], ColorFormat.BgraU8));
                Assert.Equal(new(4, 5, 6, 7), Color.Read(vertexBufferOwner.Span[4..], ColorFormat.BgraU8));
                Assert.Equal(new(8, 9, 10, 11), Color.Read(vertexBufferOwner.Span[8..], ColorFormat.BgraU8));
            }
        }

        public class WriteColorRgbaU8Method
        {
            [Fact]
            public void Should_Write_The_Value()
            {
                VertexElement[] elements = new VertexElement[]
                {
                    new(ElementName.PrimaryColor, ElementFormat.RGBA_Packed8888)
                };
                MemoryOwner<byte> vertexBufferOwner = VertexBuffer.AllocateForElements(elements, 3);
                VertexBufferWriter vertexBufferWriter = new(elements, vertexBufferOwner.Memory);

                vertexBufferWriter.WriteColorRgbaU8(0, ElementName.PrimaryColor, new(0, 1, 2, 3));
                vertexBufferWriter.WriteColorRgbaU8(1, ElementName.PrimaryColor, new(4, 5, 6, 7));
                vertexBufferWriter.WriteColorRgbaU8(2, ElementName.PrimaryColor, new(8, 9, 10, 11));

                Assert.Equal(new(0, 1, 2, 3), Color.Read(vertexBufferOwner.Span[0..], ColorFormat.RgbaU8));
                Assert.Equal(new(4, 5, 6, 7), Color.Read(vertexBufferOwner.Span[4..], ColorFormat.RgbaU8));
                Assert.Equal(new(8, 9, 10, 11), Color.Read(vertexBufferOwner.Span[8..], ColorFormat.RgbaU8));
            }
        }

        public class WriteZyxwU8Method
        {
            [Fact]
            public void Should_Write_The_Value()
            {
                VertexElement[] elements = new VertexElement[]
                {
                    new(ElementName.BlendIndex, ElementFormat.ZYXW_Packed8888)
                };
                MemoryOwner<byte> vertexBufferOwner = VertexBuffer.AllocateForElements(elements, 3);
                VertexBufferWriter vertexBufferWriter = new(elements, vertexBufferOwner.Memory);

                vertexBufferWriter.WriteZyxwU8(0, ElementName.BlendIndex, new(0, 1, 2, 3));
                vertexBufferWriter.WriteZyxwU8(1, ElementName.BlendIndex, new(4, 5, 6, 7));
                vertexBufferWriter.WriteZyxwU8(2, ElementName.BlendIndex, new(8, 9, 10, 11));

                Assert.Equal(
                    new(0, 1, 2, 3),
                    MemoryMarshal.Read<(byte z, byte y, byte x, byte w)>(vertexBufferOwner.Span[0..])
                );
                Assert.Equal(
                    new(4, 5, 6, 7),
                    MemoryMarshal.Read<(byte z, byte y, byte x, byte w)>(vertexBufferOwner.Span[4..])
                );
                Assert.Equal(
                    new(8, 9, 10, 11),
                    MemoryMarshal.Read<(byte z, byte y, byte x, byte w)>(vertexBufferOwner.Span[8..])
                );
            }
        }

        public class WriteXyzwU8Method
        {
            [Fact]
            public void Should_Write_The_Value()
            {
                VertexElement[] elements = new VertexElement[]
                {
                    new(ElementName.BlendIndex, ElementFormat.XYZW_Packed8888)
                };
                MemoryOwner<byte> vertexBufferOwner = VertexBuffer.AllocateForElements(elements, 3);
                VertexBufferWriter vertexBufferWriter = new(elements, vertexBufferOwner.Memory);

                vertexBufferWriter.WriteXyzwU8(0, ElementName.BlendIndex, new(0, 1, 2, 3));
                vertexBufferWriter.WriteXyzwU8(1, ElementName.BlendIndex, new(4, 5, 6, 7));
                vertexBufferWriter.WriteXyzwU8(2, ElementName.BlendIndex, new(8, 9, 10, 11));

                Assert.Equal(
                    new(0, 1, 2, 3),
                    MemoryMarshal.Read<(byte x, byte y, byte z, byte w)>(vertexBufferOwner.Span[0..])
                );
                Assert.Equal(
                    new(4, 5, 6, 7),
                    MemoryMarshal.Read<(byte x, byte y, byte z, byte w)>(vertexBufferOwner.Span[4..])
                );
                Assert.Equal(
                    new(8, 9, 10, 11),
                    MemoryMarshal.Read<(byte x, byte y, byte z, byte w)>(vertexBufferOwner.Span[8..])
                );
            }
        }
        #endregion
    }
}
