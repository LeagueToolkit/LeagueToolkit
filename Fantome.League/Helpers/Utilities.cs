using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantome.Libraries.League.Helpers.Utilities
{
    public static class Utilities
    {
        public static string ByteArrayToHex(byte[] array)
        {
            return BitConverter.ToString(array).Replace("-", "");
        }

        public enum LeagueFileType
        {
            Unknown,
            ANM,
            BIN,
            BNK,
            DDS,
            SCB,
            SCO,
            SKL,
            SKN
        }

        public static LeagueFileType GetLeagueFileExtensionType(byte[] fileData)
        {
            byte[] id = new byte[8];
            Buffer.BlockCopy(fileData, 0, id, 0, 8);
            if (id[0] == 'r' && id[1] == '3' && id[2] == 'd' && id[3] == '2')
            {
                if (id[4] == 'M' && id[5] == 'e' && id[6] == 's' && id[7] == 'h')
                {
                    return LeagueFileType.SCB;
                }
                else if (id[4] == 's' && id[5] == 'k' && id[6] == 'l' && id[7] == 't')
                {
                    return LeagueFileType.SKL;
                }
                else if (id[4] == 'a' && id[5] == 'n' && id[6] == 'm' && id[7] == 'd')
                {
                    return LeagueFileType.ANM;
                }
                else if (id[4] == 'c' && id[5] == 'a' && id[6] == 'n' && id[7] == 'm')
                {
                    return LeagueFileType.ANM;
                }
            }
            else if (id[0] == 'B' && id[1] == 'K' && id[2] == 'H' && id[3] == 'D')
            {
                return LeagueFileType.BNK;
            }
            else if (id[0] == 0x33 && id[1] == 0x22 && id[2] == 0x11 && id[3] == 0x00)
            {
                return LeagueFileType.SKN;
            }
            else if (id[0] == 'D' && id[1] == 'D' && id[2] == 'S' && id[3] == 0x20)
            {
                return LeagueFileType.DDS;
            }
            else if (id[0] == 'P' && id[1] == 'R' && id[2] == 'O' && id[3] == 'P')
            {
                return LeagueFileType.BIN;
            }
            else if (id[0] == '[' && id[1] == 'O' && id[2] == 'b' && id[3] == 'j')
            {
                return LeagueFileType.SCO;
            }
            else if (BitConverter.ToInt32(id.Take(4).ToArray(), 0) == fileData.Length)
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
}
