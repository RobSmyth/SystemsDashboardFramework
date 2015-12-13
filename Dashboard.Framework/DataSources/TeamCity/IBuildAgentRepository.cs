namespace NoeticTools.SystemsDashboard.Framework.DataSources.TeamCity
{
    public interface IBuildAgentRepository
    {
        IBuildAgent[] GetAll();
        void Add(IBuildAgent buildAgent);
        IBuildAgent Get(string name);
        bool Has(string name);
    }
}