using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Agents;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Channel;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Projects;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity
{
    public sealed class TeamCityDataSource : ITeamCityService
    {
        public string Name => "TeamCity";

        public TeamCityDataSource(ITeamCityChannel channel, IProjectRepository projectRepository, IChannelConnectionStateBroadcaster stateBroadcaster, IConnectedStateTicker connectedTicker)
        {
            Channel = channel;
            Projects = projectRepository;
            StateBroadcaster = stateBroadcaster;
            ConnectedTicker = connectedTicker;
        }

        public ITeamCityChannel Channel { get; }
        public IProjectRepository Projects { get; set; }
        public IChannelConnectionStateBroadcaster StateBroadcaster { get; }
        public IConnectedStateTicker ConnectedTicker { get; set; }

        public void Stop()
        {
            Channel.Stop();
        }

        public void Start()
        {
            Channel.Start();
        }
    }
}