namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Projects
{
    public interface IProjectRepository
    {
        IProject Get(string name);
        IProject[] GetAll();
    }
}