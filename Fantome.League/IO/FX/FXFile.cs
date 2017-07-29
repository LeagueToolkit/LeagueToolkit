using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Fantome.Libraries.League.Helpers.Exceptions;

namespace Fantome.Libraries.League.IO.FX
{
    public class FXFile
    {
        public List<FXTrack> Tracks { get; private set; } = new List<FXTrack>();
        public List<string> TargetBones { get; private set; } = new List<string>();

        public FXFile(string Location)
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(Location)))
            {
                for (int i = 0; i < 8; i++)
                {
                    this.Tracks.Add(new FXTrack(br));
                }

                if (br.BaseStream.Position != br.BaseStream.Length)
                {
                    UInt32 Version = br.ReadUInt32();
                    if (Version != 1)
                        throw new UnsupportedFileVersionException();

                    UInt32 Flag = br.ReadUInt32();
                    UInt32 TargetBoneCount = br.ReadUInt32();

                    if ((Flag & 1) == 1)
                    {
                        for (int i = 0; i < TargetBoneCount; i++)
                        {
                            string TargetBone = Encoding.ASCII.GetString(br.ReadBytes(64));
                            this.TargetBones.Add(TargetBone.Remove(TargetBone.IndexOfAny(new char[] { '\u0000', (char)0xCD })));
                        }
                    }
                }
            }
        }

        public void Write(string Location)
        {
            using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(Location)))
            {
                foreach (FXTrack Track in this.Tracks)
                {
                    Track.Write(bw);
                }
                bw.Write((UInt32)1);
                bw.Write((UInt32)1);
                bw.Write((UInt32)this.Tracks.Count);
                foreach (string TargetBone in this.TargetBones)
                {
                    bw.Write(TargetBone.PadRight(64, '\u0000').ToCharArray());
                }
            }
        }
    }
}
