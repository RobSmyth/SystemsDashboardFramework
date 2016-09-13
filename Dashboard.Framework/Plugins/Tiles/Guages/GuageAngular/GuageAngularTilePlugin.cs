using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.Guages.GuageAngular
{
    public sealed class GuageAngularTilePlugin : IPlugin
    {
        public int Rank => 0;

        public void Register(IServices services)
        {
            services.TileProviders.Register(new GuageAngularTileProvider(services.DashboardController, services));
        }
    }
}