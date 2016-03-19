using NoeticTools.SystemsDashboard.Framework.Plugins.DataSources.DashboardData;


namespace NoeticTools.SystemsDashboard.Framework.Services.DataServices
{
    public interface IDataService : IService, IDataSource
    {
        IDataSource GetDataSource(string name);
        void Register(string name, IDataSource dataSource);
    }
}