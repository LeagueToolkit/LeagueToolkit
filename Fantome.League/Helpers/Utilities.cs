using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantome.Libraries.League.Helpers.Utilities
{
    public static class Utilities
    {
        public static string ByteArrayToHex(byte[] array, bool reverse)
        {
            if(reverse)
            {
                array = array.Reverse().ToArray();
            }

            StringBuilder hexString = new StringBuilder(array.Length * 2);
            string hexAlphabeth = "0123456789ABCDEF";

            for(int i = 0; i < array.Length; i++)
            {
                hexString.Append(hexAlphabeth[array[i] >> 4]);
                hexString.Append(hexAlphabeth[array[i] & 0xF]);
            }

            return hexString.ToString();
        }

        public static LeagueFileType GetLeagueFileExtensionType(byte[] fileData)
        {
            if (fileData[0] == 'r' && fileData[1] == '3' && fileData[2] == 'd' && fileData[3] == '2')
            {
                if (fileData[4] == 'M' && fileData[5] == 'e' && fileData[6] == 's' && fileData[7] == 'h')
                {
                    return LeagueFileType.SCB;
                }
                else if (fileData[4] == 's' && fileData[5] == 'k' && fileData[6] == 'l' && fileData[7] == 't')
                {
                    return LeagueFileType.SKL;
                }
                else if (fileData[4] == 'a' && fileData[5] == 'n' && fileData[6] == 'm' && fileData[7] == 'd')
                {
                    return LeagueFileType.ANM;
                }
                else if (fileData[4] == 'c' && fileData[5] == 'a' && fileData[6] == 'n' && fileData[7] == 'm')
                {
                    return LeagueFileType.ANM;
                }
            }
            else if (fileData[0] == 'B' && fileData[1] == 'K' && fileData[2] == 'H' && fileData[3] == 'D')
            {
                return LeagueFileType.BNK;
            }
            else if (fileData[0] == 0x33 && fileData[1] == 0x22 && fileData[2] == 0x11 && fileData[3] == 0x00)
            {
                return LeagueFileType.SKN;
            }
            else if (fileData[0] == 'D' && fileData[1] == 'D' && fileData[2] == 'S' && fileData[3] == 0x20)
            {
                return LeagueFileType.DDS;
            }
            else if (fileData[0] == 'P' && fileData[1] == 'R' && fileData[2] == 'O' && fileData[3] == 'P')
            {
                return LeagueFileType.BIN;
            }
            else if (fileData[0] == '[' && fileData[1] == 'O' && fileData[2] == 'b' && fileData[3] == 'j')
            {
                return LeagueFileType.SCO;
            }
            else if(fileData[1] == 'L' && fileData[2] == 'u' && fileData[3] == 'a' && fileData[4] == 'Q')
            {
                return LeagueFileType.LUAOBJ;
            }
            else if (fileData[0] == 'P' && fileData[1] == 'r' && fileData[2] == 'e' && fileData[3] == 'L' && fileData[4] == 'o' && fileData[5] == 'a' && fileData[6] == 'd')
            {
                return LeagueFileType.PRELOAD;
            }
            else if (fileData[1] == 'P' && fileData[2] == 'N' && fileData[3] == 'G')
            {
                return LeagueFileType.PNG;
            }
            else if (BitConverter.ToInt32(fileData.Take(4).ToArray(), 0) == fileData.Length)
            {
                return LeagueFileType.SKL;
            }
            return LeagueFileType.Unknown;
        }

        public static string GetEntryExtension(LeagueFileType extensionType)
        {
            switch (extensionType)
            {
                case LeagueFileType.ANM:
                    return "anm";
                case LeagueFileType.BIN:
                    return "bin";
                case LeagueFileType.BNK:
                    return "bnk";
                case LeagueFileType.DDS:
                    return "dds";
                case LeagueFileType.LUAOBJ:
                    return "luaobj";
                case LeagueFileType.PRELOAD:
                    return "preload";
                case LeagueFileType.PNG:
                    return "png";
                case LeagueFileType.SCB:
                    return "scb";
                case LeagueFileType.SCO:
                    return "sco";
                case LeagueFileType.SKL:
                    return "skl";
                case LeagueFileType.SKN:
                    return "skn";
                default:
                    return "";
            }
        }
    }

    public enum LeagueFileType
    {
        Unknown,
        ANM,
        BIN,
        BNK,
        DDS,
        LUAOBJ,
        PNG,
        PRELOAD,
        SCB,
        SCO,
        SKL,
        SKN
    }
}
