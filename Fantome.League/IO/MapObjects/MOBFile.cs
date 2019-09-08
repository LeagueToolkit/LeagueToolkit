using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.MapObjects
{
    /// <summary>
    /// Represents a MapObjects.mob file
    /// </summary>
    public class MOBFile
    {
        /// <summary>
        /// Objects of this <see cref="MOBFile"/>
        /// </summary>
        public List<MOBObject> Objects { get; private set; } = new List<MOBObject>();

        /// <summary>
        /// Initializes an empty <see cref="MOBFile"/>
        /// </summary>
        public MOBFile() { }

        /// <summary>
        /// Initializes a new <see cref="MOBFile"/>
        /// </summary>
        /// <param name="objects">Objects of this <see cref="MOBFile"/></param>
        public MOBFile(List<MOBObject> objects)
        {
            this.Objects = objects;
        }

        /// <summary>
        /// Initializes a new <see cref="MOBFile"/> from the specified location
        /// </summary>
        /// <param name="fileLocation">Location to read from</param>
        public MOBFile(string fileLocation) : this(File.OpenRead(fileLocation)) { }

        /// <summary>
        /// Initalizes a new <see cref="MOBFile"/> from the specified <see cref="Stream"/>
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read from</param>
        public MOBFile(Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream))
            {
                string magic = Encoding.ASCII.GetString(br.ReadBytes(4));
                if (magic != "OPAM")
                {
                    throw new Exception("This is not a valid MOB file");
                }

                uint version = br.ReadUInt32();
                if (version != 2)
                {
                    throw new Exception("This version is not supported");
                }

                uint objectCount = br.ReadUInt32();
                br.ReadUInt32();

                for (int i = 0; i < objectCount; i++)
                {
                    this.Objects.Add(new MOBObject(br));
                }
            }
        }

        /// <summary>
        /// Writes this <see cref="MOBFile"/> to the spcified location
        /// </summary>
        /// <param name="fileLocation">Location to write to</param>
        public void Write(string fileLocation)
        {
            Write(File.Create(fileLocation));
        }

        /// <summary>
        /// Writes this <see cref="MOBFile"/> into a <see cref="Stream"/>
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to write to</param>
        public void Write(Stream stream)
        {
            using (BinaryWriter bw = new BinaryWriter(stream))
            {
                bw.Write(Encoding.ASCII.GetBytes("OPAM"));
                bw.Write((uint)2);
                bw.Write(this.Objects.Count);
                bw.Write((uint)0);

                foreach (MOBObject mobObject in this.Objects)
                {
                    mobObject.Write(bw);
                }
            }
        }
    }
}
