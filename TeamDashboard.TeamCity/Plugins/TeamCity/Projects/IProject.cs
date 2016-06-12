using System.Threading.Tasks;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Projects
{
    public interface IProject
    {
        bool Archived { get; set; }
        string Description { get; set; }
        string Href { get; set; }
        string Id { get; set; }
        string Name { get; set; }
        string WebUrl { get; set; }
        BuildTypeWrapper BuildTypes { get; set; }
        Parameters Parameters { get; set; }
        Task<Build[]> GetRunningBuilds(string buildConfigurationName);
    }
}