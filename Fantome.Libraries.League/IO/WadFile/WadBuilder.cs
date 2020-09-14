using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace Fantome.Libraries.League.IO.WadFile
{
    public class WadBuilder
    {
        public ReadOnlyDictionary<ulong, WadEntryBuilder> Entries { get; private set; }

        private Dictionary<ulong, WadEntryBuilder> _entries = new();

        public WadBuilder()
        {
            this.Entries = new(this._entries);
        }

        public WadBuilder WithEntry(WadEntryBuilder entryBuilder)
        {
            if (this._entries.ContainsKey(entryBuilder.PathXXHash))
            {
                throw new InvalidOperationException("An entry with the same XXHash has already been added: " + entryBuilder.PathXXHash);
            }
            else
            {
                this._entries.Add(entryBuilder.PathXXHash, entryBuilder);
            }

            return this;
        }

        public void Build(string fileLocation) => Build(File.OpenWrite(fileLocation));
        public void Build(Stream stream)
        {
            using Wad wad = new Wad();

            foreach (var entry in this._entries)
            {
                WadEntryBuilder entryBuilder = entry.Value;

                wad.AddEntry(
                    new WadEntry(
                        entryBuilder.PathXXHash,
                        entryBuilder.CompressedSize,
                        entryBuilder.UncompressedSize,
                        entryBuilder.EntryType,
                        entryBuilder.Sha256Checksum,
                        entryBuilder.FileRedirection,
                        entryBuilder.DataStream
                        )
                    );
            }

            wad.Write(stream);
        }
    }
}
