using System.Threading.Tasks;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.DataSources.TeamCity
{
    internal class TeamCityChannelStoppedState : ITeamCityChannel
    {
        public string[] ProjectNames { get; }
        public bool IsConnected { get; }

        public void Connect()
        {
        }

        public void Disconnect()
        {
        }

        public Task<Build> GetLastBuild(string projectName, string buildConfigurationName)
        {
            return Task.Run(() => (Build) null);
        }

        public Task<Build> GetLastSuccessfulBuild(string projectName, string buildConfigurationName)
        {
            return Task.Run(() => (Build) null);
        }

        public Task<Build[]> GetRunningBuilds(string projectName, string buildConfigurationName, string branchName)
        {
            return Task.Run(() => new Build[0]);
        }

        public Task<Build[]> GetRunningBuilds(string projectName, string buildConfigurationName)
        {
            return Task.Run(() => new Build[0]);
        }

        public Task<string[]> GetConfigurationNames(string projectName)
        {
            return Task.Run(() => new string[0]);
        }

        public Task<IBuildAgent[]> GetAgents()
        {
            return Task.Run(() => new IBuildAgent[0]);
        }

        public Task<IBuildAgent> GetAgent(string name)
        {
            return Task.Run(() => (IBuildAgent) null);
        }
    }
}