namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity
{
    public interface IChannelConnectionStateListener
    {
        void OnConnected();
        void OnDisconnected();
    }
}