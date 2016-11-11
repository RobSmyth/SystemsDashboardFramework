namespace NoeticTools.TeamStatusBoard.Framework.Services.DataServices
{
    public sealed class DataRepositoryFactory : IDataRepositoryFactory
    {
        public IDataSource Create(string typeName)
        {
            return new DataRepositoy(typeName);
        }
    }
}