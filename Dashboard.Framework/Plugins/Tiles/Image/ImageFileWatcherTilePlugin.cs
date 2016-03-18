using NoeticTools.SystemsDashboard.Framework.Dashboards;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.Image
{
    internal sealed class ImageFileWatcherTilePlugin : IPlugin
    {
        private readonly IDashboardController _dashboardController;

        public ImageFileWatcherTilePlugin(IDashboardController dashboardController)
        {
            _dashboardController = dashboardController;
        }

        public int Rank => 0;

        public void Register(IServices services)
        {
            services.TileProviders.Register(new ImageFileWatcherTileProvider(_dashboardController, services));
        }
    }
}