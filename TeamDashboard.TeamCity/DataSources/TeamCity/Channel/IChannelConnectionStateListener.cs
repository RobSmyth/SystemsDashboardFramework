namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Channel
{
    public interface IChannelConnectionStateListener
    {
        void OnConnected();
        void OnDisconnected();
    }
}