using System.Windows.Input;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Config.Commands;
using NoeticTools.SystemsDashboard.Framework.Tiles.ServerStatus;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.ServerStatus
{
    internal sealed class ServerStatusTileViewModel : IConfigurationChangeListener, ITileViewModel
    {
        public const string TileTypeId = "Server.Status";
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private readonly ServerStatusTileControl _view;

        public ServerStatusTileViewModel(TileConfiguration tile, ServerStatusTileControl view)
        {
            _view = view;
            Tile = tile;
            _tileConfigurationConverter = new TileConfigurationConverter(tile, this);
            ConfigureCommand = new NullCommand();
            Update();
        }

        public ICommand ConfigureCommand { get; }

        public TileConfiguration Tile { get; }

        private void Update()
        {
            _view.serverName.Text = _tileConfigurationConverter.GetString("Name");
            _view.message.Text = _tileConfigurationConverter.GetString("Message");
        }

        void IConfigurationChangeListener.OnConfigurationChanged(TileConfigurationConverter converter)
        {
        }
    }
}