using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace LeagueToolkit.Helpers
{
    public static class Memory
    {
        public static byte[] ToBuffer<T>(this T structure) where T: struct
        {
            int size = Marshal.SizeOf(structure);
            byte[] buffer = new byte[size];

            IntPtr unmanagedBuffer = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(structure, unmanagedBuffer, true);
            Marshal.Copy(unmanagedBuffer, buffer, 0, size);
            Marshal.FreeHGlobal(unmanagedBuffer);

            return buffer;
        }

        public static int RawSize<T>(this T structure) where T: struct
        {
            return Marshal.SizeOf(structure);
        }

        public static void CopyStructureToBuffer<T>(byte[] bufer, int offset, T structure) where T: struct
        {
            byte[] structureBuffer = structure.ToBuffer();
            Buffer.BlockCopy(structureBuffer, 0, bufer, offset, structureBuffer.Length);
        }
    }
}
