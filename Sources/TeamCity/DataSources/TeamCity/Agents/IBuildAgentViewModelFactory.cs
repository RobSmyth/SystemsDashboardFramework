using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Channel;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Agents
{
    public interface IBuildAgentViewModelFactory
    {
        IBuildAgent Create(string name, IChannelConnectionStateBroadcaster channelConnectionStateBroadcaster, IDataSource outerRepository);
    }
}