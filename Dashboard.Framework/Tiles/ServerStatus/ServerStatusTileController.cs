using System.Windows;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Config.Commands;


namespace NoeticTools.Dashboard.Framework.Tiles.ServerStatus
{
    internal class ServerStatusTileController : IViewController
    {
        public const string TileTypeId = "Server.Status";
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private ServerStatusTileControl _view;

        public ServerStatusTileController(TileConfiguration tileConfiguration)
        {
            _tileConfigurationConverter = new TileConfigurationConverter(tileConfiguration, this);
            ConfigureCommand = new NullCommand();
        }

        public ICommand ConfigureCommand { get; }

        public FrameworkElement CreateView()
        {
            _view = new ServerStatusTileControl();
            _view.serverName.Text = _tileConfigurationConverter.GetString("Name");
            _view.message.Text = _tileConfigurationConverter.GetString("Message");
            return _view;
        }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
        }
    }
}