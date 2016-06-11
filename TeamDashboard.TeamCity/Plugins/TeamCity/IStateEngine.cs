namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity
{
    internal interface IStateEngine<T>
    {
        T Current { get; }
        void OnConnected();
        void OnDisconnected();
    }
}