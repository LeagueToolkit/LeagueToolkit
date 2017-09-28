using System;
using System.IO;
using Fantome.Libraries.League.IO.ReleaseManifest;
using System.Linq;
using System.Security.Cryptography;

namespace Fantome.Libraries.League.Manager.Installation
{
    internal class LeagueRADSProjectRelease : IDisposable
    {
        public readonly LeagueRADSProject Project;
        public readonly string Version;
        public readonly uint VersionValue;
        public ReleaseManifestFile GameManifest { get; private set; }
        public ReleaseManifestFile OriginalManifest { get; private set; }
        public bool HasChanged { get; private set; }

        public LeagueRADSProjectRelease(LeagueRADSProject project, string version)
        {
            this.Project = project;
            this.Version = version;
            this.VersionValue = LeagueRADSInstallation.GetReleaseValue(version);
            string manifestPath = this.GetFolder() + "/releasemanifest";
            if (File.Exists(manifestPath))
            {
                this.GameManifest = new ReleaseManifestFile(manifestPath);
            }
            else
            {
                throw new ReleaseManifestNotFoundException();
            }
            this.LoadOriginalManifest();
        }

        private void LoadOriginalManifest()
        {
            string originalManifestFolder = String.Format("{0}/{1}/manifests/{2}",
                this.Project.Installation.ManagerInstallationFolder,
                this.Project.Name,
                this.Version);
            string manifestPath = originalManifestFolder + "/releasemanifest";
            Directory.CreateDirectory(originalManifestFolder);
            if (!File.Exists(manifestPath))
            {
                File.Copy(this.GameManifest.FilePath, manifestPath);
            }
            this.OriginalManifest = new ReleaseManifestFile(manifestPath);
        }

        public string GetFolder()
        {
            return String.Format("{0}/releases/{1}", this.Project.GetFolder(), this.Version);
        }

        public void InstallFile(string gamePath, string filePath, LeagueRADSDeployRules deployRules)
        {
            FileInfo fileToInstall = new FileInfo(filePath);

            // Getting the matching file entry (null if new file)
            ReleaseManifestFileEntry fileEntry = this.GameManifest.GetFile(gamePath, false);

            // File is already installed, don't install it again
            byte[] fileMD5 = LeagueInstallation.CalculateMD5(filePath);
            if (fileEntry != null && fileEntry.MD5.SequenceEqual(fileMD5))
                return;

            // Finding the deploy mode to use
            ReleaseManifestFile.DeployMode deployMode = deployRules.GetTargetDeployMode(this.Project.Name, fileEntry);

            // Installing file
            string installPath = Project.GetFileInstallationPath(gamePath, deployMode, LeagueRADSInstallation.FantomeFilesVersion);
            Directory.CreateDirectory(Path.GetDirectoryName(installPath));
            if (fileEntry != null && (deployMode == ReleaseManifestFile.DeployMode.Deployed4 || deployMode == ReleaseManifestFile.DeployMode.Deployed0))
            {
                // Backup deployed file
                BackupFile(fileEntry, installPath);
            }

            File.Copy(filePath, installPath, true);

            // Setting manifest values
            if (fileEntry == null)
            {
                fileEntry = this.GameManifest.GetFile(gamePath, true);
            }
            fileEntry.MD5 = fileMD5;
            fileEntry.DeployMode = deployMode;
            fileEntry.SizeRaw = (int)fileToInstall.Length;
            fileEntry.SizeCompressed = fileEntry.SizeRaw;
            fileEntry.Version = LeagueRADSInstallation.FantomeFilesVersion;
            this.HasChanged = true;
        }

        private void BackupFile(ReleaseManifestFileEntry fileEntry, string filePath)
        {
            string backupPath = this.GetBackupPath(fileEntry);
            if (!Project.BackupArchive.HasFile(backupPath))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    Project.BackupArchive.AddFile(backupPath, fs);
                }
            }
        }

        private void RestoreFile(ReleaseManifestFileEntry fileEntry, string filePath)
        {
            string backupPath = this.GetBackupPath(fileEntry);
            using (Stream backupStream = Project.BackupArchive.GetBackupFileStream(backupPath))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    backupStream.CopyTo(fs);
                }
            }
        }

        private string GetBackupPath(ReleaseManifestFileEntry fileEntry)
        {
            return Path.Combine(LeagueRADSInstallation.GetReleaseString(fileEntry.Version), fileEntry.GetFullPath());
        }

        public void RevertFile(string gamePath, byte[] md5)
        {
            ReleaseManifestFileEntry fileEntry = this.GameManifest.GetFile(gamePath, false);
            if (fileEntry == null)
                throw new NotFoundFileEntryException();

            if (md5 != null && !fileEntry.MD5.SequenceEqual(md5))
                return;

            string installedPath = Project.GetFileInstallationPath(gamePath, fileEntry.DeployMode, fileEntry.Version);
            ReleaseManifestFileEntry originalEntry = this.OriginalManifest.GetFile(gamePath, false);

            if (originalEntry == null)
            {
                // Installed file was a new file, remove it.
                if (File.Exists(installedPath))
                {
                    File.Delete(installedPath);
                }
                fileEntry.Remove();
            }
            else
            {
                // Restore original file if necessary
                if (HasToRestore(originalEntry.DeployMode, fileEntry.DeployMode))
                {
                    if (!originalEntry.MD5.SequenceEqual(fileEntry.MD5))
                    {
                        RestoreFile(originalEntry, installedPath);
                    }
                }
                else if (File.Exists(installedPath))
                {
                    File.Delete(installedPath);
                }
                // Revert original values
                fileEntry.DeployMode = originalEntry.DeployMode;
                fileEntry.LastWriteTime = originalEntry.LastWriteTime;
                fileEntry.MD5 = originalEntry.MD5;
                fileEntry.SizeCompressed = originalEntry.SizeCompressed;
                fileEntry.SizeRaw = originalEntry.SizeRaw;
                fileEntry.Version = originalEntry.Version;
            }
            HasChanged = true;
        }

        private static bool HasToRestore(ReleaseManifestFile.DeployMode originalDeployMode, ReleaseManifestFile.DeployMode installedDeployedMode)
        {
            return ((originalDeployMode == ReleaseManifestFile.DeployMode.Deployed4 || originalDeployMode == ReleaseManifestFile.DeployMode.Deployed0)
                    && (installedDeployedMode == ReleaseManifestFile.DeployMode.Deployed4 || installedDeployedMode == ReleaseManifestFile.DeployMode.Deployed0));
        }

        public void Dispose()
        {
            if (HasChanged)
            {
                GameManifest.Save();
                HasChanged = false;
            }
        }

        public class NotFoundFileEntryException : Exception
        {
            public NotFoundFileEntryException() : base("The file entry you are looking for was not found.") { }
        }

        public class ReleaseManifestNotFoundException : Exception
        {
            public ReleaseManifestNotFoundException() : base("The release manifest was not found for this release.") { }
        }
    }
}