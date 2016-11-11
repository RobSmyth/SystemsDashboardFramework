namespace NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Channel
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