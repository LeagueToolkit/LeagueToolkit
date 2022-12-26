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
                VertexElement[] vertexBufferElements1 = new VertexElement[]
                {
                    VertexElement.POSITION,
                    VertexElement.NORMAL,
                    VertexElement.DIFFUSE_UV
                };
                VertexBuffer vertexBuffer1 = VertexBuffer.Create(
                    VertexElementGroupUsage.Static,
                    vertexBufferElements1,
                    VertexBuffer.AllocateForElements(vertexBufferElements1, 3)
                );

                VertexElement[] vertexBufferElements2 = new VertexElement[] { VertexElement.BASE_COLOR };
                VertexBuffer vertexBuffer2 = VertexBuffer.Create(
                    VertexElementGroupUsage.Static,
                    vertexBufferElements2,
                    VertexBuffer.AllocateForElements(vertexBufferElements2, 3)
                );

                MultiVertexBuffer multiVertexBuffer = new(new VertexBuffer[] { vertexBuffer1, vertexBuffer2 });

                VertexElementAccessor positionAccessor = multiVertexBuffer.GetAccessor(ElementName.Position);
                VertexElementAccessor baseColorAccessor = multiVertexBuffer.GetAccessor(ElementName.BaseColor);

                Assert.Equal(ElementName.Position, positionAccessor.Name);
                Assert.Equal(ElementName.BaseColor, baseColorAccessor.Name);

                Assert.Equal(vertexBuffer1.View, positionAccessor.BufferView);
                Assert.Equal(vertexBuffer2.View, baseColorAccessor.BufferView);
            }
        }
    }
}
