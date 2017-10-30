using Fantome.Libraries.League.Converters;
using Fantome.Libraries.League.IO.AiMesh;
using Fantome.Libraries.League.IO.BIN;
using Fantome.Libraries.League.IO.FX;
using Fantome.Libraries.League.IO.INI;
using Fantome.Libraries.League.IO.Inibin;
using Fantome.Libraries.League.IO.LightDat;
using Fantome.Libraries.League.IO.LightEnvironment;
using Fantome.Libraries.League.IO.LightGrid;
using Fantome.Libraries.League.IO.MapObjects;
using Fantome.Libraries.League.IO.MapParticles;
using Fantome.Libraries.League.IO.MaterialLibrary;
using Fantome.Libraries.League.IO.NVR;
using Fantome.Libraries.League.IO.SCB;
using Fantome.Libraries.League.IO.SCO;
using Fantome.Libraries.League.IO.SimpleSkin;
using Fantome.Libraries.League.IO.WAD;
using Fantome.Libraries.League.IO.WorldGeometry;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Fantome.Libraries.League.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            WADTest();
        }

        static void WGEOTest()
        {
            WGEOFile wgeo = new WGEOFile("room.wgeo");
            wgeo.Write("roomWrite.wgeo");
        }

        static void MOBTest()
        {
            MOBFile mob = new MOBFile("MapObjects.mob");
            List<MOBObject> objects = new List<MOBObject>();
            objects.AddRange(mob.Objects.Where(x => x.Type == MOBObjectType.Info));
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
            sco.Write("kek.sco");
        }

        static void NVRTest()
        {
            NVRFile nvr = new NVRFile("room.nvr");

            WGEOConverter.ConvertNVR(nvr, new WGEOFile("room.wgeo").BucketGeometry).Write("roomNVR.wgeo");
            //IO.OBJ.OBJFile obj = new IO.OBJ.OBJFile("zed.obj");

            //var test = NVRMesh.GetGeometryFromOBJ(obj);
            //NVRMaterial mat = NVRMaterial.CreateMaterial("Zed", "zed.dds");
            //NVRFile nvr = new NVRFile("Map1/scene/roomOR.nvr");
            //nvr.AddMesh(NVRMeshQuality.VERY_LOW, mat, test.Item1, test.Item2);
            //nvr.Save("Map1/scene/room.nvr");
            //OBJConverter.VisualiseNVRNodes(nvr).Write("nodes.obj");
        }

        static void MapParticlesTest()
        {
            MapParticlesFile particlefile = new MapParticlesFile("Particles.dat");
            particlefile.Write("ParticlesWrite.dat");
        }

        static void BINTest()
        {
            BINFile bin = new BINFile("7204EAC36A3BCCB3.bin");
            bin.Write("test.bin");
        }

        static void LightDatTest()
        {
            LightDatFile lightdat = new LightDatFile("Light.dat");
            lightdat.Write("LightWrite.dat");
        }

        static void LightEnvironmentTest()
        {
            LightEnvironmentFile lightenv = new LightEnvironmentFile("Light_env.dat");
            lightenv.Write("Light_envWrite.dat");
        }

        static void LightGridTest()
        {
            LightGridFile lightgrid = new LightGridFile("LightGrid.dat");
            lightgrid.WriteTexture("LightGridWrite.tga");
        }

        static void MaterialLibraryTest()
        {
            MaterialLibraryFile materialLibrary = new MaterialLibraryFile("room.mat");
            materialLibrary.Write("kek.txt");
        }

        static void InibinTest()
        {
            InibinFile inibin = new InibinFile("bestInibinMapskins.inibin");
            inibin.AddValue("Attack", "e-xrgba", 5);
            inibin.AddValue("Attack", "kek", 10);
            inibin.AddValue("Attack", "lol", 25d);
            inibin.AddValue("Attack", "chewy", true);
            inibin.AddValue("Attack", "crauzer", false);
            inibin.AddValue("Attack", "vector3", new float[3]);
            inibin.Write("bestInibinMapskins.inibin");
        }

        static void WADTest()
        {
            using (WADFile wad = new WADFile("assets.wad"))
            {
                wad.Write("assets1.wad", 1, 0);
            }
            /*using (WADFile wad = new WADFile("Jinx.wad.client"))
            {
                Dictionary<ulong, byte[]> entries = new Dictionary<ulong, byte[]>();
                foreach(WADEntry entry in wad.Entries)
                {
                    entries.Add(entry.XXHash, entry.GetContent(true));
                }
                Parallel.ForEach(entries, (entry) =>
                {
                    File.WriteAllBytes("lol//" + entry.Key.ToString(), entry.Value);
                });
            }*/
            //string extractionFolder = "D:/Chewy/Desktop/WADTEST";
            //Directory.CreateDirectory(extractionFolder);
            /*using (WADFile wad = new WADFile(@"C:\Riot Games\League of Legends\RADS\projects\league_client\managedfiles\0.0.0.93\Plugins\rcp-fe-viewport\assets.wad"))
            {
                wad.AddEntry(123456789, File.ReadAllBytes(@"C:\Riot Games\League of Legends\RADS\projects\league_client\managedfiles\0.0.0.93\Plugins\rcp-fe-viewport\description.json"), true);
                wad.AddEntry(12345678, File.ReadAllBytes(@"C:\Riot Games\League of Legends\RADS\projects\league_client\managedfiles\0.0.0.93\Plugins\rcp-fe-viewport\description.json"), true);
                wad.AddEntry(0, "wow");
                wad.Entries[0].FileRedirection = "It's like right now";
                wad.Write(@"C:\Riot Games\League of Legends\RADS\projects\league_client\managedfiles\0.0.0.93\Plugins\rcp-fe-viewport\assetsOHWOW.wad");
            }*/
        }

        static void IniTest()
        {
            IniFile cfg = new IniFile("ObjectCFG.cfg");
            cfg.Write("ObjectCFGWrite.cfg");
        }
    }
}