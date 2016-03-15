using NoeticTools.SystemsDashboard.Framework.Dashboards;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.MessageTile
{
    internal sealed class MessageTilePlugin : IPlugin
    {
        private readonly IDashboardController _dashboardController;
        private IServices _services;

        public MessageTilePlugin(IDashboardController dashboardController)
        {
            _dashboardController = dashboardController;
        }

        public int Rank => 0;

        public void Register(IServices services)
        {
            _services = services;
            services.TileProviders.Register(new MessageTileProvider(_dashboardController, _services));
        }
    }
}