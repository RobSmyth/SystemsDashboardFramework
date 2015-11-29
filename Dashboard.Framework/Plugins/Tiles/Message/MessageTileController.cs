using System.Windows;
using Dashboard.Tiles.Message;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Tiles;


namespace NoeticTools.Dashboard.Framework.Plugins.Tiles.Message
{
    internal class MessageTileController : IViewController
    {
        private readonly TileConfiguration _tileConfiguration;
        private readonly IDashboardController _dashboardController;
        private readonly TileLayoutController _tileLayoutController;
        public static readonly string TileTypeId = "Message";
        private MessageTileControl _view;

        public MessageTileController(TileConfiguration tileConfiguration, IDashboardController dashboardController, TileLayoutController tileLayoutController)
        {
            _tileConfiguration = tileConfiguration;
            _dashboardController = dashboardController;
            _tileLayoutController = tileLayoutController;
        }

        public FrameworkElement CreateView()
        {
            _view = new MessageTileControl();
            _view.DataContext = new MessageViewModel(_tileConfiguration, _dashboardController, _tileLayoutController);

            return _view;
        }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
        }
    }
}