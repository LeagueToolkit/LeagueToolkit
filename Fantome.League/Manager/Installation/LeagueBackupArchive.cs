using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Fantome.Libraries.League.Manager.Installation
{
    internal class LeagueBackupArchive : IDisposable
    {
        private string _zipArchivePath;
        private ZipArchive _zipArchive;

        public LeagueBackupArchive(string zipArchivePath)
        {
            _zipArchivePath = zipArchivePath;
            if (File.Exists(_zipArchivePath))
            {
                _zipArchive = new ZipArchive(new FileStream(_zipArchivePath, FileMode.Open), ZipArchiveMode.Update);
            }
        }

        public bool HasFile(string fileBackupPath)
        {
            return (GetFileEntry(fileBackupPath) != null);
        }

        public void AddFile(string fileBackupPath, Stream content)
        {
            if (!HasFile(fileBackupPath))
            {
                InitZipArchive();
                ZipArchiveEntry newEntry = _zipArchive.CreateEntry(fileBackupPath);
                using (Stream entryStream = newEntry.Open())
                {
                    content.Seek(0, SeekOrigin.Begin);
                    content.CopyTo(entryStream);
                }
            }
            else
            {
                throw new BackupFileAlreadyExistsException();
            }
        }

        public Stream GetBackupFileStream(string fileBackupPath)
        {
            ZipArchiveEntry fileEntry = GetFileEntry(fileBackupPath);
            if (fileEntry != null)
            {
                return fileEntry.Open();
            }
            else
            {
                throw new BackupFileNotFoundException();
            }
        }

        private void InitZipArchive()
        {
            if (_zipArchive == null)
            {
                _zipArchive = new ZipArchive(new FileStream(_zipArchivePath, FileMode.Create), ZipArchiveMode.Update);
            }
        }

        private ZipArchiveEntry GetFileEntry(string fileBackupPath)
        {
            return _zipArchive?.Entries.FirstOrDefault(x => x.FullName.Equals(fileBackupPath, StringComparison.InvariantCultureIgnoreCase));
        }

        public void Dispose()
        {
            _zipArchive?.Dispose();
        }

        public class BackupFileAlreadyExistsException : Exception
        {
            public BackupFileAlreadyExistsException() : base("The file you are attempting to back up was already found in the backup archive.") { }
        }

        public class BackupFileNotFoundException : Exception
        {
            public BackupFileNotFoundException() : base("The backup file you are attempting to get was not found in the backup archive.") { }
        }
    }
}
