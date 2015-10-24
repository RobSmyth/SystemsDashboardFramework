using System;
using System.Windows.Controls;
using Dashboard.Config;

namespace Dashboard.Tiles.ServerStatus
{
    internal class ServerStatusTileViewModel : ITileViewModel
    {
        public static readonly Guid TileTypeId = new Guid("0FFACE9A-8B68-4DBC-8B42-0255F51368B4");
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

        public Guid TypeId
        {
            get { return TileTypeId; }
        }

        public Guid Id { get; private set; }

        public void OnConfigurationChanged()
        {
        }
    }
}