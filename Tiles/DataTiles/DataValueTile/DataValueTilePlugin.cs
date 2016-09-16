using NoeticTools.TeamStatusBoard.Framework.Plugins;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Tiles.DataTiles.DataValueTile
{
    public sealed class DataValueTilePlugin : IPlugin
    {
        public int Rank => 0;

        public void Register(IServices services)
        {
            services.TileProviders.Register(new DataValueTileProvider(services.DashboardController, services));
        }
    }
}