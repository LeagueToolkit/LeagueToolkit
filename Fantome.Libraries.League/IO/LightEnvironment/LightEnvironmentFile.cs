using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.IO.LightEnvironment
{
    public class LightEnvironmentFile
    {
        public List<LightEnvironmentLight> Lights { get; private set; } = new List<LightEnvironmentLight>();

        public LightEnvironmentFile(List<LightEnvironmentLight> lights)
        {
            this.Lights = lights;
        }

        public LightEnvironmentFile(string fileLocation)
            : this(File.OpenRead(fileLocation))
        {

        }

        public LightEnvironmentFile(Stream stream)
        {
            using (StreamReader sr = new StreamReader(stream))
            {
                string lightVersion = sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    this.Lights.Add(new LightEnvironmentLight(sr));
                }
            }
        }

        public void Write(string fileLocation)
        {
            Write(File.Create(fileLocation));
        }

        private void Write(Stream stream)
        {
            using (StreamWriter sw = new StreamWriter(stream))
            { 
                sw.WriteLine("3");

                foreach(LightEnvironmentLight light in this.Lights)
                {
                    light.Write(sw);
                }
            }
        }
    }
}
