namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Channel
{
    public interface IStateEngine<T>
    {
        T Current { get; }
        void OnConnected();
        void OnDisconnected();
        void Stop();
        void Start();
    }
}