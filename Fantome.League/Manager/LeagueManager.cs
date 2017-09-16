using Fantome.Libraries.League.Manager.Installation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Fantome.Libraries.League.Manager
{
    public class LeagueManager : IDisposable
    {
        private readonly LeagueInstallation _installation;
        private readonly string _managerFolderPath;

        public LeagueManager(string managerFolderPath, string gamePath) : this(managerFolderPath, gamePath, new LeagueRADSDeployRules(LeagueRADSFileDeployMode.Managed)) { }

        public LeagueManager(string managerFolderPath, string gamePath, LeagueRADSDeployRules deployRules)
        {
            _managerFolderPath = managerFolderPath;
            string managerInstallationFolder = GetManagerInstallationFolder(gamePath);
            if (Directory.Exists(gamePath + "/RADS"))
            {
                _installation = new LeagueRADSInstallation(managerInstallationFolder, gamePath, deployRules);
            }
            else
            {
                _installation = new LeagueRawInstallation(managerInstallationFolder, gamePath);
            }
        }

        public void InstallFile(string gamePath, string filePath)
        {
            _installation.InstallFile(gamePath, filePath);
        }
        
        public void RevertFile(string gamePath, byte[] md5 = null)
        {
            _installation.RevertFile(gamePath, md5);
        }

        public void Dispose()
        {
            _installation.Dispose();
        }

        private string GetManagerInstallationFolder(string gamePath)
        {
            List<InstallationInfo> savedInstallations = GetSavedInstallations();
            InstallationInfo foundInstallation = savedInstallations.Find(x => x.Folder.Replace("\\", "/").Equals(gamePath.Replace("\\", "/"), StringComparison.InvariantCultureIgnoreCase));
            if (foundInstallation == null)
            {
                int currentID = 1;
                while (savedInstallations.Exists(x => x.ID == currentID))
                {
                    currentID++;
                }
                foundInstallation = new InstallationInfo(gamePath, currentID);
                savedInstallations.Add(foundInstallation);
                SaveInstallations(savedInstallations);
            }
            return Path.Combine(_managerFolderPath, "lol-manager", foundInstallation.ID.ToString());
        }

        private List<InstallationInfo> GetSavedInstallations()
        {
            string installationsListPath = Path.Combine(_managerFolderPath, "lol-manager", "installations.json");
            if (File.Exists(installationsListPath))
            {
                return JsonConvert.DeserializeObject<List<InstallationInfo>>(File.ReadAllText(installationsListPath));
            }
            else
            {
                return new List<InstallationInfo>();
            }
        }

        private void SaveInstallations(List<InstallationInfo> installations)
        {
            string installationsListPath = Path.Combine(_managerFolderPath, "lol-manager", "installations.json");
            Directory.CreateDirectory(Path.GetDirectoryName(installationsListPath));
            File.WriteAllText(installationsListPath, JsonConvert.SerializeObject(installations));
        }

        private class InstallationInfo
        {
            public readonly string Folder;
            public readonly int ID;

            public InstallationInfo(string folder, int id)
            {
                Folder = folder;
                ID = id;
            }
        }
    }
}
