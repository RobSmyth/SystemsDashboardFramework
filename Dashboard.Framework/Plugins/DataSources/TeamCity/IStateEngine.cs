namespace NoeticTools.TeamStatusBoard.Framework.Plugins.DataSources.TeamCity
{
    internal interface IStateEngine<T>
    {
        T Current { get; }
        void OnConnected();
        void OnDisconnected();
    }
}