using System;
using NoeticTools.TeamStatusBoard.Common;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Agents;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Channel;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Configurations;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Projects;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.TcSharpInterop;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity
{
    public sealed class TeamCityDataSourceCommonServicesBuilder
    {
        private readonly TimeSpan _verySlowTickPeriod = TimeSpan.FromMinutes(10);
        private readonly TimeSpan _slowTickPeriod = TimeSpan.FromMinutes(1);
        private readonly TimeSpan _fastTickPeriod = TimeSpan.FromSeconds(30);
        private readonly string _serviceName;

        public TeamCityDataSourceCommonServicesBuilder(string serviceName)
        {
            _serviceName = serviceName;
        }

        public void Build(IServices services, ChannelConnectionStateBroadcaster channelStateBroadcaster, ITcSharpTeamCityClient tcsClientFacade, IBuildAgentRepository buildAgentRepository, IDataSource dataSource)
        {
            var configuration = new TeamCityDataSourceConfiguration(services.Configuration.Services.GetService(_serviceName));

            var verySlowConnectedTicker = new ConnectedStateTicker(new EventBroadcaster(), services.Timer, _verySlowTickPeriod, channelStateBroadcaster);
            var slowConnectedTicker = new ConnectedStateTicker(new EventBroadcaster(), services.Timer, _slowTickPeriod, channelStateBroadcaster);
            var fastConnectedTicker = new ConnectedStateTicker(new EventBroadcaster(), services.Timer, _fastTickPeriod, channelStateBroadcaster);

            var buildConfigurationRepositoryFactory = new BuildConfigurationRepositoryFactory(tcsClientFacade, verySlowConnectedTicker);
            var projectFactory = new ProjectFactory(buildConfigurationRepositoryFactory);
            var projectRepository = new ProjectRepository(tcsClientFacade, projectFactory, slowConnectedTicker);
            var stateEngine = new ChannelStateEngine(services, tcsClientFacade, projectRepository, buildAgentRepository, dataSource, configuration, channelStateBroadcaster);

            var conduit = new ConfigurationChangeListenerConduit();
            var tileProperties = new TileProperties(configuration, conduit, services);
            var channel = new TeamCityChannel(services, dataSource, configuration, stateEngine, buildAgentRepository, channelStateBroadcaster, tileProperties);
            conduit.SetTarget(channel);

            var teamCityDataService = new TeamCityDataService(_serviceName, channel, channelStateBroadcaster, fastConnectedTicker, projectRepository, buildAgentRepository);
            services.Register(teamCityDataService);

            services.DataService.Register(teamCityDataService.Name, dataSource);

            services.Register(new ProjectRepositoryDataService(dataSource, projectRepository, fastConnectedTicker));
        }
    }
}