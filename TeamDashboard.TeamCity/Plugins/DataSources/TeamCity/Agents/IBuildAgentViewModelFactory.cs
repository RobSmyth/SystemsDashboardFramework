using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Channel;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Agents
{
    public interface IBuildAgentViewModelFactory
    {
        IBuildAgent Create(string name, IChannelConnectionStateBroadcaster channelConnectionStateBroadcaster);
    }
}