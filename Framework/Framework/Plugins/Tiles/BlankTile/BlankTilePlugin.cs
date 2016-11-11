using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.BlankTile
{
    public class BlankTilePlugin : IPlugin
    {
        public const string TileTypeId = "Blank.Tile";

        public int Rank => 0;

        public void Register(IServices services)
        {
            services.TileProviders.Register(new BlankTileProvider(services.DashboardController, services));
        }
    }
}