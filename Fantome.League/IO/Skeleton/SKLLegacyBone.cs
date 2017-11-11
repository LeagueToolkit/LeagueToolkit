using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Fantome.Libraries.League.Helpers.Structures;

namespace Fantome.Libraries.League.IO.Skeleton
{
    /// <summary>
    /// Represents a Legacy SKL Bone
    /// </summary>
    public class SKLLegacyBone : SKLBone
    {
        /// <summary>
        /// The ID of this <see cref="SKLLegacyBone"/>
        /// </summary>
        public short _id;

        /// <summary>
        /// The Parent ID of this <see cref="SKLLegacyBone"/>
        /// </summary>
        internal short _parentID;

        /// <summary>
        /// The Scale of this <see cref="SKLLegacyBone"/>
        /// </summary>
        public float Scale { get; set; }

        /// <summary>
        /// The Global Transformation Matrix of this <see cref="SKLLegacyBone"/>
        /// </summary>
        public R3DMatrix44 GlobalMatrix { get; set; }

        /// <summary>
        /// The Local Transformation Matrix of this <see cref="SKLLegacyBone"/>
        /// </summary>
        public R3DMatrix44 LocalMatrix { get; set; }

        public SKLLegacyBone(BinaryReader br, int id)
        {
            this._id = (short)id;
            this.Name = Encoding.ASCII.GetString(br.ReadBytes(32)).Replace("\0", "");
            this._parentID = (short)br.ReadInt32();
            this.Scale = br.ReadSingle() * 10;
            this.GlobalMatrix = new R3DMatrix44(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(),
                br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(),
                br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(),
                0, 0, 0, 1
                );
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(Encoding.ASCII.GetBytes(this.Name));
            bw.Write((int)this._parentID);
            bw.Write(this.Scale * 0.1);

            bw.Write(this.GlobalMatrix.M11);
            bw.Write(this.GlobalMatrix.M12);
            bw.Write(this.GlobalMatrix.M13);
            bw.Write(this.GlobalMatrix.M14);
            bw.Write(this.GlobalMatrix.M21);
            bw.Write(this.GlobalMatrix.M22);
            bw.Write(this.GlobalMatrix.M23);
            bw.Write(this.GlobalMatrix.M24);
            bw.Write(this.GlobalMatrix.M31);
            bw.Write(this.GlobalMatrix.M32);
            bw.Write(this.GlobalMatrix.M33);
            bw.Write(this.GlobalMatrix.M34);

        }

        /// <summary>
        /// Gets the Position of this <see cref="SKLLegacyBone"/> from it's Global Matrix
        /// </summary>
        public Vector3 GetPosition()
        {
            return new Vector3(this.GlobalMatrix.M14, this.GlobalMatrix.M24, this.GlobalMatrix.M34);
        }

        /// <summary>
        /// Gets the Rotation of this <see cref="SKLLegacyBone"/> in a <see cref="Quaternion"/>
        /// </summary>
        /// <returns></returns>
        public Quaternion GetRotation()
        {
            float trace = 1 + this.LocalMatrix.M11 + this.LocalMatrix.M22 + this.LocalMatrix.M33;
            float s = 0;
            float x = 0;
            float y = 0;
            float z = 0;
            float w = 0;

            if (trace > 0.0000001)
            {
                s = (float)Math.Sqrt(trace) * 2;
                x = (this.LocalMatrix.M23 - this.LocalMatrix.M32) / s;
                y = (this.LocalMatrix.M31 - this.LocalMatrix.M13) / s;
                z = (this.LocalMatrix.M12 - this.LocalMatrix.M21) / s;
                w = 0.25f * s;
            }
            else
            {
                if (this.LocalMatrix.M11 > this.LocalMatrix.M22 && this.LocalMatrix.M11 > this.LocalMatrix.M33)
                {
                    s = (float)Math.Sqrt(1.0 + this.LocalMatrix.M11 - this.LocalMatrix.M22 - this.LocalMatrix.M33) * 2;
                    x = 0.25f * s;
                    y = (this.LocalMatrix.M12 + this.LocalMatrix.M21) / s;
                    z = (this.LocalMatrix.M31 + this.LocalMatrix.M13) / s;
                    w = (this.LocalMatrix.M23 - this.LocalMatrix.M32) / s;
                }
                else if (this.LocalMatrix.M22 > this.LocalMatrix.M33)
                {
                    s = (float)Math.Sqrt(1.0 + this.LocalMatrix.M22 - this.LocalMatrix.M11 - this.LocalMatrix.M33) * 2;
                    x = (this.LocalMatrix.M12 + this.LocalMatrix.M21) / s;
                    y = 0.25f * s;
                    z = (this.LocalMatrix.M23 + this.LocalMatrix.M32) / s;
                    w = (this.LocalMatrix.M31 - this.LocalMatrix.M13) / s;
                }
                else
                {
                    s = (float)Math.Sqrt(1.0 + this.LocalMatrix.M33 - this.LocalMatrix.M11 - this.LocalMatrix.M22) * 2;
                    x = (this.LocalMatrix.M31 + this.LocalMatrix.M13) / s;
                    y = (this.LocalMatrix.M23 + this.LocalMatrix.M32) / s;
                    z = 0.25f * s;
                    w = (this.LocalMatrix.M12 - this.LocalMatrix.M21) / s;
                }
            }

            return new Quaternion(x, y, z, w);
        }
    }
}
