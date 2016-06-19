namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Projects
{
    public interface IProjectFactory
    {
        IProject Create(TeamCitySharp.DomainEntities.Project inner);
    }
}