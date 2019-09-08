using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.FX
{
    public class FXFile
    {
        public List<FXTrack> Tracks { get; private set; } = new List<FXTrack>();
        public List<string> TargetBones { get; private set; } = new List<string>();
        public FXFile(string fileLocation)
            : this(File.OpenRead(fileLocation))
        {

        }
        public FXFile(Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream))
            {
                for (int i = 0; i < 8; i++)
                {
                    this.Tracks.Add(new FXTrack(br));
                }

                if (br.BaseStream.Position != br.BaseStream.Length)
                {
                    uint version = br.ReadUInt32();
                    if (version != 1)
                    {
                        throw new Exception("This version is not supported");
                    }

                    uint flag = br.ReadUInt32();
                    uint targetBoneCount = br.ReadUInt32();

                    if ((flag & 1) == 1)
                    {
                        for (int i = 0; i < targetBoneCount; i++)
                        {
                            string targetBone = Encoding.ASCII.GetString(br.ReadBytes(64));
                            this.TargetBones.Add(targetBone.Remove(targetBone.IndexOfAny(new char[] { '\u0000', (char)0xCD })));
                        }
                    }
                }
            }
        }

        public void Write(string fileLocation)
        {
            Write(File.Create(fileLocation));
        }
        public void Write(Stream stream)
        {
            using (BinaryWriter bw = new BinaryWriter(stream))
            {
                foreach (FXTrack track in this.Tracks)
                {
                    track.Write(bw);
                }
                bw.Write((uint)1);
                bw.Write(this.TargetBones.Count != 0 ? 1 : 0);
                bw.Write((uint)this.Tracks.Count);
                foreach (string targetBone in this.TargetBones)
                {
                    bw.Write(targetBone.PadRight(64, '\u0000').ToCharArray());
                }
            }
        }
    }
}
