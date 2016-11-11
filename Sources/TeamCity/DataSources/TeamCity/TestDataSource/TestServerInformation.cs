using System.Collections.Generic;
using TeamCitySharp.ActionTypes;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.TestDataSource
{
    public sealed class TestServerInformation : IServerInformation
    {
        public Server ServerInfo()
        {
            throw new System.NotImplementedException();
        }

        public List<Plugin> AllPlugins()
        {
            return new List<Plugin>();
        }

        public string TriggerServerInstanceBackup(BackupOptions backupOptions)
        {
            throw new System.NotImplementedException();
        }

        public string GetBackupStatus()
        {
            throw new System.NotImplementedException();
        }
    }
}