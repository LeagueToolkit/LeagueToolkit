using Fantome.Libraries.League.IO.WAD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantome.Libraries.League.Helpers
{
    public static class Utilities
    {
        public static string ByteArrayToHex(byte[] array)
        {
            char[] c = new char[array.Length * 2];
            for (int i = 0; i < array.Length; i++)
            {
                int b = array[i] >> 4;
                c[i * 2] = (char)(55 + b + (((b - 10) >> 31) & -7));
                b = array[i] & 0xF;
                c[i * 2 + 1] = (char)(55 + b + (((b - 10) >> 31) & -7));
            }

            return new string(c);
        }

        public static LeagueFileType GetExtensionType(byte[] fileData)
        {
            if (fileData.Length < 4)
            {
                return LeagueFileType.Unknown;
            }

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
                else if (fileData[4] == 1 && fileData[5] == 0 && fileData[6] == 0 && fileData[7] == 0)
                {
                    return LeagueFileType.WPK;
                }
            }
            else if (fileData[1] == 'P' && fileData[2] == 'N' && fileData[3] == 'G')
            {
                return LeagueFileType.PNG;
            }
            else if (fileData[0] == 'D' && fileData[1] == 'D' && fileData[2] == 'S' && fileData[3] == 0x20)
            {
                return LeagueFileType.DDS;
            }
            else if (fileData[0] == 0x33 && fileData[1] == 0x22 && fileData[2] == 0x11 && fileData[3] == 0x00)
            {
                return LeagueFileType.SKN;
            }
            else if (fileData[0] == 'P' && fileData[1] == 'R' && fileData[2] == 'O' && fileData[3] == 'P')
            {
                return LeagueFileType.BIN;
            }
            else if (fileData[0] == 'B' && fileData[1] == 'K' && fileData[2] == 'H' && fileData[3] == 'D')
            {
                return LeagueFileType.BNK;
            }
            else if (fileData[0] == 'W' && fileData[1] == 'G' && fileData[2] == 'E' && fileData[3] == 'O')
            {
                return LeagueFileType.WGEO;
            }
            else if (fileData[0] == 'O' && fileData[1] == 'E' && fileData[2] == 'G' && fileData[3] == 'M')
            {
                return LeagueFileType.MAPGEO;
            }
            else if (fileData[0] == '[' && fileData[1] == 'O' && fileData[2] == 'b' && fileData[3] == 'j')
            {
                return LeagueFileType.SCO;
            }
            else if (fileData[1] == 'L' && fileData[2] == 'u' && fileData[3] == 'a' && fileData[4] == 'Q')
            {
                return LeagueFileType.LUAOBJ;
            }
            else if (fileData[0] == 'P' && fileData[1] == 'r' && fileData[2] == 'e' && fileData[3] == 'L' && fileData[4] == 'o' && fileData[5] == 'a' && fileData[6] == 'd')
            {
                return LeagueFileType.PRELOAD;
            }
            else if (fileData[0] == 3 && fileData[1] == 0 && fileData[2] == 0 && fileData[3] == 0)
            {
                return LeagueFileType.LIGHTGRID;
            }
            else if (fileData[0] == 'R' && fileData[1] == 'S' && fileData[2] == 'T')
            {
                return LeagueFileType.RST;
            }
            else if (fileData[0] == 'P' && fileData[1] == 'T' && fileData[2] == 'C' && fileData[3] == 'H')
            {
                return LeagueFileType.PATCHBIN;
            }
            else if (fileData[0] == 0xff && fileData[1] == 0xd8 && fileData[fileData.Length - 2] == 0xff && fileData[fileData.Length - 1] == 0xd9)
            {
                return LeagueFileType.JPG;
            }
            else if (BitConverter.ToInt32(fileData.Take(4).ToArray(), 0) == fileData.Length)
            {
                return LeagueFileType.SKL;
            }

            return LeagueFileType.Unknown;
        }
        public static LeagueFileType GetExtensionType(string extension)
        {
            if (string.IsNullOrEmpty(extension))
            {
                return LeagueFileType.Unknown;
            }
            else
            {
                if (extension[0] == '.')
                {
                    extension = extension.Remove(0, 1);
                }

                switch (extension)
                {
                    case "anm": return LeagueFileType.ANM;
                    case "bin": return LeagueFileType.BIN;
                    case "bnk": return LeagueFileType.BNK;
                    case "dds": return LeagueFileType.DDS;
                    case "luaobj": return LeagueFileType.LUAOBJ;
                    case "mapgeo": return LeagueFileType.MAPGEO;
                    case "png": return LeagueFileType.PNG;
                    case "preload": return LeagueFileType.PRELOAD;
                    case "scb": return LeagueFileType.SCB;
                    case "sco": return LeagueFileType.SCO;
                    case "skl": return LeagueFileType.SKL;
                    case "skn": return LeagueFileType.SKN;
                    case "wgeo": return LeagueFileType.WGEO;
                    case "wpk": return LeagueFileType.WPK;
                    case "jpg": return LeagueFileType.JPG;
                    case "rst": return LeagueFileType.RST;
                    default: return LeagueFileType.Unknown;
                }
            }
        }
        public static string GetExtension(byte[] fileData)
        {
            return GetExtension(GetExtensionType(fileData));
        }
        public static string GetExtension(LeagueFileType extensionType)
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
                case LeagueFileType.WPK:
                    return "wpk";
                case LeagueFileType.MAPGEO:
                    return "mapgeo";
                case LeagueFileType.WGEO:
                    return "wgeo";
                case LeagueFileType.LIGHTGRID:
                    return "dat";
                case LeagueFileType.RST:
                    return "txt";
                case LeagueFileType.PATCHBIN:
                    return "bin";
                case LeagueFileType.JPG:
                    return "jpg";
                default:
                    return "";
            }
        }
        public static EntryType GetExtensionCompressionType(string extension)
        {
            if (string.IsNullOrEmpty(extension))
            {
                return EntryType.Uncompressed;
            }
            else
            {
                if (extension[0] == '.')
                {
                    extension = extension.Remove(0, 1);
                }

                if (extension.Contains("glsl"))
                {
                    return EntryType.Uncompressed;
                }
                if (extension.Contains("dx9"))
                {
                    return EntryType.Uncompressed;
                }

                switch (extension)
                {
                    case "wpk":
                    case "bnk":
                    case "troybin":
                    case "inibin":
                    case "gfx":
                    case "png":
                    case "preload":
                        return EntryType.Uncompressed;
                    default:
                        return EntryType.ZStandardCompressed;
                }
            }
        }

        public static float ToDegrees(float radian)
        {
            return radian * (180 / (float)Math.PI);
        }
        public static float ToRadians(float degrees)
        {
            return degrees * ((float)Math.PI / 180);
        }

        public static float Clamp(float value, float min, float max)
        {
            return Math.Max(min, Math.Min(max, value));
        }
    }

    public enum LeagueFileType
    {
        Unknown,
        ANM,
        BIN,
        BNK,
        DDS,
        LIGHTGRID,
        LUAOBJ,
        MAPGEO,
        PNG,
        PRELOAD,
        SCB,
        SCO,
        SKL,
        SKN,
        WGEO,
        WPK,
        JPG,
        RST,
        PATCHBIN
    }
}
