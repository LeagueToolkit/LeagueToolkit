using CommunityToolkit.HighPerformance.Buffers;
using LeagueToolkit.Core.Memory;
using LeagueToolkit.Core.Renderer;
using LeagueToolkit.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LeagueToolkit.Tests.Core.Memory
{
    public class VertexBufferWriterTests
    {
        public class ConstructorTests
        {
            [Fact]
            public void Should_Return_A_Correctly_Initialized_VertexBufferWriter_Instance()
            {
                VertexElementGroupUsage usage = VertexElementGroupUsage.Static;
                VertexElement[] elements = new VertexElement[]
                {
                    VertexElement.POSITION,
                    VertexElement.NORMAL,
                    VertexElement.DIFFUSE_UV
                };
                MemoryOwner<byte> vertexBufferOwner = VertexBuffer.AllocateForElements(elements, 3);
                VertexBufferWriter vertexBufferWriter = new(usage, elements, vertexBufferOwner.Memory);

                Assert.Equal(usage, vertexBufferWriter.Usage);

                Assert.Equal(VertexElement.POSITION, vertexBufferWriter.Elements[ElementName.Position].Element);
                Assert.Equal(VertexElement.NORMAL, vertexBufferWriter.Elements[ElementName.Normal].Element);
                Assert.Equal(VertexElement.DIFFUSE_UV, vertexBufferWriter.Elements[ElementName.DiffuseUV].Element);

                Assert.Equal(0, vertexBufferWriter.Elements[ElementName.Position].Offset);
                Assert.Equal(12, vertexBufferWriter.Elements[ElementName.Normal].Offset);
                Assert.Equal(24, vertexBufferWriter.Elements[ElementName.DiffuseUV].Offset);

                Assert.Equal(32, vertexBufferWriter.Stride);
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
                VertexBufferWriter vertexBufferWriter =
                    new(VertexElementGroupUsage.Static, elements, vertexBufferOwner.Memory);

                vertexBufferWriter.WriteFloat(0, ElementName.FogCoordinate, 0f);
                vertexBufferWriter.WriteFloat(1, ElementName.FogCoordinate, 1f);
                vertexBufferWriter.WriteFloat(2, ElementName.FogCoordinate, 2f);

                Assert.Equal(0f, MemoryMarshal.Read<float>(vertexBufferWriter.Buffer[0..].Span));
                Assert.Equal(1f, MemoryMarshal.Read<float>(vertexBufferWriter.Buffer[4..].Span));
                Assert.Equal(2f, MemoryMarshal.Read<float>(vertexBufferWriter.Buffer[8..].Span));
            }
        }

        public class WriteVector2Method
        {
            [Fact]
            public void Should_Write_The_Value()
            {
                VertexElement[] elements = new VertexElement[] { VertexElement.DIFFUSE_UV };
                MemoryOwner<byte> vertexBufferOwner = VertexBuffer.AllocateForElements(elements, 3);
                VertexBufferWriter vertexBufferWriter =
                    new(VertexElementGroupUsage.Static, elements, vertexBufferOwner.Memory);

                vertexBufferWriter.WriteVector2(0, ElementName.DiffuseUV, new(0f, 0f));
                vertexBufferWriter.WriteVector2(1, ElementName.DiffuseUV, new(1f, 1f));
                vertexBufferWriter.WriteVector2(2, ElementName.DiffuseUV, new(2f, 2f));

                Assert.Equal(new(0f, 0f), MemoryMarshal.Read<Vector2>(vertexBufferWriter.Buffer[0..].Span));
                Assert.Equal(new(1f, 1f), MemoryMarshal.Read<Vector2>(vertexBufferWriter.Buffer[8..].Span));
                Assert.Equal(new(2f, 2f), MemoryMarshal.Read<Vector2>(vertexBufferWriter.Buffer[16..].Span));
            }
        }

        public class WriteVector3Method
        {
            [Fact]
            public void Should_Write_The_Value()
            {
                VertexElement[] elements = new VertexElement[] { VertexElement.POSITION };
                MemoryOwner<byte> vertexBufferOwner = VertexBuffer.AllocateForElements(elements, 3);
                VertexBufferWriter vertexBufferWriter =
                    new(VertexElementGroupUsage.Static, elements, vertexBufferOwner.Memory);

                vertexBufferWriter.WriteVector3(0, ElementName.Position, new(0f, 0f, 0f));
                vertexBufferWriter.WriteVector3(1, ElementName.Position, new(1f, 1f, 1f));
                vertexBufferWriter.WriteVector3(2, ElementName.Position, new(2f, 2f, 2f));

                Assert.Equal(new(0f, 0f, 0f), MemoryMarshal.Read<Vector3>(vertexBufferWriter.Buffer[0..].Span));
                Assert.Equal(new(1f, 1f, 1f), MemoryMarshal.Read<Vector3>(vertexBufferWriter.Buffer[12..].Span));
                Assert.Equal(new(2f, 2f, 2f), MemoryMarshal.Read<Vector3>(vertexBufferWriter.Buffer[24..].Span));
            }
        }

        public class WriteVector4Method
        {
            [Fact]
            public void Should_Write_The_Value()
            {
                VertexElement[] elements = new VertexElement[] { VertexElement.BLEND_WEIGHT };
                MemoryOwner<byte> vertexBufferOwner = VertexBuffer.AllocateForElements(elements, 3);
                VertexBufferWriter vertexBufferWriter =
                    new(VertexElementGroupUsage.Static, elements, vertexBufferOwner.Memory);

                vertexBufferWriter.WriteVector4(0, ElementName.BlendWeight, new(0f, 0f, 0f, 0f));
                vertexBufferWriter.WriteVector4(1, ElementName.BlendWeight, new(1f, 1f, 1f, 1f));
                vertexBufferWriter.WriteVector4(2, ElementName.BlendWeight, new(2f, 2f, 2f, 2f));

                Assert.Equal(new(0f, 0f, 0f, 0f), MemoryMarshal.Read<Vector4>(vertexBufferWriter.Buffer[0..].Span));
                Assert.Equal(new(1f, 1f, 1f, 1f), MemoryMarshal.Read<Vector4>(vertexBufferWriter.Buffer[16..].Span));
                Assert.Equal(new(2f, 2f, 2f, 2f), MemoryMarshal.Read<Vector4>(vertexBufferWriter.Buffer[32..].Span));
            }
        }

        public class WriteColorBgraU8Method
        {
            [Fact]
            public void Should_Write_The_Value()
            {
                VertexElement[] elements = new VertexElement[]
                {
                    new(ElementName.BaseColor, ElementFormat.BGRA_Packed8888)
                };
                MemoryOwner<byte> vertexBufferOwner = VertexBuffer.AllocateForElements(elements, 3);
                VertexBufferWriter vertexBufferWriter =
                    new(VertexElementGroupUsage.Static, elements, vertexBufferOwner.Memory);

                vertexBufferWriter.WriteColorBgraU8(0, ElementName.BaseColor, new(0, 1, 2, 3));
                vertexBufferWriter.WriteColorBgraU8(1, ElementName.BaseColor, new(4, 5, 6, 7));
                vertexBufferWriter.WriteColorBgraU8(2, ElementName.BaseColor, new(8, 9, 10, 11));

                Assert.Equal(new(0, 1, 2, 3), Color.Read(vertexBufferWriter.Buffer[0..].Span, ColorFormat.BgraU8));
                Assert.Equal(new(4, 5, 6, 7), Color.Read(vertexBufferWriter.Buffer[4..].Span, ColorFormat.BgraU8));
                Assert.Equal(new(8, 9, 10, 11), Color.Read(vertexBufferWriter.Buffer[8..].Span, ColorFormat.BgraU8));
            }
        }

        public class WriteColorRgbaU8Method
        {
            [Fact]
            public void Should_Write_The_Value()
            {
                VertexElement[] elements = new VertexElement[]
                {
                    new(ElementName.BaseColor, ElementFormat.RGBA_Packed8888)
                };
                MemoryOwner<byte> vertexBufferOwner = VertexBuffer.AllocateForElements(elements, 3);
                VertexBufferWriter vertexBufferWriter =
                    new(VertexElementGroupUsage.Static, elements, vertexBufferOwner.Memory);

                vertexBufferWriter.WriteColorRgbaU8(0, ElementName.BaseColor, new(0, 1, 2, 3));
                vertexBufferWriter.WriteColorRgbaU8(1, ElementName.BaseColor, new(4, 5, 6, 7));
                vertexBufferWriter.WriteColorRgbaU8(2, ElementName.BaseColor, new(8, 9, 10, 11));

                Assert.Equal(new(0, 1, 2, 3), Color.Read(vertexBufferWriter.Buffer[0..].Span, ColorFormat.RgbaU8));
                Assert.Equal(new(4, 5, 6, 7), Color.Read(vertexBufferWriter.Buffer[4..].Span, ColorFormat.RgbaU8));
                Assert.Equal(new(8, 9, 10, 11), Color.Read(vertexBufferWriter.Buffer[8..].Span, ColorFormat.RgbaU8));
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
                VertexBufferWriter vertexBufferWriter =
                    new(VertexElementGroupUsage.Static, elements, vertexBufferOwner.Memory);

                vertexBufferWriter.WriteZyxwU8(0, ElementName.BlendIndex, new(0, 1, 2, 3));
                vertexBufferWriter.WriteZyxwU8(1, ElementName.BlendIndex, new(4, 5, 6, 7));
                vertexBufferWriter.WriteZyxwU8(2, ElementName.BlendIndex, new(8, 9, 10, 11));

                Assert.Equal(
                    new(0, 1, 2, 3),
                    MemoryMarshal.Read<(byte z, byte y, byte x, byte w)>(vertexBufferWriter.Buffer[0..].Span)
                );
                Assert.Equal(
                    new(4, 5, 6, 7),
                    MemoryMarshal.Read<(byte z, byte y, byte x, byte w)>(vertexBufferWriter.Buffer[4..].Span)
                );
                Assert.Equal(
                    new(8, 9, 10, 11),
                    MemoryMarshal.Read<(byte z, byte y, byte x, byte w)>(vertexBufferWriter.Buffer[8..].Span)
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
                VertexBufferWriter vertexBufferWriter =
                    new(VertexElementGroupUsage.Static, elements, vertexBufferOwner.Memory);

                vertexBufferWriter.WriteXyzwU8(0, ElementName.BlendIndex, new(0, 1, 2, 3));
                vertexBufferWriter.WriteXyzwU8(1, ElementName.BlendIndex, new(4, 5, 6, 7));
                vertexBufferWriter.WriteXyzwU8(2, ElementName.BlendIndex, new(8, 9, 10, 11));

                Assert.Equal(
                    new(0, 1, 2, 3),
                    MemoryMarshal.Read<(byte x, byte y, byte z, byte w)>(vertexBufferWriter.Buffer[0..].Span)
                );
                Assert.Equal(
                    new(4, 5, 6, 7),
                    MemoryMarshal.Read<(byte x, byte y, byte z, byte w)>(vertexBufferWriter.Buffer[4..].Span)
                );
                Assert.Equal(
                    new(8, 9, 10, 11),
                    MemoryMarshal.Read<(byte x, byte y, byte z, byte w)>(vertexBufferWriter.Buffer[8..].Span)
                );
            }
        }
        #endregion
    }
}
