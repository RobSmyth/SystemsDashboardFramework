using System.Windows.Input;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Config.Properties;
using NoeticTools.SystemsDashboard.Framework.Commands;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.MessageTile
{
    internal sealed class MessageViewModel : NotifyingViewModelBase, IConfigurationChangeListener
    {
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private string _text;

        public MessageViewModel(TileConfiguration tile, IDashboardController dashboardController, TileLayoutController tileLayoutController, IServices services)
        {
            _tileConfigurationConverter = new TileConfigurationConverter(tile, this);
            var parameters = new IPropertyViewModel[] {new PropertyViewModel("Message", "Text", _tileConfigurationConverter)};
            ConfigureCommand = new TileConfigureCommand(tile, "Message Tile Configuration", parameters, dashboardController, tileLayoutController, services);
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