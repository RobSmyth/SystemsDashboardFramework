namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Channel
{
    public interface IChannelConnectionStateListener
    {
        void OnConnected();
        void OnDisconnected();
    }
}