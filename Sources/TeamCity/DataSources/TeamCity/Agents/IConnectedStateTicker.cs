using System;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Agents
{
    public interface IConnectedStateTicker
    {
        void AddListener(object listener, Action tickCallback);
        void RemoveListener(object listener, Action tickCallback);
    }
}