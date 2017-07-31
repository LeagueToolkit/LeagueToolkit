using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.IO.LightDat
{
    public class LightDatFile
    {
        public List<LightDatLight> Lights { get; private set; } = new List<LightDatLight>();

        public LightDatFile(List<LightDatLight> lights)
        {
            this.Lights = lights;
        }

        public LightDatFile(string fileLocation)
        {
            using (StreamReader sr = new StreamReader(fileLocation))
            {
                while(!sr.EndOfStream)
                {
                    this.Lights.Add(new LightDatLight(sr));
                }
            }        
        }

        public void Write(string fileLocation)
        {
            using (StreamWriter sw = new StreamWriter(fileLocation))
            {
                foreach(LightDatLight light in this.Lights)
                {
                    light.Write(sw);
                }
            }
        }
    }
}
