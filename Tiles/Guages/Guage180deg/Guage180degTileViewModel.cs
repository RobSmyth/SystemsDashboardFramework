using System;
using System.Collections.Generic;
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
using NoeticTools.TeamStatusBoard.Persistence.Xml;


namespace NoeticTools.TeamStatusBoard.Tiles.Guages.Guage180deg
{
    internal sealed class Guage180DegTileViewModel : ConfiguredTileViewModelBase, IConfigurationChangeListener, ITileViewModel, ITimerListener
    {
        private readonly TimeSpan _updatePeriod = TimeSpan.FromSeconds(30);
        private readonly IServices _services;
        private IDataValue _value = new NullDataValue();
        private IDataValue _label = new DataValue("", "Label", PropertiesFlags.None);
        private IDataValue _maximum = new DataValue("", 100.0, PropertiesFlags.None);
        private IDataValue _minimum = new NullDataValue();
        private IDataValue _fromColour = new DataValue("", "Yellow", PropertiesFlags.None);
        private IDataValue _toColour = new DataValue("", "Crimson", PropertiesFlags.None);
        private string _format = "-";
        private IDataValue _uses360Mode;

        public Guage180DegTileViewModel(TileConfiguration tileConfiguration, IDashboardController dashboardController, ITileLayoutController layoutController, IServices services, ITileProperties properties)
            : base(properties)
        {
            _services = services;
            Formatter = FormatValue;

            var parameters = GetConfigurationParameters();
            ConfigureCommand = new TileConfigureCommand(tileConfiguration, "Data Value Tile Configuration", parameters, dashboardController, layoutController, services);

            Subscribe();

            _services.Timer.QueueCallback(TimeSpan.FromMilliseconds(100), this);
        }

        public string Label => _label.String;

        public double Value => _value.Double;

        public double Maximum => _maximum.Double;

        public double Minimum => _minimum.Double;

        public Color FromColour => _fromColour.Colour;

        public Color ToColour => _toColour.Colour;

        public Func<double, string> Formatter { get; set; }

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
            get { return _uses360Mode.Boolean; }
        }

        public ICommand ConfigureCommand { get; }

        public void OnConfigurationChanged(INamedValueRepository converter)
        {
            Subscribe();
            Update();
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

        private IEnumerable<IPropertyViewModel> GetConfigurationParameters()
        {
            var parameters = new IPropertyViewModel[]
            {
                new TextPropertyViewModel("Label", Configuration, _services),
                new TextPropertyViewModel("Value", Configuration, _services),
                new TextPropertyViewModel("Minimum", Configuration, _services),
                new TextPropertyViewModel("Maximum", Configuration, _services),
                new TextPropertyViewModel("Format", Configuration, _services),
                new BoolPropertyViewModel("Uses360Mode", Configuration, _services),
                new ColourPropertyViewModel("FromColour", Configuration, _services),
                new ColourPropertyViewModel("ToColour", Configuration, _services)
            };
            return parameters;
        }

        private void Subscribe()
        {
            _label = UpdateSubscription(_label, "Label", "Label");
            _minimum = UpdateSubscription(_minimum, "Minimum", 0.0);
            _maximum = UpdateSubscription(_maximum, "Maximum", 100.0);
            _value = UpdateSubscription(_value, "Value", 0.0);
            _fromColour = UpdateSubscription(_fromColour, "FromColour", "Yellow");
            _toColour = UpdateSubscription(_toColour, "ToColour", "Crimson");
            _uses360Mode = UpdateSubscription(_toColour, "Uses360Mode", "False");

            OnPropertyChanged("Label");
            OnPropertyChanged("Value");
            OnPropertyChanged("Minimum");
            OnPropertyChanged("Maximum");
            OnPropertyChanged("FromColour");
            OnPropertyChanged("ToColour");
            OnPropertyChanged("Uses360Mode");
        }

        private void Update()
        {
            Format = NamedValues.GetString("Format", "{0} %");
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            Update();
            _services.Timer.QueueCallback(_updatePeriod, this);
        }
    }
}