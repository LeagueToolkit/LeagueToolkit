using LeagueToolkit.Helpers.Extensions;
using LeagueToolkit.Helpers.Structures;
using System.IO;
using System.Numerics;

namespace LeagueToolkit.IO.MapGeometry
{
    public class MapGeometryUnkMatrixBBVec
    {
        public R3DMatrix44 UnknownMatrix { get; set; }
        public R3DBox UnknownBoundingBox { get; set; }
        public Vector3 UnknownVector { get; set; }

        public MapGeometryUnkMatrixBBVec() { }
        public MapGeometryUnkMatrixBBVec(BinaryReader br)
        {
            this.UnknownMatrix = new(br);
            this.UnknownBoundingBox = new(br);
            this.UnknownVector = br.ReadVector3();
        }

        public void Write(BinaryWriter bw)
        {
            this.UnknownMatrix.Write(bw);
            this.UnknownBoundingBox.Write(bw);
            bw.WriteVector3(this.UnknownVector);
        }
    }
}
