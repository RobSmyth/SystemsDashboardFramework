using System;
using System.Linq;
using System.Windows.Input;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Plugins.PropertyEditControls.SuggestionProviders;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.Guages.Guage180deg
{
    internal sealed class Guage180degTileViewModel : NotifyingViewModelBase, IConfigurationChangeListener, ITileViewModel, ITimerListener
    {
        private readonly TimeSpan _updatePeriod = TimeSpan.FromSeconds(30);
        private readonly IServices _services;
        private readonly INamedValueReader _tileProperties;
        private readonly INamedValueReader _namedValueReader;
        private double _value;
        private string _label = "";
        private double _maximum = 1.0;
        private double _minimum = -1.0;
        private string _format = "-";
        private bool _uses360Mode;
        private string _xyz;

        public Guage180degTileViewModel(TileConfiguration tile, IDashboardController dashboardController, ITileLayoutController layoutController, IServices services)
        {
            _services = services;
            _tileProperties = new TileConfigurationConverter(tile, this);
            _namedValueReader = new ConfigurationNamedValueReaderDecorator(_tileProperties, new NamedValueReaderAggregator(new DataSourceNamedValueReaderProvider(_services), new NullNamedValueReaderProvider()));
            Formatter = x => string.Format(Format, x);

            var parameters = GetConfigurationParameters();
            ConfigureCommand = new TileConfigureCommand(tile, "Data Value Tile Configuration", parameters, dashboardController, layoutController, services);

            _services.Timer.QueueCallback(TimeSpan.FromMilliseconds(100), this);
        }

        private IPropertyViewModel[] GetConfigurationParameters()
        {
            var parameters = new IPropertyViewModel[]
            {
                new AutoCompleteTextPropertyViewModel("Label", _tileProperties, _services),
                new AutoCompleteTextPropertyViewModel("Value", _tileProperties, _services),
                new AutoCompleteTextPropertyViewModel("Minimum", _tileProperties, _services),
                new AutoCompleteTextPropertyViewModel("Maximum", _tileProperties, _services),
                new TextPropertyViewModel("Format", _tileProperties),
                new AutoCompleteBoolPropertyViewModel("Uses360Mode", _tileProperties, _services), 
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

        public string XYZ
        {
            get { return _xyz; }
            private set
            {
                if (_xyz != value)
                {
                    _xyz = value;
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
            Label = _namedValueReader.GetString("Label", "Label");
            Format = _namedValueReader.GetString("Format", "{0} %");
            Minimum = _namedValueReader.GetDouble("Minimum");
            Maximum = _namedValueReader.GetDouble("Maximum", 100.0);
            Value = _namedValueReader.GetDouble("Value");
            Uses360Mode = _namedValueReader.GetBool("Uses360Mode");
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            Update();
            _services.Timer.QueueCallback(_updatePeriod, this);
        }
    }
}