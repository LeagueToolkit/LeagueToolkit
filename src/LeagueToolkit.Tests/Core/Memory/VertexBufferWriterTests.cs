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
    }
}
