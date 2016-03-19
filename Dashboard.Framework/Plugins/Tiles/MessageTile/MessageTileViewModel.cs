using System.Windows.Input;
using NoeticTools.SystemsDashboard.Framework.Commands;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Config.Properties;
using NoeticTools.SystemsDashboard.Framework.Dashboards;
using NoeticTools.SystemsDashboard.Framework.Services;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.MessageTile
{
    internal sealed class MessageTileViewModel : NotifyingViewModelBase, IConfigurationChangeListener, ITileViewModel
    {
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private string _text;

        public MessageTileViewModel(TileConfiguration tile, IDashboardController dashboardController, TileLayoutController layoutController, IServices services)
        {
            _tileConfigurationConverter = new TileConfigurationConverter(tile, this);
            var parameters = new IPropertyViewModel[] {new PropertyViewModel("Message", "Text", _tileConfigurationConverter)};
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

        public ICommand ConfigureCommand { get; private set; }

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