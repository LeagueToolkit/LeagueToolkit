using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.IO.InfoFile
{
    public class InfoFile
    {
        public List<InfoFileCharacter> Characters = new List<InfoFileCharacter>();

        public InfoFile(string fileLocation) : this(File.OpenRead(fileLocation)) { }

        public InfoFile(Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream))
            {
                uint characterCount = br.ReadUInt32();

                for (int i = 0; i < characterCount; i++)
                {
                    this.Characters.Add(new InfoFileCharacter(br));
                }
            }
        }
    }
}
