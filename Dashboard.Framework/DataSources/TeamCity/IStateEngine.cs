namespace Dashboard.Tiles.TeamCity
{
    internal interface IStateEngine<T>
    {
        T Current { get; }
        void OnConnected();
        void OnDisconnected();
    }
}