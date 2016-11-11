namespace NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Channel
{
    public interface IChannelConnectionStateListener
    {
        void OnConnected();
        void OnDisconnected();
    }
}