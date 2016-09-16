using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Agents;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Channel;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Projects;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity
{
    public sealed class TeamCityDataService : ITeamCityService
    {
        public string Name => "TeamCity";

        public TeamCityDataService(ITeamCityChannel channel, IChannelConnectionStateBroadcaster stateBroadcaster, 
            IConnectedStateTicker connectedTicker, IProjectRepository projectRepository, BuildAgentRepository buildAgentRepository)
        {
            Channel = channel;
            Projects = projectRepository;
            Agents = buildAgentRepository;
            StateBroadcaster = stateBroadcaster;
            ConnectedTicker = connectedTicker;
        }

        public ITeamCityChannel Channel { get; }
        public IProjectRepository Projects { get; }
        public BuildAgentRepository Agents { get; }
        public IChannelConnectionStateBroadcaster StateBroadcaster { get; }
        public IConnectedStateTicker ConnectedTicker { get; }

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