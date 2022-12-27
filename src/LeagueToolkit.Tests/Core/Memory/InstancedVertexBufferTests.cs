using LeagueToolkit.Core.Memory;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueToolkit.Tests.Core.Memory
{
    public class InstancedVertexBufferTests
    {
        public class ConstructorTests
        {
            [Fact]
            public void Should_Create_A_Valid_InstancedVertexBuffer_Instance()
            {
                VertexElement[] elements1 = new[] { VertexElement.POSITION, VertexElement.NORMAL };
                VertexElement[] elements2 = new[] { VertexElement.DIFFUSE_UV };

                VertexBuffer vertexBuffer1 = VertexBuffer.Create(
                    VertexBufferUsage.Static,
                    elements1,
                    VertexBuffer.AllocateForElements(elements1, 3)
                );
                VertexBuffer vertexBuffer2 = VertexBuffer.Create(
                    VertexBufferUsage.Static,
                    elements2,
                    VertexBuffer.AllocateForElements(elements2, 3)
                );

                InstancedVertexBuffer multiVertexBuffer = new(new[] { vertexBuffer1, vertexBuffer2 });

                Assert.Equal(vertexBuffer1.VertexCount, multiVertexBuffer.VertexCount);
                Assert.Equal(vertexBuffer2.VertexCount, multiVertexBuffer.VertexCount);

                Assert.True(multiVertexBuffer.Buffers.SequenceEqual(new[] { vertexBuffer1, vertexBuffer2 }));
            }

            [Fact]
            public void Should_Throw_If_Vertex_Buffers_Have_Overlapping_Elements()
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    CreateMultiVertexBuffer(
                        new[] { VertexElement.POSITION, VertexElement.NORMAL },
                        new[] { VertexElement.NORMAL, VertexElement.DIFFUSE_UV }
                    );
                });
            }

            [Fact]
            public void Should_Throw_If_Vertex_Buffers_Have_Different_Vertex_Counts()
            {
                VertexElement[] elements1 = new[] { VertexElement.POSITION };
                VertexElement[] elements2 = new[] { VertexElement.NORMAL };

                VertexBuffer vertexBuffer1 = VertexBuffer.Create(
                    VertexBufferUsage.Static,
                    elements1,
                    VertexBuffer.AllocateForElements(elements1, 6)
                );
                VertexBuffer vertexBuffer2 = VertexBuffer.Create(
                    VertexBufferUsage.Static,
                    elements2,
                    VertexBuffer.AllocateForElements(elements2, 3)
                );

                Assert.Throws<ArgumentException>(() =>
                {
                    _ = new InstancedVertexBuffer(new[] { vertexBuffer1, vertexBuffer2 });
                });
            }
        }

        public class GetAccessorTests
        {
            [Fact]
            public void Should_Return_A_Valid_Accessor_If_Element_Is_In_Vertex_Buffers()
            {
                InstancedVertexBuffer multiVertexBuffer = CreateMultiVertexBuffer(
                    new VertexElement[] { VertexElement.POSITION, VertexElement.NORMAL, VertexElement.DIFFUSE_UV },
                    new VertexElement[] { VertexElement.BASE_COLOR }
                );

                VertexElementAccessor positionAccessor = multiVertexBuffer.GetAccessor(ElementName.Position);
                VertexElementAccessor baseColorAccessor = multiVertexBuffer.GetAccessor(ElementName.BaseColor);

                Assert.Equal(ElementName.Position, positionAccessor.Name);
                Assert.Equal(ElementName.BaseColor, baseColorAccessor.Name);

                Assert.Equal(32, positionAccessor.VertexStride);
                Assert.Equal(4, baseColorAccessor.VertexStride);
            }

            [Fact]
            public void Should_Throw_If_Element_Doesnt_Exist_In_Vertex_Buffers()
            {
                InstancedVertexBuffer multiVertexBuffer = CreateMultiVertexBuffer(
                    new VertexElement[] { VertexElement.POSITION },
                    new VertexElement[] { VertexElement.BASE_COLOR }
                );

                Assert.Throws<KeyNotFoundException>(() =>
                {
                    _ = multiVertexBuffer.GetAccessor(ElementName.Normal);
                });
            }
        }

        public class DisposeMethod
        {
            [Fact]
            public void Should_Dispose()
            {
                InstancedVertexBuffer multiVertexBuffer = CreateMultiVertexBuffer(
                    new VertexElement[] { VertexElement.POSITION },
                    new VertexElement[] { VertexElement.NORMAL }
                );

                multiVertexBuffer.Dispose();

                Assert.Throws<ObjectDisposedException>(() =>
                {
                    _ = multiVertexBuffer.GetAccessor(ElementName.Position);
                });
            }
        }

        private static InstancedVertexBuffer CreateMultiVertexBuffer(
            IEnumerable<VertexElement> buffer1Elements,
            IEnumerable<VertexElement> buffer2Elements
        )
        {
            VertexBuffer vertexBuffer1 = VertexBuffer.Create(
                VertexBufferUsage.Static,
                buffer1Elements,
                VertexBuffer.AllocateForElements(buffer1Elements, 3)
            );
            VertexBuffer vertexBuffer2 = VertexBuffer.Create(
                VertexBufferUsage.Static,
                buffer2Elements,
                VertexBuffer.AllocateForElements(buffer2Elements, 3)
            );

            return new(new VertexBuffer[] { vertexBuffer1, vertexBuffer2 });
        }
    }
}
