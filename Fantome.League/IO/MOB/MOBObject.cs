using Fantome.League.Helpers.Structures;
using System;
using System.IO;
using System.Text;

namespace Fantome.League.IO.MOB
{
    public class MOBObject
    {
        public string Name { get; private set; }
        public ObjectType Type { get; private set; }
        public Vector3 Position { get; private set; }
        public Vector3 Rotation { get; private set; }
        public Vector3 Scale { get; private set; }
        public Vector3 HealthbarPosition { get; private set; }
        private Vector3 Unknown { get; set; }

        public MOBObject(BinaryReader br)
        {
            this.Name = Encoding.ASCII.GetString(br.ReadBytes(60)).Replace("\0", "");
            br.ReadUInt16();
            this.Type = (ObjectType)br.ReadUInt16();
            this.Position = new Vector3(br);
            this.Rotation = new Vector3(br);
            this.Scale = new Vector3(br);
            this.HealthbarPosition = new Vector3(br);
            this.Unknown = new Vector3(br);
            br.ReadUInt32();
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(this.Name.PadRight(60, '\u0000').ToCharArray());
            bw.Write((UInt16)0);
            bw.Write((UInt16)this.Type);
            this.Position.Write(bw);
            this.Rotation.Write(bw);
            this.Scale.Write(bw);
            this.HealthbarPosition.Write(bw);
            this.Unknown.Write(bw);
            bw.Write((UInt32)0);
        }
    }

    public enum ObjectType : UInt16
    {
        BarrackSpawn,
        NexusSpawn,
        LevelSize,
        Barrack,
        Nexus,
        Turret,
        Shop,
        Lake,
        Nav,
        Info,
        LevelProp
    };
}
