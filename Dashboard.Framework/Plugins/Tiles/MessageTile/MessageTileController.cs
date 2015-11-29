using System.Windows;
using Dashboard.Tiles.Message;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Tiles;


namespace NoeticTools.Dashboard.Framework.Plugins.Tiles.MessageTile
{
    internal sealed class MessageTileController : IViewController
    {
        private readonly TileConfiguration _tileConfiguration;
        private readonly IDashboardController _dashboardController;
        private readonly TileLayoutController _tileLayoutController;
        public static readonly string TileTypeId = "Message";

        public MessageTileController(TileConfiguration tileConfiguration, IDashboardController dashboardController, TileLayoutController tileLayoutController)
        {
            _tileConfiguration = tileConfiguration;
            _dashboardController = dashboardController;
            _tileLayoutController = tileLayoutController;
        }

        public FrameworkElement CreateView()
        {
            return new MessageTileControl {DataContext = new MessageViewModel(_tileConfiguration, _dashboardController, _tileLayoutController)};
        }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
        }
    }
}