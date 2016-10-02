using System.Threading.Tasks;
using log4net;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Agents;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Channel
{
    internal class ChannelEmulatedState : ITeamCityChannelState
    {
        private readonly IDataSource _dataSource;
        private readonly string[] _status = {"SUCCESS", "SUCCESS", "SUCCESS", "SUCCESS", "FAILURE", "UNKNOWN"};
        private ILog _logger;

        public ChannelEmulatedState(IDataSource dataSource)
        {
            _dataSource = dataSource;
            _logger = LogManager.GetLogger("DateSources.TeamCity.Emulated");

            foreach (var projectName in ProjectNames)
            {
                dataSource.Write($"{projectName}.Status", "Emulated");
            }
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