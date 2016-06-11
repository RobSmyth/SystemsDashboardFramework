namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity
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