using System.Threading.Tasks;
using log4net;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Agents;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Channel
{
    internal class ChannelEmulatedState : ITeamCityChannelState
    {
        private readonly string[] _status = {"SUCCESS", "SUCCESS", "SUCCESS", "SUCCESS", "FAILURE", "UNKNOWN"};
        private ILog _logger;

        public ChannelEmulatedState(IDataSource repository)
        {
            _logger = LogManager.GetLogger("DateSources.TeamCity.Emulated");

            repository.Write("Projects.Count", 0);
            foreach (var projectName in ProjectNames)
            {
                repository.Write($"Project.{projectName}.Status", "Emulated");
            }
            repository.Write("Projects.Count", ProjectNames.Length);
        }

        public string[] ProjectNames => new[] {"Project A", "Project B"};
        public bool IsConnected => true;

        public void Connect()
        {
        }

        public void Disconnect()
        {
        }

        public string[] GetConfigurationNames(string projectName)
        {
            return new[] {"Configuration 1", "Configuration 2", "Configuration 3"};
        }

        public IBuildAgent[] GetAgents()
        {
            return new IBuildAgent[0];
        }

        public Task<IBuildAgent> GetAgent(string name)
        {
            return Task.Run<IBuildAgent>(() => new NullBuildAgent(name));
        }

        void ITeamCityChannelState.Leave()
        {
        }

        void ITeamCityChannelState.Enter()
        {
        }
    }
}