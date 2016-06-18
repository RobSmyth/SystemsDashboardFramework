using System;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Agents
{
    public interface IConnectedStateTicker
    {
        void AddListener(Action tickCallback);
        void RemoveListener(Action tickCallback);
    }
}