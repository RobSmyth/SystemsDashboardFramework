using System;
using System.Windows.Controls;
using Dashboard.Config;
using NoeticTools.Dashboard.Framework.Config;
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
            Id = tileConfiguration.Id;
        }

        public void Start(Panel placeholderPanel)
        {
            _view = new ServerStatusTileControl();
            placeholderPanel.Children.Add(_view);
            _view.serverName.Text = _tileConfiguration.GetString("Name");
            _view.message.Text = _tileConfiguration.GetString("Message");
        }

        public string TypeId
        {
            get { return TileTypeId; }
        }

        public Guid Id { get; private set; }

        public void OnConfigurationChanged()
        {
        }
    }
}