using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Services.DataServices;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.DataSources.DashboardData
{
    public class DashboardDataSourcePlugin : IPlugin
    {
        private readonly IDashboardConfigurationServices _configurationServices;

        public DashboardDataSourcePlugin(IDashboardConfigurationServices configurationServices)
        {
            _configurationServices = configurationServices;
        }

        public int Rank => 100;

        public void Register(IServices services)
        {
            services.DataService.Register("Dashboard", new DashboardDataSource(_configurationServices, services, new DataRepositoy("DashboardInner")));
        }
    }
}
