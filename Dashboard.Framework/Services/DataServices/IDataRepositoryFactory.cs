namespace NoeticTools.SystemsDashboard.Framework.Services.DataServices
{
    public interface IDataRepositoryFactory
    {
        IDataSink Create(string name, int id);
    }
}