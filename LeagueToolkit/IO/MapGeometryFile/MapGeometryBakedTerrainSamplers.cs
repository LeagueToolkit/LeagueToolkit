namespace LeagueToolkit.IO.MapGeometryFile
{
    /// <summary>
    /// Contains information about which samplers should be used for sampling "BAKED_PAINT" textures
    /// </summary>
    public struct MapGeometryBakedTerrainSamplers
    {
        /// <value>
        /// The name of the primary sampler
        /// </value>
        /// <remarks>
        /// Known values: <c>BAKED_DIFFUSE_TEXTURE</c>
        /// </remarks>
        public string Primary;

        /// <value>
        /// The name of the secondary sampler
        /// </value>
        /// <remarks>
        /// This sampler uses the same description and texture handle as <see cref="Primary"/>
        /// <br>
        /// Known values: <c>BAKED_DIFFUSE_TEXTURE_ALPHA</c>
        /// </br>
        /// </remarks>
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
