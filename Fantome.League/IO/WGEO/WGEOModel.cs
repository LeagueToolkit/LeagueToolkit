﻿using Fantome.Libraries.League.Helpers.Structures;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Fantome.Libraries.League.IO.WGEO
{
    public class WGEOModel
    {
        public string Texture { get; private set; }
        public string Material { get; private set; }
        public R3DSphere Sphere { get; private set; }
        public R3DBox BoundingBox { get; private set; }
        public List<WGEOVertex> Vertices { get; private set; } = new List<WGEOVertex>();
        public List<uint> Indices { get; private set; } = new List<uint>();

        public WGEOModel(string Texture, string Material, List<WGEOVertex> Vertices, List<uint> Indices)
        {
            this.Texture = Texture;
            this.Material = Material;
            this.Vertices = Vertices;
            this.Indices = Indices;
            this.BoundingBox = CalculateBoundingBox();
            this.Sphere = CalculateSphere();
        }

        public WGEOModel(BinaryReader br)
        {
            this.Texture = Encoding.ASCII.GetString(br.ReadBytes(260)).Replace("\0", "");
            this.Material = Encoding.ASCII.GetString(br.ReadBytes(64));
            this.Material = this.Material.Remove(this.Material.IndexOf("\0"));
            this.Sphere = new R3DSphere(br);
            this.BoundingBox = new R3DBox(br);

            UInt32 VertexCount = br.ReadUInt32();
            UInt32 IndexCount = br.ReadUInt32();

            for (int i = 0; i < VertexCount; i++)
            {
                this.Vertices.Add(new WGEOVertex(br));
            }
            for (int i = 0; i < IndexCount; i++)
            {
                this.Indices.Add(br.ReadUInt16());
            }
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(this.Texture.PadRight(260, '\u0000').ToCharArray());
            bw.Write(this.Material.PadRight(64, '\u0000').ToCharArray());

            Tuple<R3DSphere, R3DBox> boundingGeometry = CalculateBoundingGeometry();
            boundingGeometry.Item1.Write(bw);
            boundingGeometry.Item2.Write(bw);

            bw.Write((UInt32)this.Vertices.Count);
            bw.Write((UInt32)this.Indices.Count);

            foreach (WGEOVertex Vertex in this.Vertices)
            {
                Vertex.Write(bw);
            }
            foreach (UInt16 Index in Indices)
            {
                bw.Write(Index);
            }
        }

        public Tuple<R3DSphere, R3DBox> CalculateBoundingGeometry()
        {
            R3DBox box = CalculateBoundingBox();
            R3DSphere sphere = CalculateSphere(box);
            return new Tuple<R3DSphere, R3DBox>(sphere, box);
        }

        public R3DBox CalculateBoundingBox()
        {
            Vector3 min = this.Vertices[0].Position;
            Vector3 max = this.Vertices[0].Position;

            foreach (WGEOVertex Vertex in this.Vertices)
            {
                if (min.X > Vertex.Position.X) min.X = Vertex.Position.X;
                if (min.Y > Vertex.Position.Y) min.Y = Vertex.Position.Y;
                if (min.Z > Vertex.Position.Z) min.Z = Vertex.Position.Z;
                if (max.X < Vertex.Position.X) max.X = Vertex.Position.X;
                if (max.Y < Vertex.Position.Y) max.Y = Vertex.Position.Y;
                if (max.Z < Vertex.Position.Z) max.Z = Vertex.Position.Z;
            }

            return new R3DBox(min, max);
        }

        public R3DSphere CalculateSphere()
        {
            R3DBox box = CalculateBoundingBox();
            Vector3 centralPoint = new Vector3
                (
                0.5f * (this.BoundingBox.Max.X + this.BoundingBox.Min.X),
                0.5f * (this.BoundingBox.Max.Y + this.BoundingBox.Min.Y),
                0.5f * (this.BoundingBox.Max.Z + this.BoundingBox.Min.Z)
                );

            return new R3DSphere(centralPoint, Vector3.Distance(centralPoint, box.Max));
        }

        public R3DSphere CalculateSphere(R3DBox box)
        {
            Vector3 centralPoint = new Vector3
                (
                0.5f * (this.BoundingBox.Max.X + this.BoundingBox.Min.X),
                0.5f * (this.BoundingBox.Max.Y + this.BoundingBox.Min.Y),
                0.5f * (this.BoundingBox.Max.Z + this.BoundingBox.Min.Z)
                );

            return new R3DSphere(centralPoint, Vector3.Distance(centralPoint, box.Max));
        }
    }
}
