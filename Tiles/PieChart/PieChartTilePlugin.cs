using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.PieChart
{
    public sealed class PieChartTilePlugin : IPlugin
    {
        public int Rank => 0;

        public void Register(IServices services)
        {
            services.TileProviders.Register(new PieChartTileProvider(services.DashboardController, services));
        }
    }
}