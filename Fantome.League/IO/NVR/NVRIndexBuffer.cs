using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fantome.Libraries.League.IO.NVR
{
    public class NVRIndexBuffer
    {
        public D3DFORMAT Format { get; private set; }
        public List<int> Indices { get; private set; } = new List<int>();
        public int CurrentMax { get; private set; } = -1;

        public NVRIndexBuffer(D3DFORMAT format)
        {
            this.Format = format;
        }

        public void AddIndex(int indexToAdd)
        {
            this.Indices.Add(indexToAdd);
            if (this.CurrentMax < indexToAdd)
            {
                this.CurrentMax = indexToAdd;
            }
        }

        public void Write(BinaryWriter bw)
        {
            // Calculate length
            int indexLength = this.Format == D3DFORMAT.D3DFMT_INDEX16 ? 2 : 4;
            bw.Write(indexLength * this.Indices.Count);
            bw.Write((int)this.Format);
            foreach (int index in this.Indices)
            {
                if (indexLength == 2)
                {
                    bw.Write((ushort)index);
                }
                else
                {
                    bw.Write(index);
                }
            }
        }

        public NVRIndexBuffer(BinaryReader br)
        {
            int length = br.ReadInt32();
            this.Format = (D3DFORMAT)br.ReadInt32();
            if (this.Format == D3DFORMAT.D3DFMT_INDEX16)
            {
                // 16-bit indices, all tested NVRs use this
                int indicesCount = length / 2;
                for (int i = 0; i < indicesCount; i++)
                {
                    this.Indices.Add(br.ReadUInt16());
                }
            }
            else if (this.Format == D3DFORMAT.D3DFMT_INDEX32)
            {
                // 32-bit indices, never seen a NVR using this yet
                int indicesCount = length / 4;
                for (int i = 0; i < indicesCount; i++)
                {
                    this.Indices.Add(br.ReadInt32());
                }
            }
            else
            {
                throw new UnsupportedD3DFORMATException(this.Format);
            }
        }
    }

    public class UnsupportedD3DFORMATException : Exception
    {
        public UnsupportedD3DFORMATException(D3DFORMAT actual) : base(String.Format("This D3DFORMAT ({0}) is not supported.", actual)) { }
    }
}