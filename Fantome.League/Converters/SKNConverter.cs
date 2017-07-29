using Fantome.Libraries.League.IO.SCO;
using Fantome.Libraries.League.IO.SKN;
using Fantome.Libraries.League.IO.WGT;

namespace Fantome.Libraries.League.Converters
{
    public static class SKNConverter
    {
        public static SKNFile ConvertLegacyModel(WGTFile Weights, SCOFile Model)
        {
            return new SKNFile(Weights, Model);
        }
    }
}
