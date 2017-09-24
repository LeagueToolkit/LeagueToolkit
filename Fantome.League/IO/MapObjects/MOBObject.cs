using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.MapObjects
{
    /// <summary>
    /// Represents an Object inside of a <see cref="MOBFile"/>
    /// </summary>
    public class MOBObject
    {
        /// <summary>
        /// Name of this <see cref="MOBObject"/>
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Type of this <see cref="MOBObject"/>
        /// </summary>
        public MOBObjectType Type { get; set; }
        /// <summary>
        /// Position of this <see cref="MOBObject"/>
        /// </summary>
        public Vector3 Position { get; set; }
        /// <summary>
        /// Rotation of this <see cref="MOBObject"/>
        /// </summary>
        public Vector3 Rotation { get; set; }
        /// <summary>
        /// Scale of this <see cref="MOBObject"/>
        /// </summary>
        public Vector3 Scale { get; set; }
        /// <summary>
        /// Used to store additional Vector data of this <see cref="MOBObject"/>
        /// </summary>
        public Vector3 ReservedVector1 { get; set; }
        /// <summary>
        /// Used to store additional Vector data of this <see cref="MOBObject"/>
        /// </summary>
        public Vector3 ReservedVector2 { get; set; }

        /// <summary>
        /// Initializes a new <see cref="MOBObject"/>
        /// </summary>
        /// <param name="name">Name of this <see cref="MOBObject"/></param>
        /// <param name="type">Type of this <see cref="MOBObject"/></param>
        /// <param name="position">Position of this <see cref="MOBObject"/></param>
        /// <param name="rotation">Scale of this <see cref="MOBObject"/></param>
        /// <param name="scale">Scale of this <see cref="MOBObject"/></param>
        /// <param name="reservedVector1">Used to store additional Vector data of this <see cref="MOBObject"/></param>
        /// <param name="reservedVector2">Used to store additional Vector data of this <see cref="MOBObject"/></param>
        public MOBObject(string name, MOBObjectType type, Vector3 position, Vector3 rotation, Vector3 scale, Vector3 reservedVector1, Vector3 reservedVector2)
        {
            this.Name = name;
            this.Type = type;
            this.Position = position;
            this.Rotation = rotation;
            this.Scale = scale;
            this.ReservedVector1 = reservedVector1;
            this.ReservedVector2 = reservedVector2;
        }

        /// <summary>
        /// Initializes a new <see cref="MOBObject"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        public MOBObject(BinaryReader br)
        {
            this.Name = Encoding.ASCII.GetString(br.ReadBytes(60)).Replace("\0", "");
            br.ReadUInt16();
            this.Type = (MOBObjectType)br.ReadUInt16();
            this.Position = new Vector3(br);
            this.Rotation = new Vector3(br);
            this.Scale = new Vector3(br);
            this.ReservedVector1 = new Vector3(br);
            this.ReservedVector2 = new Vector3(br);
            br.ReadUInt32();
        }

        /// <summary>
        /// Writes this <see cref="MOBObject"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        public void Write(BinaryWriter bw)
        {
            bw.Write(Encoding.ASCII.GetBytes(this.Name.PadRight(60, '\u0000')));
            bw.Write((ushort)0);
            bw.Write((ushort)this.Type);
            this.Position.Write(bw);
            this.Rotation.Write(bw);
            this.Scale.Write(bw);
            this.ReservedVector1.Write(bw);
            this.ReservedVector2.Write(bw);
            bw.Write(0);
        }
    }

    /// <summary>
    /// <see cref="MOBObject"/> types
    /// </summary>
    public enum MOBObjectType : ushort
    {
        /// <summary>
        /// Represents a <see cref="MOBObject"/> where minions spawn
        /// </summary>
        BarrackSpawn,
        /// <summary>
        /// Represents a <see cref="MOBObject"/> where players spawn
        /// </summary>
        NexusSpawn,
        /// <summary>
        /// Represents a <see cref="MOBObject"/> that indicates the size of the map
        /// </summary>
        LevelSize,
        /// <summary>
        /// Represents a <see cref="MOBObject"/> that is an Inhibitor
        /// </summary>
        Barrack,
        /// <summary>
        /// Represents a <see cref="MOBObject"/> that is a Nexus
        /// </summary>
        Nexus,
        /// <summary>
        /// Represents a <see cref="MOBObject"/> that is a Turret
        /// </summary>
        Turret,
        /// <summary>
        /// Represents a <see cref="MOBObject"/> that is a Shop
        /// </summary>
        Shop,
        /// <summary>
        /// Represents a <see cref="MOBObject"/> that is a Lake
        /// </summary>
        Lake,
        /// <summary>
        /// Represents a <see cref="MOBObject"/> that is a Navigation Waypoint
        /// </summary>
        Nav,
        /// <summary>
        /// Represents a <see cref="MOBObject"/> that provides certain information for the game
        /// </summary>
        Info,
        /// <summary>
        /// Represnts a <see cref="MOBObject"/> that is a Level Prop
        /// </summary>
        LevelProp
    };
}
