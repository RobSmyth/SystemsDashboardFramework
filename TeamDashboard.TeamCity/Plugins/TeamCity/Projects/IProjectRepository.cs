using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Projects
{
    public interface IProjectRepository
    {
        IProject[] GetAll();
        void Add(Project project);
        IProject Get(string name);
        bool Has(string name);
    }
}