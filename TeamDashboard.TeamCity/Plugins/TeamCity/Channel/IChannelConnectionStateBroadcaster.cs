using NoeticTools.TeamStatusBoard.Framework;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Channel
{
    public interface IChannelConnectionStateBroadcaster
    {
        EventBroadcaster OnConnected { get; }
        EventBroadcaster OnDisconnected { get; }
        void Add(IChannelConnectionStateListener listener);
        void Remove(IChannelConnectionStateListener listener);
    }
}