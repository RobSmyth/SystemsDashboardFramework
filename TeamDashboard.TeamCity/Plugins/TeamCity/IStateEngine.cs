namespace NoeticTool.sTeamStatusBoard.TeamCity.Plugins.TeamCity
{
    internal interface IStateEngine<T>
    {
        T Current { get; }
        void OnConnected();
        void OnDisconnected();
    }
}