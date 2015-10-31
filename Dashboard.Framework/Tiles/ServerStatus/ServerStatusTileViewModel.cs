using System.Windows;
using NoeticTools.Dashboard.Framework.Config;

namespace NoeticTools.Dashboard.Framework.Tiles.ServerStatus
{
    internal class ServerStatusTileViewModel : ITileViewModel
    {
        public static readonly string TileTypeId = "Server.Status";
        private readonly TileConfiguration _tileConfiguration;
        private ServerStatusTileControl _view;

        public ServerStatusTileViewModel(DashboardTileConfiguration tileConfiguration)
        {
            _tileConfiguration = new TileConfiguration(tileConfiguration, this);
        }

        public FrameworkElement CreateView()
        {
            _view = new ServerStatusTileControl();
            _view.serverName.Text = _tileConfiguration.GetString("Name");
            _view.message.Text = _tileConfiguration.GetString("Message");
            return _view;
        }

        public void OnConfigurationChanged()
        {
        }
    }
}