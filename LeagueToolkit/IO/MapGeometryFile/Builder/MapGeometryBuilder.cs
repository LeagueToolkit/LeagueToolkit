using CommunityToolkit.Diagnostics;
using LeagueToolkit.Helpers.Structures.BucketGrid;
using System.Collections.Generic;
using System.Linq;

namespace LeagueToolkit.IO.MapGeometryFile.Builder
{
    public class MapGeometryBuilder
    {
        private MapGeometryBakedTerrainSamplers _bakedTerrainSamplers;
        private readonly List<MapGeometryModelBuilder> _meshes = new();
        private BucketGrid _bucketGrid;
        private readonly List<MapGeometryPlanarReflector> _planarReflectors = new();

        public MapGeometryBuilder() { }

        public MapGeometry Build()
        {
            return new(
                this._bakedTerrainSamplers,
                this._meshes.Select((mesh, id) => mesh.Build(id)),
                this._bucketGrid,
                this._planarReflectors
            );
        }

        public MapGeometryBuilder UseBakedTerrainSamplers(MapGeometryBakedTerrainSamplers bakedTerrainSamplers)
        {
            this._bakedTerrainSamplers = bakedTerrainSamplers;
            return this;
        }

        public MapGeometryBuilder UseMesh(MapGeometryModelBuilder mesh)
        {
            Guard.IsNotNull(mesh, nameof(mesh));

            this._meshes.Add(mesh);
            return this;
        }

        public MapGeometryBuilder UseBucketGrid(BucketGrid bucketGrid)
        {
            Guard.IsNotNull(bucketGrid, nameof(bucketGrid));

            this._bucketGrid = bucketGrid;
            return this;
        }

        public MapGeometryBuilder UsePlanarReflector(MapGeometryPlanarReflector planarReflector)
        {
            this._planarReflectors.Add(planarReflector);
            return this;
        }
    }
}
