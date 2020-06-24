namespace Fantome.Libraries.League.IO.BIN
{
    internal class BINUtilities
    {
        private const int COMPLEX_TYPE_FLAG = 128;

        public static BINValueType PackType(BINValueType type)
        {
            if ((int)type >= 18 && (int)type <= 24)
            {
                type = (BINValueType)(((int)type - 18) | COMPLEX_TYPE_FLAG);
            }

            return type;
        }

        public static BINValueType UnpackType(BINValueType type)
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
