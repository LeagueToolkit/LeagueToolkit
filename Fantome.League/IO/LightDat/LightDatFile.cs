using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fantome.League.IO.LightDat
{
    public class LightDatFile
    {
        public List<LightDatLight> Lights { get; private set; } = new List<LightDatLight>();
        public LightDatFile(string location)
        {
            using (StreamReader sr = new StreamReader(File.OpenRead(location)))
            {
                while(sr.BaseStream.Position != sr.BaseStream.Length)
                {
                    this.Lights.Add(new LightDatLight(sr));
                }
            }        
        }
    }
}
