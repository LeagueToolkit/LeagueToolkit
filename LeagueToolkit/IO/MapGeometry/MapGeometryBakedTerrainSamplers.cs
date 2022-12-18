namespace LeagueToolkit.IO.MapGeometry
{
    public struct MapGeometryBakedTerrainSamplers
    {
        public string Primary;
        public string Secondary;

        public MapGeometryBakedTerrainSamplers()
        {
            this.Primary = string.Empty;
            this.Secondary = string.Empty;
        }
        public MapGeometryBakedTerrainSamplers(string primary, string secondary) 
        {
            this.Primary = primary;
            this.Secondary = secondary;
        }
    }
}
