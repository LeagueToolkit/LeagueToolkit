using System.Collections.Generic;
using System.Numerics;

namespace LeagueToolkit.IO.StaticObjectFile
{
    public class StaticObjectSubmesh
    {
        public string Name { get; set; }
        public List<StaticObjectVertex> Vertices { get; private set; }
        public List<uint> Indices { get; private set; }

        public StaticObjectSubmesh(string name, List<StaticObjectVertex> vertices, List<uint> indices)
        {
            this.Name = name;
            this.Vertices = vertices;
            this.Indices = indices;
        }

        internal List<StaticObjectFace> GetFaces()
        {
            List<StaticObjectFace> faces = new List<StaticObjectFace>();

            for(int i = 0; i < this.Indices.Count; i += 3)
            {
                uint[] indices = { this.Indices[i], this.Indices[i + 1], this.Indices[i + 2] };
                Vector2[] uvs = 
                {
                    this.Vertices[(int)indices[0]].UV,
                    this.Vertices[(int)indices[1]].UV,
                    this.Vertices[(int)indices[2]].UV
                };

                faces.Add(new StaticObjectFace(indices, this.Name, uvs));
            }

            return faces;
        }
    }
}
