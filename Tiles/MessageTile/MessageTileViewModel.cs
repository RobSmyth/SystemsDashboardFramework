using System.Windows.Input;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Tiles.MessageTile
{
    internal sealed class MessageTileViewModel : NotifyingViewModelBase, IConfigurationChangeListener, ITileViewModel
    {
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private string _text;

        public MessageTileViewModel(TileConfiguration tile, IDashboardController dashboardController, ITileLayoutController layoutController, IServices services)
        {
            _tileConfigurationConverter = new TileConfigurationConverter(tile, this);
            var parameters = new IPropertyViewModel[] {new PropertyViewModel("Message", PropertyType.Text, _tileConfigurationConverter)};
            ConfigureCommand = new TileConfigureCommand(tile, "Message Tile Configuration", parameters, dashboardController, layoutController, services);
            Update();
        }

        public string Text
        {
            get { return _text; }
            private set
            {
                if (value == _text) return;
                _text = value;
                OnPropertyChanged();
            }
        }

        public ICommand ConfigureCommand { get; }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
            Update();
        }

        private void Update()
        {
            Text = _tileConfigurationConverter.GetString("Message");
        }
    }
}