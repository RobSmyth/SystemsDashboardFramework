using System;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Agents
{
    public interface IConnectedStateTicker
    {
        void AddListener(Action tickCallback);
        void RemoveListener(Action tickCallback);
    }
}