using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Utilities
{
    public static void Seek(this BinaryReader br, uint offset, SeekOrigin origin)
    {
        br.BaseStream.Seek(offset, origin);
    }

    public static string ReadString(this BinaryReader br, int blockSize)
    {
        bool shouldStop = false;
        string toReturn = "";
        while (shouldStop != true)
        {
            toReturn += br.ReadChars(blockSize).GetCharBlock(out shouldStop);
        }
        return toReturn;
    }
    private static string GetCharBlock(this char[] charBlock, out bool shouldStop)
    {
        string toReturn = "";
        foreach (char c in charBlock)
        {
            if (c != '\u0000')
            {
                toReturn += c;
            }
            else
            {
                shouldStop = true;
                return toReturn;
            }
        }
        shouldStop = false;
        return toReturn;
    }

    public static string AddPadding(this string toPadd, string toAdd, int desiredLength)
    {
        string toReturn = toPadd;
        for(int i = toPadd.Length; i < desiredLength; i++)
        {
            toReturn += toAdd;
        }
        return toReturn;
    }
}
