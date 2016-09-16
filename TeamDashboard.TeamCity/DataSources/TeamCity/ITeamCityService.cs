using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Agents;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Channel;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Projects;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity
{
    public interface ITeamCityService : IService
    {
        ITeamCityChannel Channel { get; }
        IProjectRepository Projects { get; }
        IChannelConnectionStateBroadcaster StateBroadcaster { get; }
        IConnectedStateTicker ConnectedTicker { get; }
        BuildAgentRepository Agents { get; }
    }
}