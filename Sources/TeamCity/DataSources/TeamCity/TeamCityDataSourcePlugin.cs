using System;
using NoeticTools.TeamStatusBoard.Common;
using NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Agents;
using NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Channel;
using NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.TcSharpInterop;
using NoeticTools.TeamStatusBoard.Framework.Plugins;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using TeamCitySharp;


namespace NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity
{
    public sealed class TeamCityDataSourcePlugin : IPlugin
    {
        private readonly TimeSpan _agentStatusCheckTickPeriod = TimeSpan.FromMinutes(1);
        private readonly string _serviceName;

        public TeamCityDataSourcePlugin(string serviceName)
        {
            _serviceName = serviceName;
        }
            
        public int Rank => 90;

        public void Register(IServices services)
        {
            var channelStateBroadcaster = new ChannelConnectionStateBroadcaster(new EventBroadcaster(), new EventBroadcaster());
            var dataSource = new DataRepositoryFactory().Create(_serviceName);
            var configuration = new TeamCityDataSourceConfiguration(services.Configuration.Services.GetService(_serviceName));

            var tcsClientFacade = new TcsTeamCityClientFacade(new TeamCityClient(configuration.Url));
            var buildAgentFactory = new BuildAgentViewModelFactory(tcsClientFacade, services);
            var buildAgentRepository = new BuildAgentRepository(dataSource, channelStateBroadcaster, buildAgentFactory);
            var agentStatusCheckTicker = new ConnectedStateTicker(new EventBroadcaster(), services.Timer, _agentStatusCheckTickPeriod, channelStateBroadcaster);
            var agentStatusService = new BuildAgentsStatusService(buildAgentRepository, tcsClientFacade, dataSource, agentStatusCheckTicker, configuration);
            buildAgentRepository.AddListener(agentStatusService);

            new TeamCityDataSourceCommonServicesBuilder(_serviceName).Build(services, channelStateBroadcaster, tcsClientFacade, buildAgentRepository, dataSource);
        }

    }
}