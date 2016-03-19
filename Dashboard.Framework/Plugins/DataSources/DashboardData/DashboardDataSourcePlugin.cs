using NoeticTools.SystemsDashboard.Framework.Services;
using NoeticTools.SystemsDashboard.Framework.Services.DataServices;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.DataSources.DashboardData
{
    public class DashboardDataSourcePlugin : IPlugin
    {
        public int Rank => 100;

        public void Register(IServices services)
        {
            services.DataService.Register("Dashboard", new DashboardDataSource(services.Configuration.Services, services, new DataRepositoy("DashboardInner")));
        }
    }
}
