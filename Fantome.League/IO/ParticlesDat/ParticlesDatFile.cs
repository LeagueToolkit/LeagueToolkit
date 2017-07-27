using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.IO.ParticlesDat
{
    public class ParticlesDatFile
    {
        public List<ParticlesDatParticle> Particles { get; private set; } = new List<ParticlesDatParticle>();

        public ParticlesDatFile(string Location)
        {
            using (StreamReader sr = new StreamReader(Location))
            {
                while (sr.BaseStream.Position < sr.BaseStream.Length)
                {
                    this.Particles.Add(new ParticlesDatParticle(sr));
                }
            }
        }

        public void Write(string Location)
        {
            using (StreamWriter sw = new StreamWriter(Location))
            {
                foreach (ParticlesDatParticle Particle in this.Particles)
                {
                    Particle.Write(sw);
                }
            }
        }
    }
}
