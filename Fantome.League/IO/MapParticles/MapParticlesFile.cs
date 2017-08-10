using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.IO.MapParticles
{
    public class MapParticlesFile
    {
        public List<MapParticlesParticle> Particles { get; private set; }

        public MapParticlesFile(List<MapParticlesParticle> particles)
        {
            this.Particles = particles;
        }

        public MapParticlesFile(string fileLocation)
        {
            this.Particles = new List<MapParticlesParticle>();
            using (StreamReader sr = new StreamReader(fileLocation))
            {
                while (!sr.EndOfStream)
                {
                    this.Particles.Add(new MapParticlesParticle(sr));
                }
            }
        }

        public void Write(string fileLocation)
        {
            using (StreamWriter sw = new StreamWriter(fileLocation))
            {
                foreach (MapParticlesParticle particle in this.Particles)
                {
                    particle.Write(sw);
                }
            }
        }
    }
}
