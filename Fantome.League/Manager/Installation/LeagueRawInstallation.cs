using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Fantome.Libraries.League.Manager.Installation
{
    internal class LeagueRawInstallation : LeagueInstallation
    {
        private LeagueBackupArchive _backupArchive;

        public LeagueRawInstallation(string managerInstallationFolder, string folder) : base(managerInstallationFolder, folder)
        {
            string backupArchivePath = Path.Combine(managerInstallationFolder, "backup.zip");
            _backupArchive = new LeagueBackupArchive(backupArchivePath);
        }

        public override void Dispose()
        {
            _backupArchive.Dispose();
        }

        public override void InstallFile(string gamePath, string filePath)
        {
            base.InstallFile(gamePath, filePath);
            FileInfo leagueFile = new FileInfo(GetLeagueFilePath(gamePath));
            if (leagueFile.Exists)
            {
                // Check if file needs to be installed
                if (CalculateMD5(leagueFile.FullName).SequenceEqual(CalculateMD5(filePath)))
                    return;

                // Check if backup is necessary
                if (!_backupArchive.HasFile(gamePath))
                {
                    using (FileStream fs = new FileStream(leagueFile.FullName, FileMode.Open))
                    {
                        _backupArchive.AddFile(gamePath, fs);
                    }
                }
            }
            File.Copy(filePath, leagueFile.FullName, true);
        }

        public override void RevertFile(string gamePath, byte[] md5)
        {
            FileInfo leagueFile = new FileInfo(GetLeagueFilePath(gamePath));

            // File to uninstall is not installed!
            if (md5 != null && !CalculateMD5(leagueFile.FullName).SequenceEqual(md5))
                return;

            if (_backupArchive.HasFile(gamePath))
            {
                using (Stream backupStream = _backupArchive.GetBackupFileStream(gamePath))
                {
                    using (FileStream fs = new FileStream(leagueFile.FullName, FileMode.Create))
                    {
                        backupStream.CopyTo(fs);
                    }
                }
            }
            else
            {
                leagueFile.Delete();
            }
        }

        private string GetLeagueFilePath(string gamePath)
        {
            return Path.Combine(Folder, gamePath);
        }
    }
}