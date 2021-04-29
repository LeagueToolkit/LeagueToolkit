using LeagueToolkit.IO.SkeletonFile;
using LeagueToolkit.IO.WadFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueToolkit.Helpers
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

        public static LeagueFileType GetExtensionType(byte[] magicData)
        {
            if (magicData.Length < 4)
            {
                return LeagueFileType.Unknown;
            }

            if (magicData[0] == 'r' && magicData[1] == '3' && magicData[2] == 'd' && magicData[3] == '2')
            {
                if (magicData[4] == 'M' && magicData[5] == 'e' && magicData[6] == 's' && magicData[7] == 'h')
                {
                    return LeagueFileType.StaticObjectBinary;
                }
                else if (magicData[4] == 's' && magicData[5] == 'k' && magicData[6] == 'l' && magicData[7] == 't')
                {
                    return LeagueFileType.Skeleton;
                }
                else if (magicData[4] == 'a' && magicData[5] == 'n' && magicData[6] == 'm' && magicData[7] == 'd')
                {
                    return LeagueFileType.Animation;
                }
                else if (magicData[4] == 'c' && magicData[5] == 'a' && magicData[6] == 'n' && magicData[7] == 'm')
                {
                    return LeagueFileType.Animation;
                }
                else if (magicData[4] == 1 && magicData[5] == 0 && magicData[6] == 0 && magicData[7] == 0)
                {
                    return LeagueFileType.WwisePackage;
                }
            }
            else if (magicData[1] == 'P' && magicData[2] == 'N' && magicData[3] == 'G')
            {
                return LeagueFileType.PngImage;
            }
            else if (magicData[0] == 'D' && magicData[1] == 'D' && magicData[2] == 'S' && magicData[3] == 0x20)
            {
                return LeagueFileType.DdsImage;
            }
            else if (magicData[0] == 0x33 && magicData[1] == 0x22 && magicData[2] == 0x11 && magicData[3] == 0x00)
            {
                return LeagueFileType.SimpleSkin;
            }
            else if (magicData[0] == 'P' && magicData[1] == 'R' && magicData[2] == 'O' && magicData[3] == 'P')
            {
                return LeagueFileType.PropertyBin;
            }
            else if (magicData[0] == 'B' && magicData[1] == 'K' && magicData[2] == 'H' && magicData[3] == 'D')
            {
                return LeagueFileType.WwiseBank;
            }
            else if (magicData[0] == 'W' && magicData[1] == 'G' && magicData[2] == 'E' && magicData[3] == 'O')
            {
                return LeagueFileType.WorldGeometry;
            }
            else if (magicData[0] == 'O' && magicData[1] == 'E' && magicData[2] == 'G' && magicData[3] == 'M')
            {
                return LeagueFileType.MapGeometry;
            }
            else if (magicData[0] == '[' && magicData[1] == 'O' && magicData[2] == 'b' && magicData[3] == 'j')
            {
                return LeagueFileType.StaticObjectAscii;
            }
            else if (magicData[1] == 'L' && magicData[2] == 'u' && magicData[3] == 'a' && magicData[4] == 'Q')
            {
                return LeagueFileType.LuaObj;
            }
            else if (magicData[0] == 'P' && magicData[1] == 'r' && magicData[2] == 'e' && magicData[3] == 'L' && magicData[4] == 'o' && magicData[5] == 'a' && magicData[6] == 'd')
            {
                return LeagueFileType.Preload;
            }
            else if (magicData[0] == 3 && magicData[1] == 0 && magicData[2] == 0 && magicData[3] == 0)
            {
                return LeagueFileType.Lightgrid;
            }
            else if (magicData[0] == 'R' && magicData[1] == 'S' && magicData[2] == 'T')
            {
                return LeagueFileType.RiotStringTable;
            }
            else if (magicData[0] == 'P' && magicData[1] == 'T' && magicData[2] == 'C' && magicData[3] == 'H')
            {
                return LeagueFileType.PatchPropertyBin;
            }
            else if (magicData[0] == 0xFF && magicData[1] == 0xD8 && magicData[2] == 0xFF)
            {
                return LeagueFileType.JpegImage;
            }
            else if (BitConverter.ToInt32(magicData, 4) == Skeleton.FORMAT_TOKEN)
            {
                return LeagueFileType.Skeleton;
            }

            return LeagueFileType.Unknown;
        }
        public static LeagueFileType GetExtensionType(Stream stream, int headerSizeHint = 8)
        {
            byte[] header = new byte[headerSizeHint];
            int originalPosition = (int)stream.Position;

            stream.Read(header, 0, header.Length);
            stream.Seek(originalPosition, SeekOrigin.Begin);

            return GetExtensionType(header);
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
                    case "anm": return LeagueFileType.Animation;
                    case "bin": return LeagueFileType.PropertyBin;
                    case "bnk": return LeagueFileType.WwiseBank;
                    case "dds": return LeagueFileType.DdsImage;
                    case "luaobj": return LeagueFileType.LuaObj;
                    case "mapgeo": return LeagueFileType.MapGeometry;
                    case "png": return LeagueFileType.PngImage;
                    case "preload": return LeagueFileType.Preload;
                    case "scb": return LeagueFileType.StaticObjectBinary;
                    case "sco": return LeagueFileType.StaticObjectAscii;
                    case "skl": return LeagueFileType.Skeleton;
                    case "skn": return LeagueFileType.SimpleSkin;
                    case "wgeo": return LeagueFileType.WorldGeometry;
                    case "wpk": return LeagueFileType.WwisePackage;
                    case "jpg": return LeagueFileType.JpegImage;
                    case "rst": return LeagueFileType.RiotStringTable;
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
                case LeagueFileType.Animation:
                    return "anm";
                case LeagueFileType.PropertyBin:
                    return "bin";
                case LeagueFileType.WwiseBank:
                    return "bnk";
                case LeagueFileType.DdsImage:
                    return "dds";
                case LeagueFileType.LuaObj:
                    return "luaobj";
                case LeagueFileType.Preload:
                    return "preload";
                case LeagueFileType.PngImage:
                    return "png";
                case LeagueFileType.StaticObjectBinary:
                    return "scb";
                case LeagueFileType.StaticObjectAscii:
                    return "sco";
                case LeagueFileType.Skeleton:
                    return "skl";
                case LeagueFileType.SimpleSkin:
                    return "skn";
                case LeagueFileType.WwisePackage:
                    return "wpk";
                case LeagueFileType.MapGeometry:
                    return "mapgeo";
                case LeagueFileType.WorldGeometry:
                    return "wgeo";
                case LeagueFileType.Lightgrid:
                    return "dat";
                case LeagueFileType.RiotStringTable:
                    return "txt";
                case LeagueFileType.PatchPropertyBin:
                    return "bin";
                case LeagueFileType.JpegImage:
                    return "jpg";
                default:
                    return "";
            }
        }
        public static WadEntryType GetExtensionWadCompressionType(string extension)
        {
            if (string.IsNullOrEmpty(extension))
            {
                return WadEntryType.Uncompressed;
            }
            else
            {
                if (extension[0] == '.')
                {
                    extension = extension.Remove(0, 1);
                }

                if (extension.Contains("glsl"))
                {
                    return WadEntryType.Uncompressed;
                }
                if (extension.Contains("dx9"))
                {
                    return WadEntryType.Uncompressed;
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
                        return WadEntryType.Uncompressed;
                    default:
                        return WadEntryType.ZStandardCompressed;
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
        Animation,
        PropertyBin,
        WwiseBank,
        DdsImage,
        Lightgrid,
        LuaObj,
        MapGeometry,
        PngImage,
        Preload,
        StaticObjectBinary,
        StaticObjectAscii,
        Skeleton,
        SimpleSkin,
        WorldGeometry,
        WwisePackage,
        JpegImage,
        RiotStringTable,
        PatchPropertyBin,
        WadArchive
    }
}
