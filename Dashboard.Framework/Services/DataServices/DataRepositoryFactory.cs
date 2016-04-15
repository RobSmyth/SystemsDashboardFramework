using NoeticTools.SystemsDashboard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.Framework.Services.DataServices
{
    public sealed class DataRepositoryFactory : IDataRepositoryFactory
    {
        public IDataSource Create(string name, int id)
        {
            return new DataRepositoy(name);
        }
    }
}