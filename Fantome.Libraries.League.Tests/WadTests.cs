using Fantome.Libraries.League.IO.WadFile;
using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Fantome.Libraries.League.Tests
{
    public class WadTests
    {
        private const string WAD_DIR = "files/wad";

        [SetUp]
        public void Setup()
        {

        }

        [TestCase("Aatrox.wad.client", Author = "Crauzer", Category = "WAD")]
        [TestCase("Yone.wad.client", Author = "Crauzer", Category = "WAD")]
        [TestCase("Aphelios.wad.client", Author = "Crauzer", Category = "WAD")]
        public void TestWad(string wadName)
        {
            Wad wad = null;

            // Test whether we can read the file
            Assert.DoesNotThrow(delegate
            {
                wad = Wad.Mount(Path.Combine(WAD_DIR, wadName), true);
            },
            "Failed to read the file");

            // Test entry handle operations
            foreach (WadEntry entry in wad.Entries.Values)
            {
                TestEntryDataHandleOperations(entry);
            }

            // Rebuild the WAD
            TestRebuild(wad);

            Assert.Pass();
        }

        public void TestEntryDataHandleOperations(WadEntry entry)
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

                Assert.IsTrue(computedHash.SequenceEqual(entry.SHA), "Entry checksum does not match computed one");
            }
        }

        public void TestRebuild(Wad wad)
        {
            WadBuilder wadBuilder = new WadBuilder();

            TestDataStreamRebuild();

            void TestDataStreamRebuild()
            {
                // Build entries
                foreach (WadEntry entry in wad.Entries.Values)
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

                using MemoryStream rebuiltWadStream = new MemoryStream();
                Assert.DoesNotThrow(delegate
                {
                    wadBuilder.Build(rebuiltWadStream);
                }, "Failed to build WAD using data streams");

                File.WriteAllBytes("test.wad.client", rebuiltWadStream.ToArray());

                TestRebuiltWad(rebuiltWadStream);
            }

            void TestRebuiltWad(Stream wadStream)
            {
                wadStream.Seek(0, SeekOrigin.Begin);

                // Test whether we can read the file
                Assert.DoesNotThrow(delegate
                {
                    Wad.Mount(wadStream, false);
                },
                "Failed to read the file");
            }
        }
    }
}