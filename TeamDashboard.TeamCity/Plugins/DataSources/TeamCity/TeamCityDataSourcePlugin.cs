using System;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Plugins;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Agents;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Channel;
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
            var slowConnectedTicker = new ConnectedStateTicker(new EventBroadcaster(), services.Timer, TimeSpan.FromMinutes(1), channelStateBroadcaster);
            var fastConnectedTicker = new ConnectedStateTicker(new EventBroadcaster(), services.Timer, TimeSpan.FromMinutes(1), channelStateBroadcaster);
            var teamCityClient = new TcSharpTeamCityClient(new TeamCityClient(configuration.Url));
            var buildAgentFactory = new BuildAgentViewModelFactory(services, dataSource, fastConnectedTicker, teamCityClient);
            var buildAgentRepository = new BuildAgentRepository(dataSource, teamCityClient, services, channelStateBroadcaster, slowConnectedTicker, buildAgentFactory);
            var projectRepository = new ProjectRepository(dataSource, teamCityClient, services, channelStateBroadcaster);
            var stateEngine = new ChannelStateEngine(services, teamCityClient, projectRepository, buildAgentRepository, dataSource, configuration, channelStateBroadcaster);

            var channel = new TeamCityChannel(services, dataSource, configuration, stateEngine, projectRepository, buildAgentRepository, channelStateBroadcaster);
            services.Register(new TeamCityDataSource(channel, projectRepository));

            services.DataService.Register("TeamCity", dataSource);
        }
    }
}