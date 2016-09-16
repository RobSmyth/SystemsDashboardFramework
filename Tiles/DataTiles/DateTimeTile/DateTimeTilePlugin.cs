using NoeticTools.TeamStatusBoard.Framework.Plugins;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Tiles.DataTiles.DateTimeTile
{
    public sealed class DateTimeDataTilePlugin : IPlugin
    {
        public int Rank => 0;

        public void Register(IServices services)
        {
            services.TileProviders.Register(new DateTimeDataTileProvider(services.DashboardController, services));
        }
    }
}