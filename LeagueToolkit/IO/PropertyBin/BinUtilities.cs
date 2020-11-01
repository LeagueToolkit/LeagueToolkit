namespace LeagueToolkit.IO.PropertyBin
{
    internal sealed class BinUtilities
    {
        private const int COMPLEX_TYPE_FLAG = 128;
        private const byte FIRST_COMPLEX_TYPE = (byte)BinPropertyType.Container;

        public static BinPropertyType PackType(BinPropertyType type)
        {
            if ((int)type >= FIRST_COMPLEX_TYPE)
            {
                type = (BinPropertyType)(((int)type - FIRST_COMPLEX_TYPE) | COMPLEX_TYPE_FLAG);
            }

            return type;
        }

        public static BinPropertyType UnpackType(BinPropertyType type)
        {
            // If complex type flag is set then we add the value of the first complex type
            // to the packed type
            if (((int)type & COMPLEX_TYPE_FLAG) == COMPLEX_TYPE_FLAG)
            {
                type -= COMPLEX_TYPE_FLAG;
                type += FIRST_COMPLEX_TYPE;
            }

            return type;
        }
    }
}
