using System;
using System.Windows.Input;
using System.Windows.Media;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;


namespace NoeticTools.TeamStatusBoard.Tiles.Guages.Guage180deg
{
    internal sealed class Guage180DegTileViewModel : ConfiguredTileViewModelBase, IConfigurationChangeListener, ITileViewModel, ITimerListener
    {
        private readonly TimeSpan _updatePeriod = TimeSpan.FromSeconds(30);
        private readonly IServices _services;
        private readonly INamedValueRepository _configuration;
        private IDataValue _value = new NullDataValue();
        private IDataValue _label = new DataValue("", "Label", PropertiesFlags.None, () => { });
        private IDataValue _maximum = new DataValue("", 100.0, PropertiesFlags.None, () => { });
        private IDataValue _minimum = new NullDataValue();
        private string _format = "-";
        private bool _uses360Mode;
        private IDataValue _fromColour = new DataValue("", "Yellow", PropertiesFlags.None, () => { });
        private IDataValue _toColour = new DataValue("", "Crimson", PropertiesFlags.None, () => { });
        private readonly INamedValueRepository _namedValues;

        public Guage180DegTileViewModel(TileConfiguration tile, IDashboardController dashboardController, ITileLayoutController layoutController, IServices services, ITileProperties properties)
            : base(properties)
        {
            _services = services;
            _configuration = properties.Properties;
            _namedValues = properties.NamedValueRepository;
            Formatter = FormatValue;

            var parameters = GetConfigurationParameters();
            ConfigureCommand = new TileConfigureCommand(tile, "Data Value Tile Configuration", parameters, dashboardController, layoutController, services);

            Subscribe();

            _services.Timer.QueueCallback(TimeSpan.FromMilliseconds(100), this);
        }

        private string FormatValue(double x)
        {
            try
            {
                return string.Format(Format, x);
            }
            catch (Exception)
            {
                return $"{x}";
            }
        }

        private IPropertyViewModel[] GetConfigurationParameters()
        {
            var parameters = new IPropertyViewModel[]
            {
                new TextPropertyViewModel("Label", _configuration, _services),
                new TextPropertyViewModel("Value", _configuration, _services),
                new TextPropertyViewModel("Minimum", _configuration, _services),
                new TextPropertyViewModel("Maximum", _configuration, _services),
                new TextPropertyViewModel("Format", _configuration, _services),
                new BoolPropertyViewModel("Uses360Mode", _configuration, _services),
                new ColourPropertyViewModel("FromColour", _configuration, _services),
                new ColourPropertyViewModel("ToColour", _configuration, _services),
            };
            return parameters;
        }

        public string Label => _label.String;
        public Func<double, string> Formatter { get; set; }
        public double Value => _value.Double;
        public double Maximum => _maximum.Double;
        public double Minimum => _minimum.Double;

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

        public Color FromColour => _fromColour.Colour;
        public Color ToColour => _toColour.Colour;

        public ICommand ConfigureCommand { get; }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
            Subscribe();
            Update();
        }

        private void Subscribe()
        {
            _label = Subscribe(_label, "Label", "Label");
            _minimum = Subscribe(_minimum, "Minimum", 0.0);
            _maximum = Subscribe(_maximum, "Maximum", 100.0);
            _value = Subscribe(_value, "Value", 0.0);
            _fromColour = Subscribe(_fromColour, "FromColour", "Yellow");
            _toColour = Subscribe(_toColour, "ToColour", "Crimson");

            OnPropertyChanged("Label");
            OnPropertyChanged("Value");
            OnPropertyChanged("Minimum");
            OnPropertyChanged("Maximum");
            OnPropertyChanged("FromColour");
            OnPropertyChanged("ToColour");
        }

        private IDataValue Subscribe(IDataValue existing, string propertyName, object defaultValue)
        {
            var datum = string.IsNullOrWhiteSpace(propertyName) ? (IDataValue)new NullDataValue() : _namedValues.GetDatum(propertyName, defaultValue);
            if (ReferenceEquals(existing, datum))
            {
                return existing;
            }

            existing.Broadcaster.RemoveListener(this);
            datum.Broadcaster.AddListener(this, () => OnPropertyChanged(propertyName));

            return datum;
        }

        private void Update()
        {
            Format = _namedValues.GetString("Format", "{0} %");
            Uses360Mode = _namedValues.GetBool("Uses360Mode");
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            Update();
            _services.Timer.QueueCallback(_updatePeriod, this);
        }
    }
}