using NoeticTools.TeamStatusBoard.Framework.Plugins;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Tiles.ColouredTile
{
    public class ColouredTilePlugin : IPlugin
    {
        public const string TileTypeId = "Coloured.Tile";

        public int Rank => 0;

        public void Register(IServices services)
        {
            services.TileProviders.Register(new ColouredTileProvider(services.DashboardController, services));
        }
    }
}