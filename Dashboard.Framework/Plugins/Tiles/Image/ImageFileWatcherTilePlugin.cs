using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.Image
{
    internal sealed class ImageFileWatcherTilePlugin : IPlugin
    {
        public int Rank => 0;

        public void Register(IServices services)
        {
            services.TileProviders.Register(new ImageFileWatcherTileProvider(services.DashboardController, services));
        }
    }
}