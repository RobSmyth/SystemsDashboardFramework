using System.Threading.Tasks;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Projects
{
    public interface IProjectRepository
    {
        Build[] GetRunningBuilds(string projectName, string buildConfigurationName);
        IProject Get(string name);
        IProject[] GetAll();
    }
}