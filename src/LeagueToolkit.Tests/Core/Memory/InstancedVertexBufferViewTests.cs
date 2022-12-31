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
