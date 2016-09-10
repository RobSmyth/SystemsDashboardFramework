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
        private double _value;
        private string _label = "";
        private double _maximum = 1.0;
        private double _minimum = -1.0;
        private string _format = "-";
        private bool _uses360Mode;

        public Guage180degTileViewModel(TileConfiguration tile, IDashboardController dashboardController, ITileLayoutController layoutController, IServices services)
        {
            _services = services;
            _tileConfigurationConverter = new TileConfigurationConverter(tile, this);
            Formatter = x => string.Format(Format, x);

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
                new TextPropertyViewModel("Label", _tileConfigurationConverter),
                new DependantPropertyViewModel("Value", "TextFromCombobox", _tileConfigurationConverter, dataSourceTypeViewModel,
                    () => _services.DataService.Get((string) dataSourceTypeViewModel.Value).GetAllNames().Cast<object>().ToArray()),
                new TextPropertyViewModel("Minimum", _tileConfigurationConverter),
                new TextPropertyViewModel("Maximum", _tileConfigurationConverter),
                new TextPropertyViewModel("Format", _tileConfigurationConverter),
                new TextPropertyViewModel("Uses360Mode", _tileConfigurationConverter),
            };
            return parameters;
        }

        public string Label
        {
            get { return _label; }
            set
            {
                if (_label != null && _label.Equals(value))
                {
                    return;
                }
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
                if (double.IsNaN(value) || value >= Maximum)
                {
                    return;
                }
                _minimum = value;
                OnPropertyChanged();
            }
        }

        public string Format
        {
            get { return _format; }
            private set
            {
                if (_format != null && _format.Equals(value))
                {
                    return;
                }
                _format = value;
                OnPropertyChanged();
                OnPropertyChanged("Minimum");
                OnPropertyChanged("Maximum");
            }
        }

        public bool Uses360Mode
        {
            get { return _uses360Mode; }
            private set
            {
                if (_uses360Mode != value)
                {
                    _uses360Mode = value;
                    OnPropertyChanged();
                }
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
            Label = _tileConfigurationConverter.GetString("Label", "Label");
            Format = _tileConfigurationConverter.GetString("Format", "{0} %");
            Minimum = _tileConfigurationConverter.GetDouble("Minimum", 0.0);
            Maximum = _tileConfigurationConverter.GetDouble("Maximum", 100.0);
            Value = datasource.Read<double>(_tileConfigurationConverter.GetString("Value"));
            Uses360Mode = _tileConfigurationConverter.GetBool("Uses360Mode");
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            Update();
            _services.Timer.QueueCallback(_updatePeriod, this);
        }
    }
}