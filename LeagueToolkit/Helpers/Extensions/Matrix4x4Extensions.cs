using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace LeagueToolkit.Helpers.Extensions
{
    public static class Matrix4x4Extensions
    {
        public static Vector3 GetScale(this Matrix4x4 matrix)
        {
            return new Vector3()
            {
                X = new Vector3(matrix.M11, matrix.M12, matrix.M13).Length(),
                Y = new Vector3(matrix.M21, matrix.M22, matrix.M23).Length(),
                Z = new Vector3(matrix.M31, matrix.M32, matrix.M33).Length()
            };
        }
    }
}
