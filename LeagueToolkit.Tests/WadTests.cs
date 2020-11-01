using LeagueToolkit.Helpers;
using LeagueToolkit.IO.WadFile;
using NUnit.Framework;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;

namespace LeagueToolkit.Tests
{
    [TestFixture("Aatrox.wad.client", Author = "Crauzer", Category = "WAD")]
    [TestFixture("Yone.wad.client", Author = "Crauzer", Category = "WAD")]
    [TestFixture("Aphelios.wad.client", Author = "Crauzer", Category = "WAD")]
    public class WadTests
    {
        private const string WAD_DIR = "files/wad";

        private string _wadName;
        private Wad _originalWad;
        private Wad _dataStreamWad;
        private Wad _fileStreamWad;

        public WadTests(string wadName)
        {
            this._wadName = wadName;

            Directory.CreateDirectory("temp");
        }

        [Test, Order(0)]
        public void TestReadOriginalWad()
        {
            // Test whether we can read the file
            Assert.DoesNotThrow(delegate
            {
                this._originalWad = Wad.Mount(Path.Combine(WAD_DIR, this._wadName), true);
            },
            "Failed to read the file");

            Assert.Pass();
        }

        [Test, Order(1)]
        public void TestOriginalWadEntryDataHandleOperations()
        {
            TestWadEntryDataHandleOperations(this._originalWad);
        }

        [Test, Order(2)]
        public void TestDataStreamRebuildWad()
        {
            WadBuilder wadBuilder = new WadBuilder();

            // Build entries
            foreach (WadEntry entry in this._originalWad.Entries.Values)
            {
                WadEntryBuilder entryBuilder = new WadEntryBuilder();

                entryBuilder
                    .WithPathXXHash(entry.XXHash);

                if (entry.Type == WadEntryType.GZipCompressed)
                {
                    entryBuilder.WithGZipDataStream(entry.GetDataHandle().GetCompressedStream(), entry.CompressedSize, entry.UncompressedSize);
                }
                else if (entry.Type == WadEntryType.ZStandardCompressed)
                {
                    entryBuilder.WithZstdDataStream(entry.GetDataHandle().GetCompressedStream(), entry.CompressedSize, entry.UncompressedSize);
                }
                else if (entry.Type == WadEntryType.Uncompressed)
                {
                    entryBuilder.WithUncompressedDataStream(entry.GetDataHandle().GetDecompressedStream());
                }
                else if (entry.Type == WadEntryType.FileRedirection)
                {
                    entryBuilder.WithFileRedirection(entry.FileRedirection);
                }

                wadBuilder.WithEntry(entryBuilder);
            }

            MemoryStream rebuiltWadStream = new MemoryStream();
            Assert.DoesNotThrow(delegate
            {
                wadBuilder.Build(rebuiltWadStream, true);
            }, "Failed to build WAD using data streams");

            rebuiltWadStream.Seek(0, SeekOrigin.Begin);
            this._dataStreamWad = Wad.Mount(rebuiltWadStream, true);
        }

        [Test, Order(2)]
        public void TestFileStreamRebuildWad()
        {
            WadBuilder wadBuilder = new WadBuilder();

            // Build entries
            foreach (WadEntry entry in this._originalWad.Entries.Values)
            {
                WadEntryBuilder entryBuilder = new WadEntryBuilder();
                Stream entryDecompressedStream = entry.GetDataHandle().GetDecompressedStream();
                string entryFileName = "temp/" + entry.XXHash + "." + Utilities.GetExtension(Utilities.GetExtensionType(entryDecompressedStream));

                // Extract entry data to temporary file
                using (FileStream writeEntryFileStream = File.OpenWrite(entryFileName))
                {
                    entry.GetDataHandle().GetDecompressedStream().CopyTo(writeEntryFileStream);
                }

                entryBuilder
                    .WithPathXXHash(entry.XXHash)
                    .WithFileDataStream(entryFileName);

                wadBuilder.WithEntry(entryBuilder);
            }

            MemoryStream rebuiltWadStream = new MemoryStream();
            Assert.DoesNotThrow(delegate
            {
                wadBuilder.Build(rebuiltWadStream, true);
            }, "Failed to build WAD using files");

            rebuiltWadStream.Seek(0, SeekOrigin.Begin);
            this._fileStreamWad = Wad.Mount(rebuiltWadStream, true);
        }

        [Test, Order(4)]
        public void TestDataStreamRebuiltWadEntryDataHandleOperations()
        {
            TestWadEntryDataHandleOperations(this._dataStreamWad);
        }

        [Test, Order(4)]
        public void TestFileStreamRebuiltWadEntryDataHandleOperations()
        {
            TestWadEntryDataHandleOperations(this._fileStreamWad);
        }

        private void TestWadEntryDataHandleOperations(Wad wad)
        {
            foreach (WadEntry entry in wad.Entries.Values)
            {
                // Ignore file redirections
                if (entry.Type == WadEntryType.FileRedirection) return;

                WadEntryDataHandle dataHandle = entry.GetDataHandle();
                Stream comressedDataStream = null;
                Stream decompressedDataStream = null;

                Assert.DoesNotThrow(delegate
                {
                    comressedDataStream = dataHandle.GetCompressedStream();
                }, "Failed to get compressed stream");

                Assert.DoesNotThrow(delegate
                {
                    decompressedDataStream = dataHandle.GetDecompressedStream();
                }, "Failed to get decompressed data stream");

                Assert.AreEqual((int)comressedDataStream.Length, entry.CompressedSize);
                Assert.AreEqual((int)decompressedDataStream.Length, entry.UncompressedSize);

                // Verify SHA checksum
                using (SHA256 sha = SHA256.Create())
                {
                    byte[] computedHash = sha.ComputeHash(comressedDataStream).Take(8).ToArray();

                    Assert.IsTrue(computedHash.SequenceEqual(entry.SHA), $"Entry ({entry.XXHash}) checksum does not match computed one");
                }
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            this._originalWad?.Dispose();
            this._dataStreamWad?.Dispose();
            this._fileStreamWad?.Dispose();
        }
    }
}