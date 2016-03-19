using System;
using System.Windows.Input;
using NoeticTools.SystemsDashboard.Framework.Commands;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Config.Properties;
using NoeticTools.SystemsDashboard.Framework.Dashboards;
using NoeticTools.SystemsDashboard.Framework.Services;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.DataTiles.DateTimeTile
{
    internal sealed class DateTimeDataTileViewModel : NotifyingViewModelBase, IConfigurationChangeListener, ITileViewModel
    {
        private readonly IServices _services;
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private string _text;

        public DateTimeDataTileViewModel(TileConfiguration tile, IDashboardController dashboardController, TileLayoutController layoutController, IServices services)
        {
            _services = services;
            _tileConfigurationConverter = new TileConfigurationConverter(tile, this);
            var parameters = new IPropertyViewModel[]
            {
                new PropertyViewModel("PropertyAddress", "Text", _tileConfigurationConverter),
                new PropertyViewModel("DateTimeFormat", "Text", _tileConfigurationConverter),
            };
            ConfigureCommand = new TileConfigureCommand(tile, "Text Data Tile Configuration", parameters, dashboardController, layoutController, services);
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
            var propertyAddress = _tileConfigurationConverter.GetString("PropertyAddress");
            var dateTimeFormat = _tileConfigurationConverter.GetString("DateTimeFormat");
            if (string.IsNullOrWhiteSpace(dateTimeFormat))
            {
                dateTimeFormat = "M";
                _tileConfigurationConverter.SetParameter("DateTimeFormat", dateTimeFormat);
            }
            var utcTime = _services.DataService.Read<string>(propertyAddress);
            DateTime dateTime;
            if (!DateTime.TryParse(utcTime, out dateTime))
            {
                Text = "ERROR";
            }
            else
            {
                Text = dateTime.ToString(dateTimeFormat);
            }
        }
    }
}