using Fantome.League.Helpers.Structures;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fantome.League.IO.WGEO
{
    public class WGEOModel
    {
        public string Texture { get; private set; }
        public string Material { get; private set; }
        public WGEOSphere Sphere { get; private set; }
        public Vector3 Min { get; private set; }
        public Vector3 Max { get; private set; }
        public List<WGEOVertex> Vertices { get; private set; } = new List<WGEOVertex>();
        public List<UInt16> Indices { get; private set; } = new List<UInt16>();
        public WGEOModel(BinaryReader br)
        {
            this.Texture = Encoding.ASCII.GetString(br.ReadBytes(260)).Replace("\0", "");
            this.Material = Encoding.ASCII.GetString(br.ReadBytes(64));
            this.Material = this.Material.Remove(this.Material.IndexOf("\0"));
            this.Sphere = new WGEOSphere(br);
            this.Min = new Vector3(br);
            this.Max = new Vector3(br);

            UInt32 VertexCount = br.ReadUInt32();
            UInt32 IndexCount = br.ReadUInt32();

            for(int i = 0; i < VertexCount; i++)
            {
                this.Vertices.Add(new WGEOVertex(br));
            }
            for(int i = 0; i < IndexCount; i++)
            {
                this.Indices.Add(br.ReadUInt16());
            }
        }
        public void Write(BinaryWriter bw)
        {
            bw.Write(this.Texture.PadRight(260, '\u0000').ToCharArray());
            bw.Write(this.Material.PadRight(64, '\u0000').ToCharArray());
            this.Sphere.Write(bw);
            this.Min.Write(bw);
            this.Max.Write(bw);

            bw.Write((UInt32)this.Vertices.Count);
            bw.Write((UInt32)this.Indices.Count);

            foreach(WGEOVertex Vertex in this.Vertices)
            {
                Vertex.Write(bw);
            }
            foreach(UInt16 Index in Indices)
            {
                bw.Write(Index);
            }
        }
    }
}
