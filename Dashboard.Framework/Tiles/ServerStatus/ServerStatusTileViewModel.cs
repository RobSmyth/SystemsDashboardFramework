using System;
using System.Windows;
using System.Windows.Controls;
using Dashboard.Config;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Tiles;
using NoeticTools.TeamDashboard.Tiles;

namespace Dashboard.Tiles.ServerStatus
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