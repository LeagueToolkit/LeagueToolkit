using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.IO.MapParticles
{
    public class MapParticlesFile
    {
        public List<MapParticlesParticle> Particles { get; private set; } = new List<MapParticlesParticle>();

        public MapParticlesFile(string Location)
        {
            using (StreamReader sr = new StreamReader(Location))
            {
                while (sr.BaseStream.Position < sr.BaseStream.Length)
                {
                    this.Particles.Add(new MapParticlesParticle(sr));
                }
            }
        }

        public void Write(string Location)
        {
            using (StreamWriter sw = new StreamWriter(Location))
            {
                foreach (MapParticlesParticle Particle in this.Particles)
                {
                    Particle.Write(sw);
                }
            }
        }
    }
}
