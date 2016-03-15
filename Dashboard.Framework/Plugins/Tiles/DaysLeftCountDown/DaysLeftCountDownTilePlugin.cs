using NoeticTools.SystemsDashboard.Framework.Dashboards;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.DaysLeftCountDown
{
    public sealed class DaysLeftCountDownTilePlugin : IPlugin
    {
        private readonly IDashboardController _dashboardController;

        public DaysLeftCountDownTilePlugin(IDashboardController dashboardController)
        {
            _dashboardController = dashboardController;
        }

        public int Rank => 0;

        public void Register(IServices services)
        {
            services.TileProviders.Register(new DaysLeftCountDownTileProvider(_dashboardController, services));
        }
    }
}