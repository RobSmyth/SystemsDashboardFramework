using System;


namespace NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Agents
{
    public interface IConnectedStateTicker
    {
        void AddListener(object listener, Action tickCallback);
        void RemoveListener(object listener, Action tickCallback);
    }
}