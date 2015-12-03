using System.Windows;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Tiles;


namespace NoeticTools.Dashboard.Framework.Plugins.Tiles.MessageTile
{
    internal sealed class MessageTileController : IViewController
    {
        private readonly TileConfiguration _tileConfiguration;
        private readonly IDashboardController _dashboardController;
        public static readonly string TileTypeId = "Message";
        private readonly TileLayoutController _layoutController;

        public MessageTileController(TileConfiguration tileConfiguration, IDashboardController dashboardController, TileLayoutController layoutController)
        {
            _tileConfiguration = tileConfiguration;
            _dashboardController = dashboardController;
            _layoutController = layoutController;
        }

        public FrameworkElement CreateView()
        {
            return new MessageTileControl {DataContext = new MessageViewModel(_tileConfiguration, _dashboardController, _layoutController)};
        }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
        }
    }
}