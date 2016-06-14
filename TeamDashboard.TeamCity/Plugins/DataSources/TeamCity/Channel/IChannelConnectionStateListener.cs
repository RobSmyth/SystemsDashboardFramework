namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Channel
{
    public interface IChannelConnectionStateListener
    {
        void OnConnected();
        void OnDisconnected();
    }
}