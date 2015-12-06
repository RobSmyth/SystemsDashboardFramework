using System.Windows;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.MessageTile;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.Image
{
    internal sealed class ImageTileController : IViewController
    {
        private readonly TileConfiguration _tile;
        private readonly IDashboardController _dashboardController;
        public static readonly string TileTypeId = "Image";
        private readonly TileLayoutController _layoutController;
        private readonly IServices _services;

        public ImageTileController(TileConfiguration tile, IDashboardController dashboardController, TileLayoutController layoutController, IServices services)
        {
            _tile = tile;
            _dashboardController = dashboardController;
            _layoutController = layoutController;
            _services = services;
        }

        public FrameworkElement CreateView()
        {
            return new ImageTileControl {DataContext = new ImageViewModel(_tile, _dashboardController, _layoutController, _services)};
        }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
        }
    }
}