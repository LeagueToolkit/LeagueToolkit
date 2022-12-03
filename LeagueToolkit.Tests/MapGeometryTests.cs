using LeagueToolkit.IO.MapGeometry;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueToolkit.Tests
{
    [TestFixture(Author = "Crauzer", Category = "MapGeometry")]
    public class MapGeometryTests { }

    [TestFixture(Author = "Crauzer", Category = "MapGeometry")]
    public class MapGeometryVertexElementGroupTests
    {
        [Test(ExpectedResult = true)]
        public bool TestIEquatable_Equal()
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

            return a.Equals(b);
        }

        [Test(ExpectedResult = false)]
        public bool TestIEquatable_UsageMismatch()
        {
            MapGeometryVertexElementGroup a =
                new(MapGeometryVertexElementGroupUsage.Static, new List<MapGeometryVertexElement>());
            MapGeometryVertexElementGroup b =
                new(MapGeometryVertexElementGroupUsage.Dynamic, new List<MapGeometryVertexElement>());

            return a.Equals(b);
        }

        [Test(ExpectedResult = false)]
        public bool TestIEquatable_ElementMismatch()
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

            return a.Equals(b);
        }
    }

    [TestFixture(Author = "Crauzer", Category = "MapGeometry")]
    public class MapGeometryVertexElementTests { }
}
