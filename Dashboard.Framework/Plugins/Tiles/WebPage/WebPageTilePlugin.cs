using NoeticTools.SystemsDashboard.Framework.Plugins;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.WebPage
{
    public sealed class WebPageTilePlugin : IPlugin
    {
        public int Rank => 0;

        public void Register(IServices services)
        {
            services.TileProviders.Register(new WebPageTileProvider(services.DashboardController, services));
        }
    }
}