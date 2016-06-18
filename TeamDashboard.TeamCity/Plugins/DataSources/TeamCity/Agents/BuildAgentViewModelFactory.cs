using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Channel;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.TcSharpInterop;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Agents
{
    public sealed class BuildAgentViewModelFactory : IBuildAgentViewModelFactory
    {
        private readonly IServices _services;
        private readonly IDataSource _outerRepository;
        private readonly IConnectedStateTicker _connectedStateTicker;
        private readonly ITcSharpTeamCityClient _teamCityClient;

        public BuildAgentViewModelFactory(IServices services, IDataSource outerRepository, IConnectedStateTicker connectedStateTicker, ITcSharpTeamCityClient teamCityClient)
        {
            _services = services;
            _outerRepository = outerRepository;
            _connectedStateTicker = connectedStateTicker;
            _teamCityClient = teamCityClient;
        }

        public IBuildAgent Create(string name, IChannelConnectionStateBroadcaster channelConnectionStateBroadcaster)
        {
            return new BuildAgentViewModel(name, _outerRepository, channelConnectionStateBroadcaster, _teamCityClient, _connectedStateTicker);
        }
    }
}