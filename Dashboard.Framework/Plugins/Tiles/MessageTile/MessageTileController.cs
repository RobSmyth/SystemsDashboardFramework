using System.Windows;
using NoeticTools.SystemsDashboard.Framework.Commands;
using NoeticTools.SystemsDashboard.Framework.Config;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.MessageTile
{
    internal sealed class MessageTileController : IViewController
    {
        private readonly IDashboardController _dashboardController;
        public static readonly string TileTypeId = "Message";
        private readonly TileLayoutController _layoutController;
        private readonly IServices _services;

        public MessageTileController(TileConfiguration tile, IDashboardController dashboardController, TileLayoutController layoutController, IServices services)
        {
            Tile = tile;
            _dashboardController = dashboardController;
            _layoutController = layoutController;
            _services = services;
        }

        private TileConfiguration Tile { get; }

        public FrameworkElement CreateView()
        {
            return new MessageTileControl {DataContext = new MessageViewModel(Tile, _dashboardController, _layoutController, _services)};
        }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
        }
    }
}