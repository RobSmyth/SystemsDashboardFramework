using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Projects
{
    public interface IProjectRepository
    {
        IProject Add(string name);
        IProject Get(string name);
        IProject[] GetAll();
        void AddListener(IDataChangeListener listener);
        void Add(IProject project);
    }
}