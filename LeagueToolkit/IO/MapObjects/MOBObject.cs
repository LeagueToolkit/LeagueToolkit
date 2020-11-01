using LeagueToolkit.Helpers.Extensions;
using LeagueToolkit.Helpers.Structures;
using System.IO;
using System.Numerics;
using System.Text;

namespace LeagueToolkit.IO.MapObjects
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
        /// Skin ID of this <see cref="MOBObject"/>
        /// </summary>
        public uint SkinID { get; set; }
        /// <summary>
        /// Collision flag of this <see cref="MOBObject"/>
        /// </summary>
        public bool IgnoreCollisionOnPlacement { get; set; }
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
        /// Bounding Box of this <see cref="MOBObject"/>
        /// </summary>
        public R3DBox BoundingBox { get; set; }

        /// <summary>
        /// Initializes a new <see cref="MOBObject"/>
        /// </summary>
        /// <param name="name">Name of this <see cref="MOBObject"/></param>
        /// <param name="type">Type of this <see cref="MOBObject"/></param>
        /// <param name="skinID">Skin ID of this <see cref="MOBObject"/></param>
        /// <param name="ignoreCollisionOnPlacement">Collision flag of this <see cref="MOBObject"/></param>
        /// <param name="position">Position of this <see cref="MOBObject"/></param>
        /// <param name="rotation">Scale of this <see cref="MOBObject"/></param>
        /// <param name="scale">Scale of this <see cref="MOBObject"/></param>
        /// <param name="boundingBox">Bounding Box of this <see cref="MOBObject"/></param>
        public MOBObject(string name, MOBObjectType type, uint skinID, bool ignoreCollisionOnPlacement, Vector3 position, Vector3 rotation, Vector3 scale, R3DBox boundingBox)
        {
            this.Name = name;
            this.Type = type;
            this.SkinID = skinID;
            this.IgnoreCollisionOnPlacement = ignoreCollisionOnPlacement;
            this.Position = position;
            this.Rotation = rotation;
            this.Scale = scale;
            this.BoundingBox = boundingBox;
        }

        /// <summary>
        /// Initializes a new <see cref="MOBObject"/> from a <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> to read from</param>
        public MOBObject(BinaryReader br)
        {
            this.Name = Encoding.ASCII.GetString(br.ReadBytes(62)).Replace("\0", "");
            this.Type = (MOBObjectType)br.ReadByte();
            this.IgnoreCollisionOnPlacement = br.ReadBoolean();
            this.Position = br.ReadVector3();
            this.Rotation = br.ReadVector3();
            this.Scale = br.ReadVector3();
            this.BoundingBox = new R3DBox(br);
            this.SkinID = br.ReadUInt32();
        }

        /// <summary>
        /// Writes this <see cref="MOBObject"/> into a <see cref="BinaryWriter"/>
        /// </summary>
        /// <param name="bw">The <see cref="BinaryWriter"/> to write to</param>
        public void Write(BinaryWriter bw)
        {
            bw.Write(Encoding.ASCII.GetBytes(this.Name.PadRight(62, '\u0000')));
            bw.Write((byte)this.Type);
            bw.Write(this.IgnoreCollisionOnPlacement);
            bw.WriteVector3(this.Position);
            bw.WriteVector3(this.Rotation);
            bw.WriteVector3(this.Scale);
            this.BoundingBox.Write(bw);
            bw.Write(this.SkinID);
        }
    }

    /// <summary>
    /// <see cref="MOBObject"/> types
    /// </summary>
    public enum MOBObjectType : byte
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
