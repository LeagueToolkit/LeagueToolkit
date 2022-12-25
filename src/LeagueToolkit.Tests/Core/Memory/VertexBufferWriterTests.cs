using CommunityToolkit.HighPerformance.Buffers;
using LeagueToolkit.Core.Memory;
using LeagueToolkit.Core.Renderer;
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

                Assert.Equal(VertexElement.POSITION, vertexBufferWriter.Elements[ElementName.Position].element);
                Assert.Equal(VertexElement.NORMAL, vertexBufferWriter.Elements[ElementName.Normal].element);
                Assert.Equal(VertexElement.DIFFUSE_UV, vertexBufferWriter.Elements[ElementName.DiffuseUV].element);

                Assert.Equal(0, vertexBufferWriter.Elements[ElementName.Position].offset);
                Assert.Equal(12, vertexBufferWriter.Elements[ElementName.Normal].offset);
                Assert.Equal(24, vertexBufferWriter.Elements[ElementName.DiffuseUV].offset);

                Assert.Equal(32, vertexBufferWriter.Stride);
            }
        }

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
    }
}
