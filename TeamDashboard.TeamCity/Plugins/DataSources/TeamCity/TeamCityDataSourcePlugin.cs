using System;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Plugins;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Agents;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Channel;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Configurations;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Projects;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.TcSharpInterop;
using TeamCitySharp;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity
{
    public class TeamCityDataSourcePlugin : IPlugin
    {
        public int Rank => 90;

        public void Register(IServices services)
        {
            var dataSource = new DataRepositoryFactory().Create("TeamCity", "0");
            var configuration = new DataSourceConfiguration(services.Configuration.Services.GetService("TeamCity"));
            var channelStateBroadcaster = new ChannelConnectionStateBroadcaster(new EventBroadcaster(), new EventBroadcaster());
            var verySlowConnectedTicker = new ConnectedStateTicker(new EventBroadcaster(), services.Timer, TimeSpan.FromMinutes(10), channelStateBroadcaster);
            var slowConnectedTicker = new ConnectedStateTicker(new EventBroadcaster(), services.Timer, TimeSpan.FromMinutes(1), channelStateBroadcaster);
            var fastConnectedTicker = new ConnectedStateTicker(new EventBroadcaster(), services.Timer, TimeSpan.FromSeconds(30), channelStateBroadcaster);
            var teamCityClient = new TcSharpTeamCityClient(new TeamCityClient(configuration.Url));
            var buildAgentFactory = new BuildAgentViewModelFactory(services, dataSource, fastConnectedTicker, teamCityClient);
            var buildAgentRepository = new BuildAgentRepository(dataSource, teamCityClient, channelStateBroadcaster, slowConnectedTicker, buildAgentFactory);
            var buildConfigurationRepositoryFactory = new BuildConfigurationRepositoryFactory(teamCityClient, services, channelStateBroadcaster, verySlowConnectedTicker);
            var projectFactory = new ProjectFactory(buildConfigurationRepositoryFactory);
            var projectRepository = new ProjectRepository(dataSource, teamCityClient, projectFactory, slowConnectedTicker);
            var stateEngine = new ChannelStateEngine(services, teamCityClient, projectRepository, buildAgentRepository, dataSource, configuration, channelStateBroadcaster);

            var channel = new TeamCityChannel(services, dataSource, configuration, stateEngine, projectRepository, buildAgentRepository, channelStateBroadcaster);
            var teamCityDataService = new TeamCityDataService(channel, channelStateBroadcaster, fastConnectedTicker, projectRepository, buildAgentRepository);
            services.Register(teamCityDataService);

            services.DataService.Register(teamCityDataService.Name, dataSource);

            RegisterProjectsDataSource(services, projectRepository);
        }

        private void RegisterProjectsDataSource(IServices services, IProjectRepository projectRepository)
        {
            var dataSource = new DataRepositoryFactory().Create("TeamCity.Projects", "0");
            var projectRepositoryDataService = new ProjectRepositoryDataService(projectRepository, dataSource);
            services.DataService.Register(projectRepositoryDataService.Name, dataSource);
            services.Register(projectRepositoryDataService);
        }
    }
}