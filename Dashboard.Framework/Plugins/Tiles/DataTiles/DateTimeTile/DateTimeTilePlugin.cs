using NoeticTools.SystemsDashboard.Framework.Services;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.DataTiles.DateTimeTile
{
    internal sealed class DateTimeDataTilePlugin : IPlugin
    {
        public int Rank => 0;

        public void Register(IServices services)
        {
            services.TileProviders.Register(new DateTimeDataTileProvider(services.DashboardController, services));
        }
    }
}