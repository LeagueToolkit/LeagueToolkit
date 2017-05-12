using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fantome.League.IO.WGEO;
using Fantome.League.IO.MOB;
using Fantome.League.IO.SKN;
using Fantome.League.IO.FX;
using Fantome.League.Converters;
using Fantome.League.IO.AiMesh;

namespace Fantome.League.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            AiMeshTest();
        }

        static void WGEOTest()
        {
            WGEOFile wgeo = new WGEOFile("room.wgeo");
            wgeo.Write("roomWrite.wgeo");
        }
        static void MOBTest()
        {
            MOBFile mob = new MOBFile("MapObjects.mob");
            mob.Write("MapObjectsWrite.mob");
        }
        static void SKNTest()
        {
            SKNFile skn = new SKNFile("Gangplank_Skin08.skn");
        }
        static void FXTest()
        {
            FXFile fx = new FXFile("KalistaPChannel.fx");
            fx.Write("KalistaPChannelWrite.fx");
        }
        static void WGEOConverterTest()
        {
            WGEOFile wgeo = new WGEOFile("room.wgeo");
            WGEOConverter converter = new WGEOConverter(wgeo);
            converter.ExportModels("WGEO");
        }
        static void AiMeshTest()
        {
            AiMeshFile aimesh = new AiMeshFile("AIPath.aimesh");
            aimesh.Write("AIPathWrite.aimesh");
        }
    }
}
