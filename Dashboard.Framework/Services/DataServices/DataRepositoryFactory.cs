namespace NoeticTools.SystemsDashboard.Framework.Services.DataServices
{
    public sealed class DataRepositoryFactory : IDataRepositoryFactory
    {
        public IDataSink Create(string name, int id)
        {
            return new NamedDataRepositoy(name, id);
        }
    }
}