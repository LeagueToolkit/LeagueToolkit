using Fantome.League.IO.SCO;
using Fantome.League.IO.SKN;
using Fantome.League.IO.WGT;

namespace Fantome.League.Converters
{
    public static class SKNConverter
    {
        public static SKNFile ConvertLegacyModel(WGTFile Weights, SCOFile Model)
        {
            return new SKNFile(Weights, Model);
        }
    }
}
