using System.Threading.Tasks;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Projects
{
    public interface IProjectRepository
    {
        IProject Get(string name);
        IProject[] GetAll();
    }
}