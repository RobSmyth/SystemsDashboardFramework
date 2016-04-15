using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.SystemsDashboard.Framework.Services.DataServices
{
    public interface IDataRepositoryFactory
    {
        IDataSource Create(string name, int id);
    }
}