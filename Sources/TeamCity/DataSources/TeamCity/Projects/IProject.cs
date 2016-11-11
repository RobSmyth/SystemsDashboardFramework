using NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Configurations;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Projects
{
    public interface IProject
    {
        bool Archived { get; }
        string Description { get; }
        string Href { get; }
        string Id { get; }
        string Name { get; }
        string WebUrl { get; }
        Parameters Parameters { get; }
        IBuildConfiguration[] Configurations { get; }
        IBuildConfiguration GetConfiguration(string name);
        void Update(TeamCitySharp.DomainEntities.Project project);
    }
}