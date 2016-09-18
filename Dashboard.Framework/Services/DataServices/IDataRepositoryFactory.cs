namespace NoeticTools.TeamStatusBoard.Framework.Services.DataServices
{
    public interface IDataRepositoryFactory
    {
        IDataSource Create(string typeName);
    }
}