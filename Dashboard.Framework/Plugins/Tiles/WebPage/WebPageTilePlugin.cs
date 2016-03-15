using NoeticTools.SystemsDashboard.Framework.Dashboards;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.WebPage
{
    public sealed class WebPageTilePlugin : IPlugin
    {
        private readonly IDashboardController _dashboardController;

        public WebPageTilePlugin(IDashboardController dashboardController)
        {
            _dashboardController = dashboardController;
        }

        public int Rank => 0;

        public void Register(IServices services)
        {
            services.TileProviders.Register(new WebPageTileProvider(_dashboardController, services));
        }
    }
}