﻿using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Agents;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Plugins;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Channel;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Projects;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.TcSharpInterop;
using TeamCitySharp;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity
{
    public class TeamCityServicePlugin : IPlugin
    {
        public int Rank => 90;

        public void Register(IServices services)
        {
            var dataSource = new DataRepositoryFactory().Create("TeamCity", "0");
            var configuration = new TeamCityServiceConfiguration(services.Configuration.Services.GetService("TeamCity"));
            var channelStateBroadcaster = new ChannelConnectionStateBroadcaster(new EventBroadcaster(), new EventBroadcaster());
            var teamCityClient = new TcSharpTeamCityClient(new TeamCityClient(configuration.Url));
            var buildAgentRepository = new BuildAgentRepository(dataSource, teamCityClient, services, channelStateBroadcaster);
            var projectRepository = new ProjectRepository(dataSource, teamCityClient, services, channelStateBroadcaster);
            var stateEngine = new ChannelStateEngine(services, teamCityClient, projectRepository, buildAgentRepository, dataSource, configuration, channelStateBroadcaster);

            var channel = new TeamCityChannel(services, dataSource, configuration, stateEngine, projectRepository);
            services.Register(new TeamCityService(channel));
            services.DataService.Register("TeamCity", dataSource, new NullTileControllerProvider());
        }
    }
}