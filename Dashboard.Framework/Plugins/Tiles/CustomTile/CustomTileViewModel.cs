using System.Linq;
using System.Windows.Input;
using NoeticTools.SystemsDashboard.Framework.Commands;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Config.Properties;
using NoeticTools.SystemsDashboard.Framework.Dashboards;
using NoeticTools.SystemsDashboard.Framework.Services;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.CustomTile
{
    internal sealed class CustomTileViewModel : NotifyingViewModelBase, IConfigurationChangeListener, ITileViewModel
    {
        private readonly IServices _services;
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private string _text;

        public CustomTileViewModel(TileConfiguration tile, IDashboardController dashboardController, TileLayoutController layoutController, IServices services)
        {
            _services = services;
            _tileConfigurationConverter = new TileConfigurationConverter(tile, this);
            var parameters = new IPropertyViewModel[]
            {
                new PropertyViewModel("Address1", "Text", _tileConfigurationConverter),
                new PropertyViewModel("Address1", "Text", _tileConfigurationConverter),
                new PropertyViewModel("Address1", "Text", _tileConfigurationConverter),
                new PropertyViewModel("Address1", "Text", _tileConfigurationConverter),
                new PropertyViewModel("Address1", "Text", _tileConfigurationConverter),
            };
            ConfigureCommand = new TileConfigureCommand(tile, "Custom Tile Configuration", parameters, dashboardController, layoutController, services);
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
            var propertyAddress = _tileConfigurationConverter.GetString("Address1");
            Text = _services.DataService.Read<string>(propertyAddress);
        }
    }
}