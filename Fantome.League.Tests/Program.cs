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
using Fantome.League.IO.SCB;
using Fantome.League.IO.SCO;
using Fantome.League.IO.WGT;
using Fantome.League.IO.NVR;
using Fantome.League.IO.ParticlesDat;

namespace Fantome.League.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            SKNConverterTest();
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
            SKNFile skn = new SKNFile("Plantking.skn");
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
        static void SCBTest()
        {
            SCBFile scb = new SCBFile("aatrox_skin02_q_pulse_01.scb");
            scb.Write("aatrox_skin02_q_pulse_01Write.scb");
        }
        static void SCOTest()
        {
            SCOFile sco = new SCOFile("Aatrox_Basic_A_trail_01.sco");
        }
        static void SKNConverterTest()
        {
            SKNConverter.Convert(SKNConverter.Convert(new WGTFile("Plantking.wgt"), new SCOFile("Plantking.sco")), true).Write("Plantking.obj");
        }
        static void NVRTest()
        {
            NVRFile nvr = new NVRFile("roomWrite.nvr");
            nvr.Save("roomWrite.nvr");
        }
        static void ParticlesDatTest()
        {
            ParticlesDatFile particlefile = new ParticlesDatFile("Particles.dat");
            particlefile.Write("ParticlesWrite.dat");
        }
    }
}
