using NoeticTools.SystemsDashboard.Framework.Services;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.DataTiles.TextTile
{
    internal sealed class TextDataTilePlugin : IPlugin
    {
        public int Rank => 0;

        public void Register(IServices services)
        {
            services.TileProviders.Register(new TextDataTileProvider(services.DashboardController, services));
        }
    }
}