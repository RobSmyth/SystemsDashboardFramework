using System;
using NoeticTools.TeamStatusBoard.Common;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Channel;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.TcSharpInterop;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Agents
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