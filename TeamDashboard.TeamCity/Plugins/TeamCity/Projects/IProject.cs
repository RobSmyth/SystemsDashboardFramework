using System.Threading.Tasks;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Configurations;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Projects
{
    public interface IProject
    {
        bool Archived { get; }
        string Description { get; }
        string Href { get; }
        string Id { get; }
        string Name { get; }
        string WebUrl { get; }
        BuildTypeWrapper BuildTypes { get; }
        Parameters Parameters { get; }
        IBuildConfiguration[] Configurations { get; }
        IBuildConfiguration GetConfiguration(string name);
        void Update(TeamCitySharp.DomainEntities.Project project);
    }
}