using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.InfoFile
{
    public class InfoFileCharacter
    {
        public string Name { get; set; }
        public uint Unknown { get; set; }
        public List<Tuple<string, uint>> Skins = new List<Tuple<string, uint>>();

        public InfoFileCharacter(BinaryReader br)
        {
            this.Name = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
            uint skinCount = br.ReadUInt32();
            this.Unknown = br.ReadUInt32();

            for (int i = 0; i < skinCount; i++)
            {
                if ((this.Skins.Count + 1) != skinCount)
                {
                    this.Skins.Add(new Tuple<string, uint>(Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32())), br.ReadUInt32()));
                }
                else
                {
                    this.Skins.Add(new Tuple<string, uint>(Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32())), 0));
                }
            }
        }
    }
}
