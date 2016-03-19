namespace NoeticTools.SystemsDashboard.Framework.Services.DataServices
{
    public sealed class PropertiesRepositoryFactory : IDataRepositoryFactory
    {
        public IDataSource Create(string name, int id)
        {
            return new DataRepositoy(name);
        }
    }
}