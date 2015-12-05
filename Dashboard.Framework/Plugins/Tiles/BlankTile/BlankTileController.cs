using System;
using System.Windows;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Config.Commands;
using NoeticTools.Dashboard.Framework.Plugins.Tiles.Date;
using NoeticTools.Dashboard.Framework.Tiles;
using NoeticTools.Dashboard.Framework.Time;


namespace NoeticTools.Dashboard.Framework.Plugins.Tiles.BlankTile
{
    internal class BlankTileController : IViewController
    {
        private readonly TileConfiguration _tile;
        private readonly IDashboardController _dashboardController;
        private readonly TileLayoutController _layoutController;
        private readonly IServices _services;
        public const string TileTypeId = "Blank.Tile";
        private BlankTileControl _view;

        public BlankTileController(TileConfiguration tile, IDashboardController dashboardController, TileLayoutController layoutController, IServices services)
        {
            _tile = tile;
            _dashboardController = dashboardController;
            _layoutController = layoutController;
            _services = services;
        }

        public FrameworkElement CreateView()
        {
            _view = new BlankTileControl() {DataContext = new BlankTileViewModel(_tile, _dashboardController, _layoutController, _services)};
            UpdateView();
            return _view;
        }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
        }

        private void UpdateView()
        {
        }
    }
}