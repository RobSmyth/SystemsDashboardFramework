namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Projects
{
    public interface IProjectFactory
    {
        IProject Create(TeamCitySharp.DomainEntities.Project inner);
    }
}