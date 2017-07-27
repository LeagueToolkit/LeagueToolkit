﻿using Fantome.Libraries.League.Helpers.Structures;
using System.Diagnostics;
using System.IO;

namespace Fantome.Libraries.League.IO.WGEO
{
    [DebuggerDisplay("[ {Position.X}, {Position.Y}, {Position.Z} ]")]
    public struct WGEOVertex
    {
        public Vector3 Position;
        public Vector2 UV;

        public WGEOVertex(Vector3 Position, Vector2 UV)
        {
            this.Position = Position;
            this.UV = UV;
        }

        public WGEOVertex(BinaryReader br)
        {
            this.Position = new Vector3(br);
            this.UV = new Vector2(br);
        }

        public void Write(BinaryWriter bw)
        {
            Position.Write(bw);
            UV.Write(bw);
        }
    }
}
