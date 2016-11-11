using NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Agents;
using NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Projects;
using NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.TcSharpInterop;


namespace NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Configurations
{
    public sealed class BuildConfigurationRepositoryFactory : IBuildConfigurationRepositoryFactory
    {
        private readonly ITcSharpTeamCityClient _teamCityClient;
        private readonly ConnectedStateTicker _connectedTicker;

        public BuildConfigurationRepositoryFactory(ITcSharpTeamCityClient teamCityClient, ConnectedStateTicker connectedTicker)
        {
            _teamCityClient = teamCityClient;
            _connectedTicker = connectedTicker;
        }

        public IBuildConfigurationRepository Create(IProject project)
        {
            return new BuildConfigurationRepository(_teamCityClient, project, _connectedTicker);
        }
    }
}