using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fantome.Libraries.League.Helpers.Structures;

namespace Fantome.Libraries.League.IO.MapGeometry
{
    public class MGEOMesh
    {
        public string Name { get; set; }
        public uint Unknown1 { get; set; }
        public uint Unknown2 { get; set; }
        public List<uint> Unknown3 { get; set; } = new List<uint>(); //Possibly the index of the vertex and index buffers ?
        public uint Unknown4 { get; set; }
        public uint Unknown5 { get; set; }
        public List<MGEOMaterial> Materials { get; set; } = new List<MGEOMaterial>();
        public R3DBox BoundingBox { get; set; }
        public R3DMatrix44 TransformationMatrix { get; set; }
        public Vector3 Unknown7 { get; set; }
        public float[] Unknown8 { get; set; } = new float[27];
        public string Texture { get; set; }
        public ColorRGBAVector4 Color { get; set; }

        public MGEOMesh(BinaryReader br, bool specialHeaderFlag)
        {
            this.Name = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
            this.Unknown1 = br.ReadUInt32();
            uint unknownCount = br.ReadUInt32();
            this.Unknown2 = br.ReadUInt32();

            for (int i = 0; i < unknownCount; i++)
            {
                this.Unknown3.Add(br.ReadUInt32());
            }

            this.Unknown4 = br.ReadUInt32();
            this.Unknown5 = br.ReadUInt32();

            uint materialCount = br.ReadUInt32();
            for (int i = 0; i < materialCount; i++)
            {
                this.Materials.Add(new MGEOMaterial(br));
            }

            this.BoundingBox = new R3DBox(br);
            this.TransformationMatrix = new R3DMatrix44(br);
            uint unknownPaddingOrFlag = br.ReadByte();

            if (specialHeaderFlag)
            {
                this.Unknown7 = new Vector3(br);
            }

            for (int i = 0; i < 27; i++)
            {
                this.Unknown8[i] = br.ReadSingle();
            }

            this.Texture = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
            this.Color = new ColorRGBAVector4(br);
        }
    }
}
