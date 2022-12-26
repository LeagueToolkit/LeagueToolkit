using LeagueToolkit.Core.Memory;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueToolkit.Tests.Core.Memory
{
    public class MultiVertexBufferTests
    {
        public class ConstructorTests
        {
            [Fact]
            public void Should_Throw_If_Provided_Vertex_Buffers_Have_Overlapping_Elements()
            {
                VertexElement[] vertexBufferElements1 = new VertexElement[]
                {
                    VertexElement.POSITION,
                    VertexElement.NORMAL
                };
                VertexBuffer vertexBuffer1 = VertexBuffer.Create(
                    VertexElementGroupUsage.Static,
                    vertexBufferElements1,
                    VertexBuffer.AllocateForElements(vertexBufferElements1, 3)
                );

                VertexElement[] vertexBufferElements2 = new VertexElement[]
                {
                    VertexElement.NORMAL,
                    VertexElement.DIFFUSE_UV
                };
                VertexBuffer vertexBuffer2 = VertexBuffer.Create(
                    VertexElementGroupUsage.Static,
                    vertexBufferElements2,
                    VertexBuffer.AllocateForElements(vertexBufferElements2, 3)
                );

                Assert.Throws<ArgumentException>(() =>
                {
                    new MultiVertexBuffer(new VertexBuffer[] { vertexBuffer1, vertexBuffer2 });
                });
            }
        }

        public class GetAccessorTests
        {
            [Fact]
            public void Should_Return_A_Valid_Accessor_If_Element_Is_In_Vertex_Buffers()
            {
                MultiVertexBuffer multiVertexBuffer = CreateMultiVertexBuffer(
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
                MultiVertexBuffer multiVertexBuffer = CreateMultiVertexBuffer(
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
                MultiVertexBuffer multiVertexBuffer = CreateMultiVertexBuffer(
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

        private static MultiVertexBuffer CreateMultiVertexBuffer(
            IEnumerable<VertexElement> buffer1Elements,
            IEnumerable<VertexElement> buffer2Elements
        )
        {
            VertexBuffer vertexBuffer1 = VertexBuffer.Create(
                VertexElementGroupUsage.Static,
                buffer1Elements,
                VertexBuffer.AllocateForElements(buffer1Elements, 3)
            );
            VertexBuffer vertexBuffer2 = VertexBuffer.Create(
                VertexElementGroupUsage.Static,
                buffer2Elements,
                VertexBuffer.AllocateForElements(buffer2Elements, 3)
            );

            return new(new VertexBuffer[] { vertexBuffer1, vertexBuffer2 });
        }
    }
}
