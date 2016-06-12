using System.Threading.Tasks;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Agents;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity
{
    internal class TeamCityChannelStoppedState : ITeamCityChannelState
    {
        public string[] ProjectNames { get; }
        public bool IsConnected { get; }

        public void Connect()
        {
        }

        public void Disconnect()
        {
        }

        public Build GetLastBuild(string projectName, string buildConfigurationName)
        {
            return (Build) null;
        }

        public Build GetLastSuccessfulBuild(string projectName, string buildConfigurationName)
        {
            return (Build) null;
        }

        public Build[] GetRunningBuilds(string projectName, string buildConfigurationName, string branchName)
        {
            return new Build[0];
        }

        public Build[] GetRunningBuilds(string projectName, string buildConfigurationName)
        {
            return new Build[0];
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

        void ITeamCityChannelState.Leave()
        {
        }

        void ITeamCityChannelState.Enter()
        {
        }
    }
}