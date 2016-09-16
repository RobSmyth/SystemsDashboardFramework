using NoeticTools.TeamStatusBoard.Framework.Plugins;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Tiles.DaysLeftCountDown
{
    public sealed class DaysLeftCountDownTilePlugin : IPlugin
    {
        public int Rank => 0;

        public void Register(IServices services)
        {
            services.TileProviders.Register(new DaysLeftCountDownTileProvider(services.DashboardController, services));
        }
    }
}