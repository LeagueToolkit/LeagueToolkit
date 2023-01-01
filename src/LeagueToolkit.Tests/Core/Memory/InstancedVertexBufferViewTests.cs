using LeagueToolkit.Core.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueToolkit.Tests.Core.Memory
{
    public class InstancedVertexBufferViewTests
    {
        public class ConstructorTests
        {
            [Fact]
            public void Should_Create_A_New_Instance()
            {
                VertexElement[] elements1 = new[] { VertexElement.POSITION, VertexElement.NORMAL };
                VertexElement[] elements2 = new[] { VertexElement.DIFFUSE_UV };
                int vertexCount = 3;
                var (buffer1, buffer2) = CreateVertexBuffers(elements1, elements2, vertexCount);

                InstancedVertexBufferView instanced = new(vertexCount, new[] { buffer1, buffer2 });

                Assert.Equal(vertexCount, instanced.VertexCount);
                Assert.Collection(
                    instanced.Buffers,
                    bufferInspector => Assert.Same(bufferInspector, buffer1),
                    bufferInspector => Assert.Same(bufferInspector, buffer2)
                );
            }

            [Fact]
            public void Should_Throw_If_Elements_Of_Buffers_Overlap()
            {
                VertexElement[] elements1 = new[] { VertexElement.POSITION, VertexElement.NORMAL };
                VertexElement[] elements2 = new[] { VertexElement.NORMAL };
                int vertexCount = 3;
                var (buffer1, buffer2) = CreateVertexBuffers(elements1, elements2, vertexCount);

                Assert.Throws<ArgumentException>(() =>
                {
                    _ = new InstancedVertexBufferView(vertexCount, new[] { buffer1, buffer2 });
                });
            }

            [Fact]
            public void Should_Throw_If_Buffers_Have_Different_Vertex_Counts()
            {
                VertexElement[] elements1 = new[] { VertexElement.POSITION };
                VertexElement[] elements2 = new[] { VertexElement.NORMAL };

                VertexBuffer vertexBuffer1 = VertexBuffer.Create(
                    VertexBufferUsage.Static,
                    elements1,
                    VertexBuffer.AllocateForElements(elements1, 3)
                );
                VertexBuffer vertexBuffer2 = VertexBuffer.Create(
                    VertexBufferUsage.Static,
                    elements2,
                    VertexBuffer.AllocateForElements(elements2, 6)
                );

                Assert.Throws<ArgumentException>(() =>
                {
                    _ = new InstancedVertexBufferView(3, new[] { vertexBuffer1, vertexBuffer2 });
                });
            }
        }

        public class GetAccessorTests
        {
            [Fact]
            public void Should_Return_Accessor_If_Element_Is_In_Vertex_Buffers()
            {
                var (buffer1, buffer2) = CreateVertexBuffers(
                    new VertexElement[] { VertexElement.POSITION, VertexElement.NORMAL, VertexElement.DIFFUSE_UV },
                    new VertexElement[] { VertexElement.BASE_COLOR },
                    3
                );
                InstancedVertexBufferView instanced = new(3, new[] { buffer1, buffer2 });

                VertexElementAccessor positionAccessor = instanced.GetAccessor(ElementName.Position);
                VertexElementAccessor baseColorAccessor = instanced.GetAccessor(ElementName.BaseColor);

                Assert.Equal(VertexElement.POSITION, positionAccessor.Element);
                Assert.Equal(VertexElement.BASE_COLOR, baseColorAccessor.Element);

                Assert.Equal(32, positionAccessor.VertexStride);
                Assert.Equal(4, baseColorAccessor.VertexStride);
            }

            [Fact]
            public void Should_Throw_If_Element_Doesnt_Exist_In_Vertex_Buffers()
            {
                var (buffer1, buffer2) = CreateVertexBuffers(
                    new VertexElement[] { VertexElement.POSITION },
                    new VertexElement[] { VertexElement.BASE_COLOR },
                    3
                );
                InstancedVertexBufferView instanced = new(3, new[] { buffer1, buffer2 });

                Assert.Throws<KeyNotFoundException>(() =>
                {
                    _ = instanced.GetAccessor(ElementName.Normal);
                });
            }
        }

        public class TryGetAccessorTests
        {
            [Fact]
            public void Should_Return_True_And_Accessor_If_Element_Is_In_Vertex_Buffers()
            {
                var (buffer1, buffer2) = CreateVertexBuffers(
                    new VertexElement[] { VertexElement.POSITION, VertexElement.NORMAL, VertexElement.DIFFUSE_UV },
                    new VertexElement[] { VertexElement.BASE_COLOR },
                    3
                );
                InstancedVertexBufferView instanced = new(3, new[] { buffer1, buffer2 });

                bool hasPosition = instanced.TryGetAccessor(ElementName.Position, out var positionAccessor);
                bool hasBaseColor = instanced.TryGetAccessor(ElementName.BaseColor, out var baseColorAccessor);

                Assert.True(hasPosition && hasBaseColor);

                Assert.Equal(VertexElement.POSITION, positionAccessor.Element);
                Assert.Equal(VertexElement.BASE_COLOR, baseColorAccessor.Element);

                Assert.Equal(32, positionAccessor.VertexStride);
                Assert.Equal(4, baseColorAccessor.VertexStride);
            }

            [Fact]
            public void Should_Return_False_If_Element_Doesnt_Exist_In_Vertex_Buffers()
            {
                var (buffer1, buffer2) = CreateVertexBuffers(
                    new VertexElement[] { VertexElement.POSITION },
                    new VertexElement[] { VertexElement.BASE_COLOR },
                    3
                );
                InstancedVertexBufferView instanced = new(3, new[] { buffer1, buffer2 });

                bool result = instanced.TryGetAccessor(ElementName.Normal, out var accessor);

                Assert.False(result);
                Assert.Equal(default, accessor);
            }
        }

        private static (VertexBuffer buffer1, VertexBuffer buffer2) CreateVertexBuffers(
            IEnumerable<VertexElement> buffer1Elements,
            IEnumerable<VertexElement> buffer2Elements,
            int vertexCount
        )
        {
            VertexBuffer vertexBuffer1 = VertexBuffer.Create(
                VertexBufferUsage.Static,
                buffer1Elements,
                VertexBuffer.AllocateForElements(buffer1Elements, vertexCount)
            );
            VertexBuffer vertexBuffer2 = VertexBuffer.Create(
                VertexBufferUsage.Static,
                buffer2Elements,
                VertexBuffer.AllocateForElements(buffer2Elements, vertexCount)
            );

            return (vertexBuffer1, vertexBuffer2);
        }
    }
}
