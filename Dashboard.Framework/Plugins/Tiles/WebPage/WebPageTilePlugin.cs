using System;
using System.Windows;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Dashboards;
using NoeticTools.SystemsDashboard.Framework.Tiles.WebPage;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.WebPage
{
    public class WebPageTilePlugin : IPlugin, ITileControllerProvider
    {
        private readonly IDashboardController _dashboardController;
        private IServices _services;

        public WebPageTilePlugin(IDashboardController dashboardController)
        {
            _dashboardController = dashboardController;
        }

        public int Rank => 0;

        public string Name => "Web page";

        public bool MatchesId(string id)
        {
            return id == WebPageTileViewModel.TileTypeId || id.Equals("92CE0D61-4748-4427-8EB7-DC8B8B741C15", StringComparison.InvariantCultureIgnoreCase);
        }

        public FrameworkElement CreateTile(TileConfiguration tile, TileLayoutController layoutController)
        {
            var view = new WebPageTileControl {DataContext = this};
            new WebPageTileViewModel(tile, _dashboardController, layoutController, _services, view);
            return view;
        }

        public TileConfiguration CreateDefaultConfiguration()
        {
            return new TileConfiguration
            {
                TypeId = WebPageTileViewModel.TileTypeId,
                Id = Guid.NewGuid(),
                Tiles = new TileConfiguration[0]
            };
        }

        public void Register(IServices services)
        {
            _services = services;
            services.TileProviders.Register(this);
        }
    }
}