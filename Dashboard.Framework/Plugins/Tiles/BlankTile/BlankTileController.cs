using System.Windows;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.Date;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.BlankTile
{
    internal class BlankTileController
    {
        private readonly IDashboardController _dashboardController;
        private readonly TileLayoutController _layoutController;
        private readonly IServices _services;
        private BlankTileControl _view;
        private readonly TileConfiguration _tile;

        public BlankTileController(TileConfiguration tile, IDashboardController dashboardController, TileLayoutController layoutController, IServices services)
        {
            _tile = tile;
            _dashboardController = dashboardController;
            _layoutController = layoutController;
            _services = services;
        }

        public FrameworkElement CreateView()
        {
            _view = new BlankTileControl {DataContext = new BlankTileViewModel(_tile, _dashboardController, _layoutController, _services)};
            UpdateView();
            return _view;
        }

        private void UpdateView()
        {
        }
    }
}