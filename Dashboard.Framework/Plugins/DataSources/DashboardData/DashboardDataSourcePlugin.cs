using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.DataSources.DashboardData
{
    public class DashboardDataSourcePlugin : IPlugin
    {
        public int Rank => 100;

        public void Register(IServices services)
        {
            services.DataService.Register("Dashboard", new DashboardDataSource(services.Configuration.Services, services, 
                new DataRepositoy("DashboardInner", "")), new NullTileControllerProvider());
        }
    }
}