using System.Windows;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.Date;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.BlankTile
{
    internal class BlankTileController : IViewController
    {
        public const string TileTypeId = "Blank.Tile";
        private readonly IDashboardController _dashboardController;
        private readonly TileLayoutController _layoutController;
        private readonly IServices _services;
        private BlankTileControl _view;

        public BlankTileController(TileConfiguration tile, IDashboardController dashboardController, TileLayoutController layoutController, IServices services)
        {
            Tile = tile;
            _dashboardController = dashboardController;
            _layoutController = layoutController;
            _services = services;
        }

        public TileConfiguration Tile { get; }

        public FrameworkElement CreateView()
        {
            _view = new BlankTileControl {DataContext = new BlankTileViewModel(Tile, _dashboardController, _layoutController, _services)};
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