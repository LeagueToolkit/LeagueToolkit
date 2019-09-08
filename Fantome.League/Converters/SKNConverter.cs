using Fantome.Libraries.League.IO.SCO;
using Fantome.Libraries.League.IO.SimpleSkin;
using Fantome.Libraries.League.IO.WGT;

namespace Fantome.Libraries.League.Converters
{
    public static class SKNConverter
    {
        /// <summary>
        /// Converts <paramref name="weights"/> and <paramref name="model"/> to an <see cref="SKNFile"/>
        /// </summary>
        /// <param name="weights">The <see cref="WGTFile"/> to be used for weights</param>
        /// <param name="model">The <see cref="SCOFile"/> to be used for model data</param>
        /// <returns>An <see cref="SKNFile"/> converted from <paramref name="weights"/> and <paramref name="model"/></returns>
        public static SKNFile ConvertLegacyModel(WGTFile weights, SCOFile model)
        {
            return new SKNFile(weights, model);
        }
    }
}
