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

                Assert.Throws<ArgumentException>(() =>
                {
                    _ = vertexBuffer.GetAccessor(ElementName.Normal);
                });
            }
        }
    }
}
