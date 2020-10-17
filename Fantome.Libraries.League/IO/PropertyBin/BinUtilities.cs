namespace Fantome.Libraries.League.IO.PropertyBin
{
    internal sealed class BinUtilities
    {
        private const int COMPLEX_TYPE_FLAG = 128;

        public static BinPropertyType PackType(BinPropertyType type)
        {
            int firstComplexTypeValue = (int)BinPropertyType.Container;

            if ((int)type >= firstComplexTypeValue)
            {
                type = (BinPropertyType)(((int)type - firstComplexTypeValue) | COMPLEX_TYPE_FLAG);
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
                type += (byte)BinPropertyType.Container;
            }

            return type;
        }
    }
}
