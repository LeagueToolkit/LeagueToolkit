using CommunityToolkit.HighPerformance.Buffers;
using LeagueToolkit.Core.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LeagueToolkit.Tests.Core.Memory
{
    public class VertexElementArrayTests
    {
        public class ConstructorTests
        {
            [Fact]
            public void Should_Return_A_Correctly_Initialized_VertexElementArray_Instance()
            {
                VertexElement[] elements = new VertexElement[]
                {
                    VertexElement.POSITION,
                    VertexElement.NORMAL,
                    VertexElement.DIFFUSE_UV
                };
                VertexBuffer vertexBuffer = VertexBuffer.Create(
                    VertexBufferUsage.Static,
                    elements,
                    VertexBuffer.AllocateForElements(elements, 3)
                );

                VertexElementArray<Vector3> positionArray = vertexBuffer
                    .GetAccessor(ElementName.Position)
                    .AsVector3Array();

                Assert.Equal(3, positionArray.Count);
            }
        }

        public class IndexerTests
        {
            [Fact]
            public void Should_Return_A_Correct_Value_For_The_Specified_Index()
            {
                VertexElement[] elements = new VertexElement[]
                {
                    VertexElement.POSITION,
                    VertexElement.NORMAL,
                    VertexElement.DIFFUSE_UV
                };

                MemoryOwner<byte> vertexBufferOwner = VertexBuffer.AllocateForElements(elements, 3);
                VertexBufferWriter vertexBufferWriter = new(elements, vertexBufferOwner.Memory);

                for (int i = 0; i < 3; i++)
                {
                    vertexBufferWriter.WriteVector3(i, ElementName.Position, new(i + 100, i + 100, i + 100));
                    vertexBufferWriter.WriteVector3(i, ElementName.Normal, new(i, i, i));
                    vertexBufferWriter.WriteVector2(i, ElementName.DiffuseUV, new(i + 200, i + 200));
                }

                VertexBuffer vertexBuffer = VertexBuffer.Create(VertexBufferUsage.Static, elements, vertexBufferOwner);

                VertexElementArray<Vector3> normalArray = vertexBuffer.GetAccessor(ElementName.Normal).AsVector3Array();
                VertexElementArray<Vector2> diffuseUvArray = vertexBuffer
                    .GetAccessor(ElementName.DiffuseUV)
                    .AsVector2Array();

                Assert.Equal(new(1, 1, 1), normalArray[1]);
                Assert.Equal(new(201, 201), diffuseUvArray[1]);
            }
        }
    }
}
