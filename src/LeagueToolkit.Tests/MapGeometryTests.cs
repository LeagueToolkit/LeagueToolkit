using LeagueToolkit.IO.MapGeometry;

namespace LeagueToolkit.Tests
{
    public class MapGeometryTests { }

    public class MapGeometryVertexElementGroupTests
    {
        [Fact]
        public void TestIEquatable_Equal()
        {
            MapGeometryVertexElementGroup a =
                new(
                    MapGeometryVertexElementGroupUsage.Static,
                    new List<MapGeometryVertexElement>()
                    {
                        new(MapGeometryVertexElementName.Position, MapGeometryVertexElementFormat.XYZ_Float32)
                    }
                );
            MapGeometryVertexElementGroup b =
                new(
                    MapGeometryVertexElementGroupUsage.Static,
                    new List<MapGeometryVertexElement>()
                    {
                        new(MapGeometryVertexElementName.Position, MapGeometryVertexElementFormat.XYZ_Float32)
                    }
                );

            Assert.True(a.Equals(b));
        }

        [Fact]
        public void TestIEquatable_UsageMismatch()
        {
            MapGeometryVertexElementGroup a =
                new(MapGeometryVertexElementGroupUsage.Static, new List<MapGeometryVertexElement>());
            MapGeometryVertexElementGroup b =
                new(MapGeometryVertexElementGroupUsage.Dynamic, new List<MapGeometryVertexElement>());

            Assert.False(a.Equals(b));
        }

        [Fact]
        public void TestIEquatable_ElementMismatch()
        {
            MapGeometryVertexElementGroup a =
                new(
                    MapGeometryVertexElementGroupUsage.Static,
                    new List<MapGeometryVertexElement>()
                    {
                        new(MapGeometryVertexElementName.Position, MapGeometryVertexElementFormat.XYZ_Float32),
                        new(MapGeometryVertexElementName.DiffuseUV, MapGeometryVertexElementFormat.XY_Float32)
                    }
                );
            MapGeometryVertexElementGroup b =
                new(
                    MapGeometryVertexElementGroupUsage.Static,
                    new List<MapGeometryVertexElement>()
                    {
                        new(MapGeometryVertexElementName.Position, MapGeometryVertexElementFormat.XYZ_Float32),
                        new(MapGeometryVertexElementName.Normal, MapGeometryVertexElementFormat.XYZ_Float32)
                    }
                );

            Assert.False(a.Equals(b));
        }
    }
}
