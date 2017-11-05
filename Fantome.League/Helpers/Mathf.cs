using System;

namespace Fantome.Libraries.League.Helpers
{
    public static class Mathf
    {
        public static float RadiansToDegrees(float radians)
        {
            return radians * (float)(180 / Math.PI);
        }

        public static float DegreesToRadians(float degrees)
        {
            return degrees * (float)(Math.PI / 180);
        }
    }
}
