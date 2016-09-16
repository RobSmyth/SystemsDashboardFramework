using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Projects
{
    public interface IProjectRepository
    {
        IProject Get(string name);
        IProject[] GetAll();
        void AddListener(IDataChangeListener listener);
    }
}