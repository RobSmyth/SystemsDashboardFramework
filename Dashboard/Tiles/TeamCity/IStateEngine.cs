namespace Dashboard.Tiles.TeamCity
{
    interface IStateEngine<T>
    {
        T Current { get; }
        void OnConnected();
        void OnDisconnected();
    }
}
