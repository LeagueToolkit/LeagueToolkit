using CommunityToolkit.HighPerformance.Buffers;
using LeagueToolkit.Core.Memory;
using LeagueToolkit.Core.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueToolkit.Tests.Core.Memory
{
    public class VertexBufferTests
    {
        public class CreateMethod
        {
            [Fact]
            public void Should_Return_A_Correctly_Initialized_VertexBuffer_Instance()
            {
                VertexElementGroupUsage usage = VertexElementGroupUsage.Static;
                VertexElement[] elements = new VertexElement[]
                {
                    VertexElement.POSITION,
                    VertexElement.NORMAL,
                    VertexElement.DIFFUSE_UV
                };
                VertexBuffer vertexBuffer = VertexBuffer.Create(
                    usage,
                    elements,
                    VertexBuffer.AllocateForElements(elements, 3)
                );

                Assert.Equal(usage, vertexBuffer.Usage);
                Assert.Equal(96, vertexBuffer.View.Length);
                Assert.Equal(32, vertexBuffer.Stride);

                // Test element offsets
                Assert.Equal(0, vertexBuffer.Elements[ElementName.Position].offset);
                Assert.Equal(12, vertexBuffer.Elements[ElementName.Normal].offset);
                Assert.Equal(24, vertexBuffer.Elements[ElementName.DiffuseUV].offset);
            }

            [Fact]
            public void Should_Throw_If_Passed_Duplicate_Vertex_Elements()
            {
                VertexElement[] elements = new VertexElement[] { VertexElement.POSITION, VertexElement.POSITION };

                Assert.Throws<ArgumentException>(() =>
                {
                    VertexBuffer vertexBuffer = VertexBuffer.Create(
                        VertexElementGroupUsage.Static,
                        elements,
                        VertexBuffer.AllocateForElements(elements, 3)
                    );
                });
            }
        }

        public class AllocateForElements
        {
            [Fact]
            public void Should_Return_Buffer_Of_Correct_Size()
            {
                MemoryOwner<byte> buffer = VertexBuffer.AllocateForElements(
                    new VertexElement[] { VertexElement.POSITION, VertexElement.NORMAL, VertexElement.DIFFUSE_UV },
                    3
                );

                Assert.Equal(96, buffer.Length);
            }

            [Fact]
            public void Should_Throw_If_Passed_Duplicate_Vertex_Elements()
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    VertexBuffer.AllocateForElements(
                        new VertexElement[] { VertexElement.POSITION, VertexElement.POSITION },
                        3
                    );
                });
            }
        }

        public class GetAccessorMethod
        {
            [Fact]
            public void Should_Return_Accessor_If_Element_Exists()
            {
                VertexElement[] vertexBufferElements = new VertexElement[]
                {
                    VertexElement.POSITION,
                    VertexElement.NORMAL,
                    VertexElement.DIFFUSE_UV
                };
                VertexBuffer vertexBuffer = VertexBuffer.Create(
                    VertexElementGroupUsage.Static,
                    vertexBufferElements,
                    VertexBuffer.AllocateForElements(vertexBufferElements, 3)
                );

                _ = vertexBuffer.GetAccessor(ElementName.Position);
                _ = vertexBuffer.GetAccessor(ElementName.Normal);
                _ = vertexBuffer.GetAccessor(ElementName.DiffuseUV);

                Assert.True(true);
            }

            [Fact]
            public void Should_Throw_If_Element_Cannot_Be_Found()
            {
                VertexElement[] vertexBufferElements = new VertexElement[] { VertexElement.POSITION };
                VertexBuffer vertexBuffer = VertexBuffer.Create(
                    VertexElementGroupUsage.Static,
                    vertexBufferElements,
                    VertexBuffer.AllocateForElements(vertexBufferElements, 3)
                );

                Assert.Throws<KeyNotFoundException>(() =>
                {
                    _ = vertexBuffer.GetAccessor(ElementName.Normal);
                });
            }
        }
    }
}
