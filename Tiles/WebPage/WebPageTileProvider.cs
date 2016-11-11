using System;
using System.Windows;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Persistence.Xml;


namespace NoeticTools.TeamStatusBoard.Tiles.WebPage
{
    public class WebPageTileProvider : ITileControllerProvider
    {
        private readonly IDashboardController _dashboardController;
        private readonly IServices _services;

        public WebPageTileProvider(IDashboardController dashboardController, IServices services)
        {
            _dashboardController = dashboardController;
            _services = services;
        }

        public string Name => "Web page";

        public string TypeId => WebPageTileViewModel.TileTypeId;

        public bool MatchesId(string id)
        {
            return id == WebPageTileViewModel.TileTypeId || id.Equals("92CE0D61-4748-4427-8EB7-DC8B8B741C15", StringComparison.InvariantCultureIgnoreCase);
        }

        public FrameworkElement CreateTile(TileConfiguration tileConfigturation, TileLayoutController layoutController)
        {
            var view = new WebPageTileControl {DataContext = this};
            new WebPageTileViewModel(tileConfigturation, _dashboardController, layoutController, _services, view);
            return view;
        }

        public TileConfiguration CreateDefaultConfiguration()
        {
            return new TileConfiguration
            {
                TypeId = WebPageTileViewModel.TileTypeId,
            };
        }
    }
}