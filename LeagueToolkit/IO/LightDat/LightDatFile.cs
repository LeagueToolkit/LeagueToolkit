using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LeagueToolkit.IO.LightDat
{
    public class LightDatFile
    {
        public List<LightDatLight> Lights { get; private set; } = new List<LightDatLight>();

        public LightDatFile(List<LightDatLight> lights)
        {
            this.Lights = lights;
        }
        public LightDatFile(string fileLocation)
            : this(File.OpenRead(fileLocation))
        {

        }
        public LightDatFile(Stream stream)
        {
            using (StreamReader sr = new StreamReader(stream))
            {
                while (!sr.EndOfStream)
                {
                    this.Lights.Add(new LightDatLight(sr));
                }
            }
        }

        public void Write(string fileLocation)
        {
            Write(File.Create(fileLocation));
        }

        private void Write(Stream stream, bool leaveOpen = false)
        {
            using (StreamWriter sw = new StreamWriter(stream, Encoding.UTF8, 1024, leaveOpen))
            {
                foreach (LightDatLight light in this.Lights)
                {
                    light.Write(sw);
                }
            }
        }
    }
}
