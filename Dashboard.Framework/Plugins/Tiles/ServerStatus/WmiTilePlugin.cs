

namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.ServerStatus
{
    internal sealed class WmiTilePlugin : IPlugin
    {
        public int Rank => 0;

        public void Register(IServices services)
        {
            services.TileProviders.Register(new WmiTileProvider(services.DashboardController, services));
        }
    }
}