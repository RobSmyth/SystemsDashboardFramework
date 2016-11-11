using NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Agents;
using NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Channel;
using NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Projects;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity
{
    public interface ITeamCityService : IService
    {
        ITeamCityChannel Channel { get; }
        IProjectRepository Projects { get; }
        IChannelConnectionStateBroadcaster StateBroadcaster { get; }
        IConnectedStateTicker ConnectedTicker { get; }
        IBuildAgentRepository Agents { get; }
    }
}