namespace Fantome.Libraries.League.IO.BIN
{
    internal sealed class BinUtilities
    {
        private const int COMPLEX_TYPE_FLAG = 128;

        public static BinPropertyType PackType(BinPropertyType type)
        {
            if ((int)type >= 18 && (int)type <= 24)
            {
                type = (BinPropertyType)(((int)type - 18) | COMPLEX_TYPE_FLAG);
            }

            return type;
        }

        public static BinPropertyType UnpackType(BinPropertyType type)
        {
            if (((int)type & COMPLEX_TYPE_FLAG) == COMPLEX_TYPE_FLAG)
            {
                type -= COMPLEX_TYPE_FLAG;
                type += 18;
            }

            return type;
        }
    }
}
