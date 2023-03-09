using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance.Buffers;
using LeagueToolkit.Core.Primitives;
using LeagueToolkit.Utils.Extensions;
using System;
using System.IO;
using System.Numerics;
using System.Text;

namespace LeagueToolkit.Core.Environment
{
    public sealed class LightGrid : IDisposable
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Vector2 Bounds { get; private set; }
        public float LightScale { get; set; }
        public float FullbrightIntensity { get; set; }

        private readonly MemoryOwner<byte> _gridData;

        public bool IsDisposed { get; private set; }

        public LightGrid(int width, int height, Vector2 bounds, MemoryOwner<byte> gridData)
        {
            Guard.HasSizeEqualTo(gridData.Span, 24 * width * height, nameof(gridData));

            this.Width = width;
            this.Height = height;
            this.Bounds = bounds;
            this._gridData = gridData;
        }

        public LightGrid(string fileLocation) : this(File.OpenRead(fileLocation)) { }

        public LightGrid(Stream stream)
        {
            using BinaryReader br = new(stream, Encoding.UTF8, true);

            uint version = br.ReadUInt32();
            if (version is not 3)
                ThrowHelper.ThrowInvalidOperationException($"Invalid version: {version}");

            uint gridOffset = br.ReadUInt32();
            this.Width = br.ReadInt32();
            this.Height = br.ReadInt32();
            this.Bounds = br.ReadVector2();
            this.LightScale = br.ReadSingle();
            this.FullbrightIntensity = br.ReadSingle();

            br.BaseStream.Seek(gridOffset, SeekOrigin.Begin);
            this._gridData = MemoryOwner<byte>.Allocate(24 * this.Width * this.Height);

            br.Read(this._gridData.Span);
        }

        public void Write(string fileLocation) => Write(File.Create(fileLocation));

        public void Write(Stream stream)
        {
            using BinaryWriter bw = new(stream, Encoding.UTF8, true);

            bw.Write((uint)3);
            bw.Write((uint)32);
            bw.Write(this.Width);
            bw.Write(this.Height);
            bw.WriteVector2(this.Bounds);
            bw.Write(this.LightScale);
            bw.Write(this.FullbrightIntensity);
            bw.Write(this._gridData.Span);
        }

        public LightGridCell GetCellColor(int cell)
        {
            Span<Color> colors = stackalloc Color[6];

            for (int i = 0; i < 6; i++)
            {
                int offset = 24 * cell + i * 4;
                colors[i] = Color.Read(this._gridData.Span[offset..], ColorFormat.BgraU8) with { A = 1.0f };
            }

            return new()
            {
                C1 = colors[0],
                C2 = colors[1],
                C3 = colors[2],
                C4 = colors[3],
                C5 = colors[4],
                C6 = colors[5]
            };
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (this.IsDisposed)
                return;

            if (disposing)
                this._gridData?.Dispose();

            this.IsDisposed = true;
        }
    }

    public struct LightGridCell
    {
        public Vector4 C1;
        public Vector4 C2;
        public Vector4 C3;
        public Vector4 C4;
        public Vector4 C5;
        public Vector4 C6;
    }
}
