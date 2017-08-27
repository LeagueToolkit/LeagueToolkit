using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.MOB
{
    public class MOBObject
    {
        public string Name { get; private set; }
        public ObjectType Type { get; private set; }
        public Vector3 Position { get; private set; }
        public Vector3 Rotation { get; private set; }
        public Vector3 Scale { get; private set; }
        public Vector3 HealthbarPosition { get; private set; }
        public Vector3 Unknown { get; set; }

        public MOBObject(string name, ObjectType type, Vector3 position, Vector3 rotation, Vector3 scale, Vector3 healthbarPosition, Vector3 unknown)
        {
            this.Name = name;
            this.Type = type;
            this.Position = position;
            this.Rotation = rotation;
            this.Scale = scale;
            this.HealthbarPosition = healthbarPosition;
            this.Unknown = unknown;
        }

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
            bw.Write(Encoding.ASCII.GetBytes(this.Name.PadRight(60, '\u0000')));
            bw.Write((ushort)0);
            bw.Write((ushort)this.Type);
            this.Position.Write(bw);
            this.Rotation.Write(bw);
            this.Scale.Write(bw);
            this.HealthbarPosition.Write(bw);
            this.Unknown.Write(bw);
            bw.Write((uint)0);
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
