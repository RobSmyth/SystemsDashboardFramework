using System;
using System.Linq;
using System.Windows.Input;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.Guages.Guage180deg
{
    internal sealed class Guage180degTileViewModel : NotifyingViewModelBase, IConfigurationChangeListener, ITileViewModel, ITimerListener
    {
        private readonly TimeSpan _updatePeriod = TimeSpan.FromSeconds(30);
        private readonly IServices _services;
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private string _text;
        private double _value;
        private string _label;
        private double _maximum;
        private double _minimum;

        public Guage180degTileViewModel(TileConfiguration tile, IDashboardController dashboardController, ITileLayoutController layoutController, IServices services)
        {
            _services = services;
            _tileConfigurationConverter = new TileConfigurationConverter(tile, this);
            Formatter = x => x + " %";
            Label = "Running Build Agents";
            Maximum = 100.0;
            Minimum = 0.0;

            var dataSourceTypeViewModel = new DataSourceTypePropertyViewModel("Datasource", _tileConfigurationConverter, _services);

            var parameters = GetConfigurationParameters(dataSourceTypeViewModel);
            ConfigureCommand = new TileConfigureCommand(tile, "Data Value Tile Configuration", parameters, dashboardController, layoutController, services);

            _services.Timer.QueueCallback(TimeSpan.FromMilliseconds(100), this);
        }

        private IPropertyViewModel[] GetConfigurationParameters(INotifyingPropertyViewModel dataSourceTypeViewModel)
        {
            var parameters = new IPropertyViewModel[]
            {
                dataSourceTypeViewModel,
                new DependantPropertyViewModel("Value", "TextFromCombobox", _tileConfigurationConverter, dataSourceTypeViewModel,
                    () => _services.DataService.Get((string) dataSourceTypeViewModel.Value).GetAllNames().Cast<object>().ToArray()),
                new DependantPropertyViewModel("Maximum", "TextFromCombobox", _tileConfigurationConverter, dataSourceTypeViewModel,
                    () => _services.DataService.Get((string) dataSourceTypeViewModel.Value).GetAllNames().Cast<object>().ToArray()),
            };
            return parameters;
        }

        public string Label
        {
            get { return _label; }
            set
            {
                _label = value;
                OnPropertyChanged();
            }
        }

        public Func<double, string> Formatter { get; set; }

        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }

        public double Maximum
        {
            get { return _maximum; }
            set
            {
                if (double.IsNaN(value) || value <= Minimum)
                {
                    return;
                }
                _maximum = value;
                OnPropertyChanged();
            }
        }

        public double Minimum
        {
            get { return _minimum; }
            set
            {
                _minimum = value;
                OnPropertyChanged();
            }
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
            var datasource = _services.DataService.Get(_tileConfigurationConverter.GetString("Datasource"));
            Value = datasource.Read<double>(_tileConfigurationConverter.GetString("Value"));
            Maximum = datasource.Read<double>(_tileConfigurationConverter.GetString("Maximum"));
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            Update();
            _services.Timer.QueueCallback(_updatePeriod, this);
        }
    }
}