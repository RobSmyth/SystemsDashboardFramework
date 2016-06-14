﻿namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Agents
{
    public interface IBuildAgentRepository
    {
        IBuildAgent[] GetAll();
        void Add(IBuildAgent buildAgent);
        IBuildAgent Get(string name);
        bool Has(string name);
    }
}