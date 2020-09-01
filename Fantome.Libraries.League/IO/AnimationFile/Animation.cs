using Fantome.Libraries.League.Helpers.Extensions;
using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

using Vector3 = System.Numerics.Vector3;
using Quaternion = System.Numerics.Quaternion;

namespace Fantome.Libraries.League.IO.AnimationFile
{
    public class Animation
    {
        public float FrameDuration { get; private set; }
        public float FPS => 1 / this.FrameDuration;

        public List<AnimationTrack> Tracks { get; private set; } = new();

        private List<List<ushort>> _jumpCaches = new();

        public Animation(string fileLocation) : this(File.OpenRead(fileLocation)) { }
        public Animation(Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream))
            {
                string magic = Encoding.ASCII.GetString(br.ReadBytes(8));
                uint version = br.ReadUInt32();

                if (magic == "r3d2canm")
                {
                    ReadCompressed(br);
                }
                else if (magic == "r3d2anmd")
                {
                    if (version == 5)
                    {
                        ReadV5(br);
                    }
                    else if (version == 4)
                    {
                        ReadV4(br);
                    }
                    else
                    {
                        ReadLegacy(br);
                    }
                }
                else
                {
                    throw new Exception("Invalid file signature: " + magic);
                }
            }
        }

        private void ReadCompressed(BinaryReader br)
        {
            uint resourceSize = br.ReadUInt32();
            uint formatToken = br.ReadUInt32();
            uint flags = br.ReadUInt32(); // 7 ?

            int jointCount = br.ReadInt32();
            int frameCount = br.ReadInt32();
            int jumpCacheCount = br.ReadInt32();

            float duration = br.ReadSingle();
            this.FrameDuration = 1 / br.ReadSingle();

            TransformQuantizationProperties rotationQuantizationProperties = new(br);
            TransformQuantizationProperties translationQuantizationProperties = new(br);
            TransformQuantizationProperties scaleQuantizationProperties = new(br);

            Vector3 translationMin = br.ReadVector3();
            Vector3 translationMax = br.ReadVector3();

            Vector3 scaleMin = br.ReadVector3();
            Vector3 scaleMax = br.ReadVector3();

            int framesOffset = br.ReadInt32();
            int jumpCachesOffset = br.ReadInt32(); // 5328
            int jointNameHashesOffset = br.ReadInt32();

            if (framesOffset <= 0) throw new Exception("Animation does not contain Frame data");
            if (jumpCachesOffset <= 0) throw new Exception("Animation does not contain jump cache data");
            if (jointNameHashesOffset <= 0) throw new Exception("Animation does not contain joint data");

            // Read joint hashes
            br.BaseStream.Seek(jointNameHashesOffset + 12, SeekOrigin.Begin);
            List<uint> jointHashes = new(jointCount);
            for(int i = 0; i < jointCount; i++)
            {
                jointHashes.Add(br.ReadUInt32());
            }

            // Read frames
            br.BaseStream.Seek(framesOffset + 12, SeekOrigin.Begin);
            Dictionary<uint, Dictionary<float, Vector3>> translations = new(jointCount);
            Dictionary<uint, Dictionary<float, Vector3>> scales = new(jointCount);
            Dictionary<uint, Dictionary<float, Quaternion>> rotations = new(jointCount);
            
            for(int i = 0; i < jointCount; i++)
            {
                uint jointHash = jointHashes[i];

                translations.Add(jointHash, new Dictionary<float, Vector3>());
                scales.Add(jointHash, new Dictionary<float, Vector3>());
                rotations.Add(jointHash, new Dictionary<float, Quaternion>());

                this.Tracks.Add(new AnimationTrack(jointHash));
            }
            
            for(int i = 0; i < frameCount; i++)
            {
                ushort compressedTime = br.ReadUInt16();
                byte jointHashIndex = br.ReadByte();
                CompressedTransformType transformType = (CompressedTransformType)br.ReadByte();
                byte[] compressedTransform = br.ReadBytes(6);

                uint jointHash = jointHashes[jointHashIndex];
                float uncompressedTime = UncompressFrameTime(compressedTime, duration);

                if (transformType == CompressedTransformType.Rotation)
                {
                    Quaternion rotation = new QuantizedQuaternion(compressedTransform).Decompress();

                    if(!rotations[jointHash].ContainsKey(uncompressedTime))
                    {
                        rotations[jointHash].Add(uncompressedTime, rotation);
                    }
                }
                else if(transformType == CompressedTransformType.Translation)
                {
                    Vector3 translation = UncompressVector3(translationMin, translationMax, compressedTransform);

                    if (!translations[jointHash].ContainsKey(uncompressedTime))
                    {
                        translations[jointHash].Add(uncompressedTime, translation);
                    }
                }
                else if(transformType == CompressedTransformType.Scale)
                {
                    Vector3 scale = UncompressVector3(scaleMin, scaleMax, compressedTransform);

                    if (!scales[jointHash].ContainsKey(uncompressedTime))
                    {
                        scales[jointHash].Add(uncompressedTime, scale);
                    }
                }
                else
                {
                    throw new Exception("Encountered unknown transform type: " + transformType);
                }
            }

            // Build quantized tracks
            for(int i = 0; i < jointCount; i++)
            {
                uint jointHash = jointHashes[i];
                AnimationTrack track = this.Tracks.First(x => x.JointHash == jointHash);

                track.Translations = translations[jointHash];
                track.Scales = scales[jointHash];
                track.Rotations = rotations[jointHash];
            }

            // Read jump caches
            //br.BaseStream.Seek(jumpCachesOffset + 12, SeekOrigin.Begin);
            //for(int i = 0; i < jumpCacheCount; i++)
            //{
            //    int count = 1332;
            //
            //    this._jumpCaches.Add(new List<ushort>());
            //    for(int j = 0; j < count; j++)
            //    {
            //        this._jumpCaches[i].Add(br.ReadUInt16());
            //    }
            //}

            DequantizeAnimationChannels(rotationQuantizationProperties, translationQuantizationProperties, scaleQuantizationProperties);
        }

        private void ReadV4(BinaryReader br)
        {
            throw new NotImplementedException();

            uint resourceSize = br.ReadUInt32();
            uint formatToken = br.ReadUInt32();
            uint version = br.ReadUInt32();
            uint flags = br.ReadUInt32();

            uint trackCount = br.ReadUInt32();
            uint frameCount = br.ReadUInt32();
            this.FrameDuration = br.ReadSingle();

            int tracksOffset = br.ReadInt32();
            int assetNameOffset = br.ReadInt32();
            int timeOffset = br.ReadInt32();
            int vectorsOffset = br.ReadInt32();
            int rotationsOffset = br.ReadInt32();
            int framesOffset = br.ReadInt32();

            if (tracksOffset <= 0) throw new Exception("Animation does not contain track data");
            if (vectorsOffset <= 0) throw new Exception("Animation does not contain Vector data");
            if (rotationsOffset <= 0) throw new Exception("Animation does not contain Rotation data");
            if (framesOffset <= 0) throw new Exception("Animation does not contain Frame data");

        }

        private void ReadV5(BinaryReader br)
        {
            uint resourceSize = br.ReadUInt32();
            uint formatToken = br.ReadUInt32();
            uint version = br.ReadUInt32();
            uint flags = br.ReadUInt32();

            int trackCount = br.ReadInt32();
            int framesPerTrack = br.ReadInt32();
            this.FrameDuration = br.ReadSingle();

            int jointHashesOffset = br.ReadInt32();
            int assetNameOffset = br.ReadInt32();
            int timeOffset = br.ReadInt32();
            int vectorsOffset = br.ReadInt32();
            int rotationsOffset = br.ReadInt32();
            int framesOffset = br.ReadInt32();

            if (jointHashesOffset <= 0) throw new Exception("Animation does not contain Joint hashes");
            if (vectorsOffset <= 0) throw new Exception("Animation does not contain Vector data");
            if (rotationsOffset <= 0) throw new Exception("Animation does not contain Rotation data");
            if (framesOffset <= 0) throw new Exception("Animation does not contain Frame data");

            int jointHashesCount = (framesOffset - jointHashesOffset) / sizeof(uint);
            int vectorsCount = (rotationsOffset - vectorsOffset) / 12;
            int rotationsCount = (jointHashesOffset - rotationsOffset) / 6;

            List<uint> jointHashes = new(jointHashesCount);
            List<Vector3> vectors = new(vectorsCount);
            List<Quaternion> rotations = new(rotationsCount);
            var frames = new List<(ushort, ushort, ushort)>(framesPerTrack * trackCount);

            // Read Joint Hashes
            br.BaseStream.Seek(jointHashesOffset + 12, SeekOrigin.Begin);
            for (int i = 0; i < jointHashesCount; i++)
            {
                jointHashes.Add(br.ReadUInt32());
            }

            // Read Vectors
            br.BaseStream.Seek(vectorsOffset + 12, SeekOrigin.Begin);
            for (int i = 0; i < vectorsCount; i++)
            {
                vectors.Add(br.ReadVector3());
            }

            // Read Rotations
            br.BaseStream.Seek(rotationsOffset + 12, SeekOrigin.Begin);
            for (int i = 0; i < rotationsCount; i++)
            {
                rotations.Add(new QuantizedQuaternion(br.ReadBytes(6)).Decompress().Normalized());
            }

            // Read Frames
            br.BaseStream.Seek(framesOffset + 12, SeekOrigin.Begin);
            for (int i = 0; i < framesPerTrack * trackCount; i++)
            {
                frames.Add((br.ReadUInt16(), br.ReadUInt16(), br.ReadUInt16()));
            }

            // Create tracks
            for (int i = 0; i < trackCount; i++)
            {
                this.Tracks.Add(new AnimationTrack(jointHashes[i]));
            }

            for(int t = 0; t < trackCount; t++)
            {
                AnimationTrack track = this.Tracks[t];
                float currentTime = 0;
                for (int f = 0; f < framesPerTrack; f++)
                {
                    (int translationIndex, int scaleIndex, int rotationIndex) = frames[f * trackCount + t];

                    track.Translations.Add(currentTime, vectors[translationIndex]);
                    track.Scales.Add(currentTime, vectors[scaleIndex]);
                    track.Rotations.Add(currentTime, rotations[rotationIndex]);

                    currentTime += this.FrameDuration;
                }
            }
        }

        private void ReadLegacy(BinaryReader br)
        {
            throw new NotImplementedException();
        }

        private Vector3 UncompressVector3(Vector3 min, Vector3 max, byte[] compressedData)
        {
            Vector3 uncompressed = max - min;
            ushort cX = (ushort)(compressedData[0] | compressedData[1] << 8);
            ushort cY = (ushort)(compressedData[2] | compressedData[3] << 8);
            ushort cZ = (ushort)(compressedData[4] | compressedData[5] << 8);

            uncompressed.X *= (cX / 65535.0f);
            uncompressed.Y *= (cY / 65535.0f);
            uncompressed.Z *= (cZ / 65535.0f);

            uncompressed += min;

            return uncompressed;
        }
        private float UncompressFrameTime(ushort compressedTime, float animationLength)
        {
            return (compressedTime / 65535.0f) * animationLength;
        }

        // ------------ DEQUANTIZATION ------------ \\
        private void DequantizeAnimationChannels(
            TransformQuantizationProperties rotationQuantizationProperties,
            TransformQuantizationProperties translationQuantizationProperties,
            TransformQuantizationProperties scaleQuantizationProperties)
        {
            // TODO

            //TranslationDequantizationRound(translationQuantizationProperties);
        }
        private void TranslationDequantizationRound(TransformQuantizationProperties quantizationProperties)
        {
            foreach (AnimationTrack track in this.Tracks)
            {
                List<(float, Vector3)> interpolatedFrames = new();
                for (int i = 0; i < track.Translations.Count; i++)
                {
                    // Do not process last frame
                    if (i + 1 >= track.Translations.Count) return;

                    var frameA = track.Translations.ElementAt(i + 0);
                    var frameB = track.Translations.ElementAt(i + 1);

                    // Check if interpolation is needed

                    interpolatedFrames.Add(InterpolateTranslationFrames((frameA.Key, frameA.Value), (frameB.Key, frameB.Value)));
                }

                foreach ((float time, Vector3 value) in interpolatedFrames)
                {
                    track.Translations.Add(time, value);
                }
            }
        }

        private (float, Vector3) InterpolateTranslationFrames((float, Vector3) a, (float, Vector3) b)
        {
            float time = (a.Item1 + b.Item1) / 2;
            return (time, Vector3.Lerp(a.Item2, b.Item2, 0.5f));
        }

        private struct TransformQuantizationProperties
        {
            internal float ErrorMargin { get; private set; }
            internal float DiscontinuityThreshold { get; private set; }

            internal TransformQuantizationProperties(BinaryReader br)
            {
                this.ErrorMargin = br.ReadSingle();
                this.DiscontinuityThreshold = br.ReadSingle();
            }
        }
        private enum CompressedTransformType : byte
        {
            Rotation = 0,
            Translation = 64,
            Scale = 128
        }
    }
}
