namespace NoeticTools.Dashboard.Framework.DataSources.TeamCity
{
    internal interface IStateEngine<T>
    {
        T Current { get; }
        void OnConnected();
        void OnDisconnected();
    }
}