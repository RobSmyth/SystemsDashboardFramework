using System;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Tiles;
using NoeticTools.Dashboard.Framework.Tiles.WebPage;


namespace NoeticTools.Dashboard.Framework.Plugins.Tiles.WebPage
{
    public class WebPageTilePlugin : IPlugin, ITileControllerProvider
    {
        private readonly IDashboardController _dashboardController;

        public WebPageTilePlugin(IDashboardController dashboardController)
        {
            _dashboardController = dashboardController;
        }

        public string Name => "Web page";

        public bool MatchesId(string id)
        {
            return id == WebPageTileController.TileTypeId || id.Equals("92CE0D61-4748-4427-8EB7-DC8B8B741C15", StringComparison.InvariantCultureIgnoreCase);
        }

        public IViewController CreateTileController(TileConfiguration tileConfiguration)
        {
            return new WebPageTileController(tileConfiguration, _dashboardController);
        }

        public TileConfiguration CreateDefaultConfiguration()
        {
            return new TileConfiguration
            {
                TypeId = WebPageTileController.TileTypeId,
                Id = Guid.NewGuid(),
                Tiles = new TileConfiguration[0]
            };
        }

        public void Register(IServices services)
        {
            services.TileProviderRegistry.Register(this);
        }
    }
}