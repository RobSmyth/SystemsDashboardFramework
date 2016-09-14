using System;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.Guages.Guage180deg
{
    internal sealed class Guage180DegTileViewModel : NotifyingViewModelBase, IConfigurationChangeListener, ITileViewModel, ITimerListener
    {
        private readonly TimeSpan _updatePeriod = TimeSpan.FromSeconds(30);
        private readonly IServices _services;
        private readonly INamedValueRepository _configurationNamedValues;
        private double _value;
        private string _label = "";
        private double _maximum = 1.0;
        private double _minimum = -1.0;
        private string _format = "-";
        private bool _uses360Mode;
        private Color _fromColour;
        private Color _toColour;
        private INamedValueRepository _namedValues;

        public Guage180DegTileViewModel(TileConfiguration tile, IDashboardController dashboardController, ITileLayoutController layoutController, IServices services, TileProperties properties)
        {
            _services = services;
            _configurationNamedValues = properties.Properties;
            _namedValues = properties.NamedValueRepository;
            Formatter = x => string.Format(Format, x);

            var parameters = GetConfigurationParameters();
            ConfigureCommand = new TileConfigureCommand(tile, "Data Value Tile Configuration", parameters, dashboardController, layoutController, services);

            _services.Timer.QueueCallback(TimeSpan.FromMilliseconds(100), this);
        }

        private IPropertyViewModel[] GetConfigurationParameters()
        {
            var parameters = new IPropertyViewModel[]
            {
                new TextPropertyViewModel("Label", _configurationNamedValues, _services),
                new TextPropertyViewModel("Value", _configurationNamedValues, _services),
                new TextPropertyViewModel("Minimum", _configurationNamedValues, _services),
                new TextPropertyViewModel("Maximum", _configurationNamedValues, _services),
                new TextPropertyViewModel("Format", _configurationNamedValues, _services),
                new BoolPropertyViewModel("Uses360Mode", _configurationNamedValues, _services),
                new ColourPropertyViewModel("FromColour", _configurationNamedValues, _services),
                new ColourPropertyViewModel("ToColour", _configurationNamedValues, _services),
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

        public Color FromColour
        {
            get { return _fromColour; }
            private set
            {
                if (_fromColour != value)
                {
                    _fromColour = value;
                    OnPropertyChanged();
                }
            }
        }

        public Color ToColour
        {
            get { return _toColour; }
            private set
            {
                if (_toColour != value)
                {
                    _toColour = value;
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
            Label = _namedValues.GetString("Label", "Label");
            Format = _namedValues.GetString("Format", "{0} %");
            Minimum = _namedValues.GetDouble("Minimum");
            Maximum = _namedValues.GetDouble("Maximum", 100.0);
            Value = _namedValues.GetDouble("Value");
            Uses360Mode = _namedValues.GetBool("Uses360Mode");
            FromColour = _namedValues.GetColour("FromColour", "Yellow");
            ToColour = _namedValues.GetColour("ToColour", "Crimson");
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            Update();
            _services.Timer.QueueCallback(_updatePeriod, this);
        }
    }
}