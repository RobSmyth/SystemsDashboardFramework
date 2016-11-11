using NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Channel;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Agents
{
    public interface IBuildAgentViewModelFactory
    {
        IBuildAgent Create(string name, IChannelConnectionStateBroadcaster channelConnectionStateBroadcaster, IDataSource outerRepository);
    }
}