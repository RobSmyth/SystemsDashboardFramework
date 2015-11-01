using System.Windows;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Config.Commands;


namespace NoeticTools.Dashboard.Framework.Tiles.ServerStatus
{
    internal class ServerStatusTileViewModel : ITileViewModel
    {
        public const string TileTypeId = "Server.Status";
        private readonly TileConfiguration _tileConfiguration;
        private ServerStatusTileControl _view;

        public ServerStatusTileViewModel(DashboardTileConfiguration tileConfiguration)
        {
            _tileConfiguration = new TileConfiguration(tileConfiguration, this);
            ConfigureCommand = new NullCommand();
        }

        public ICommand ConfigureCommand { get; }

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