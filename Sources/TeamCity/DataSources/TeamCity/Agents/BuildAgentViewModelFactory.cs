using System;
using NoeticTools.TeamStatusBoard.Common;
using NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Channel;
using NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.TcSharpInterop;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Agents
{
    public sealed class BuildAgentViewModelFactory : IBuildAgentViewModelFactory
    {
        private readonly ITcSharpTeamCityClient _teamCityClient;
        private readonly IServices _services;
        private readonly TimeSpan _tickPeriod = TimeSpan.FromSeconds(30);

        public BuildAgentViewModelFactory(ITcSharpTeamCityClient teamCityClient, IServices services)
        {
            _teamCityClient = teamCityClient;
            _services = services;
        }

        public IBuildAgent Create(string name, IChannelConnectionStateBroadcaster channelConnectionStateBroadcaster, IDataSource outerRepository)
        {
            var ticker = new ConnectedStateTicker(new EventBroadcaster(), _services.Timer, _tickPeriod, channelConnectionStateBroadcaster);
            return new BuildAgentViewModel(name, outerRepository, channelConnectionStateBroadcaster, _teamCityClient, ticker);
        }
    }
}