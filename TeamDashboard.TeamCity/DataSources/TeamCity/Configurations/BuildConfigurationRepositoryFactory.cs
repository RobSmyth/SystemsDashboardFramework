using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Agents;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Channel;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Projects;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.TcSharpInterop;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Configurations
{
    public sealed class BuildConfigurationRepositoryFactory : IBuildConfigurationRepositoryFactory
    {
        private readonly ITcSharpTeamCityClient _teamCityClient;
        private readonly IServices _services;
        private readonly IChannelConnectionStateBroadcaster _stateBroadcaster;
        private readonly ConnectedStateTicker _connectedTicker;

        public BuildConfigurationRepositoryFactory(ITcSharpTeamCityClient teamCityClient, IServices services, IChannelConnectionStateBroadcaster stateBroadcaster, ConnectedStateTicker connectedTicker)
        {
            _teamCityClient = teamCityClient;
            _services = services;
            _stateBroadcaster = stateBroadcaster;
            _connectedTicker = connectedTicker;
        }

        public IBuildConfigurationRepository Create(IProject project)
        {
            return new BuildConfigurationRepository(_teamCityClient, project, _connectedTicker);
        }
    }
}