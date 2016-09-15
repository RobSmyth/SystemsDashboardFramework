using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using LiveCharts.Wpf;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.Guages.GuageAngular
{
    internal sealed class GuageAngularTileViewModel : NotifyingViewModelBase, IConfigurationChangeListener, ITileViewModel, ITimerListener
    {
        private const double DefaultTickStepRatio = 4.0;
        private const double DefaultLabelsStepRatio = 2.0;
        private readonly TimeSpan _updatePeriod = TimeSpan.FromSeconds(30);
        private readonly IServices _services;
        private readonly INamedValueRepository _configurationNamedValues;
        private readonly INamedValueRepository _namedValues;
        private double _value;
        private string _label = "";
        private double _maximum = 1.0;
        private double _minimum = -1.0;
        private string _format = "{0}";
        private double _labelsStep;
        private double _ticksStep;

        public GuageAngularTileViewModel(TileConfiguration tile, IDashboardController dashboardController, ITileLayoutController layoutController, IServices services, ITileProperties properties)
        {
            _services = services;
            _configurationNamedValues = properties.Properties;
            _namedValues = properties.NamedValueRepository;

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
                new DividerPropertyViewModel(),
                new ColourPropertyViewModel("LabelsStep", _configurationNamedValues, _services),
                new ColourPropertyViewModel("TicksStep", _configurationNamedValues, _services),
                new TextPropertyViewModel("Wedge", _configurationNamedValues, _services),
                new TextPropertyViewModel("InnerRadius", _configurationNamedValues, _services),
                new ColourPropertyViewModel("TicksColour", _configurationNamedValues, _services),
                new DividerPropertyViewModel(),
                new TextPropertyViewModel("LowerSpan", _configurationNamedValues, _services),
                new TextPropertyViewModel("UpperSpan", _configurationNamedValues, _services),
                new ColourPropertyViewModel("LowerColour", _configurationNamedValues, _services),
                new ColourPropertyViewModel("MidColour", _configurationNamedValues, _services),
                new ColourPropertyViewModel("UpperColour", _configurationNamedValues, _services),
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

        public double LabelsStep
        {
            get { return _labelsStep; }
            set
            {
                if (double.IsNaN(value) || value.Equals(_labelsStep))
                {
                    return;
                }
                _labelsStep = value;
                OnPropertyChanged();
            }
        }

        public double TicksStep
        {
            get { return _ticksStep; }
            set
            {
                if (double.IsNaN(value) || value.Equals(_ticksStep))
                {
                    return;
                }
                _ticksStep = value;
                OnPropertyChanged();
            }
        }

        public ICommand ConfigureCommand { get; }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
            Update();
        }

        private double ValueSpan => Maximum - Minimum;

        private void Update()
        {
            Label = _namedValues.GetString("Label", "Label");
            Minimum = _namedValues.GetDouble("Minimum");
            Maximum = _namedValues.GetDouble("Maximum", 100.0);
            Value = _namedValues.GetDouble("Value");
            Format = _namedValues.GetString("Format", "{0}%");
            LabelsStep = _namedValues.GetDouble("LabelsStep", ValueSpan / DefaultLabelsStepRatio);
            TicksStep = _namedValues.GetDouble("TicksStep", ValueSpan / DefaultTickStepRatio);
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            Update();
            _services.Timer.QueueCallback(_updatePeriod, this);
        }

        public void InitialiseGuageView(AngularGauge view)
        {
            view.LabelFormatter = x => string.Format(Format,x);
            Update();

            var ticksColour = _namedValues.GetColour("TicksColour", "Gray");
            var wedge = _namedValues.GetDouble("Wedge", 300.0);
            view.Wedge = wedge;
            view.TicksForeground = new SolidColorBrush(ticksColour);

            InitialiseSpans(view);
        }

        private void InitialiseSpans(AngularGauge view)
        {
            var lowerSpan = _namedValues.GetDouble("LowerSpan", TicksStep);
            var upperSpan = _namedValues.GetDouble("UpperSpan", TicksStep);
            var midSpanStart = Minimum + lowerSpan;
            var midSpanEnd = Maximum - upperSpan;
            var lowerColour = _namedValues.GetColour("LowerColour", "LightGray");
            var midColour = _namedValues.GetColour("MidColour", "#F8A725");
            var upperColour = _namedValues.GetColour("UpperColour", "#FF3939");
            view.Sections.Add(new AngularSection() {FromValue = Minimum, ToValue = midSpanStart, Fill = new SolidColorBrush(lowerColour)});
            view.Sections.Add(new AngularSection() {FromValue = midSpanStart, ToValue = midSpanEnd, Fill = new SolidColorBrush(midColour)});
            view.Sections.Add(new AngularSection() {FromValue = midSpanEnd, ToValue = Maximum, Fill = new SolidColorBrush(upperColour)});
            view.SectionsInnerRadius = _namedValues.GetDouble("InnerRadius", 0.5);
        }
    }
}