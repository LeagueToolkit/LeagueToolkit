using LeagueToolkit.Core.Memory;

namespace LeagueToolkit.Tests.Core.Memory
{
    public class VertexElementTests
    {
        public class EqualOperatorTests
        {
            [Fact]
            public void Should_Return_False_If_Name_Or_Format_Are_Different() =>
                Assert.False(VertexElement.POSITION == VertexElement.NORMAL);
        }

        public class NotEqualOperatorTests
        {
            [Fact]
            public void Should_Return_True_If_Name_Or_Format_Are_Different() =>
                Assert.True(VertexElement.POSITION != VertexElement.NORMAL);
        }
    }
}
