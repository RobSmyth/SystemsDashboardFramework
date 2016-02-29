namespace NoeticTools.SystemsDashboard.Framework.Services.DataServices
{
    public interface IDataService : IService
    {
        IDataSource GetDataSource(string name);
        IDataSink CreateDataSink(string name);
    }
}