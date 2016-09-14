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
        private readonly TimeSpan _updatePeriod = TimeSpan.FromSeconds(30);
        private readonly IServices _services;
        private readonly TileProperties _tileProperties;
        private double _value;
        private string _label = "";
        private double _maximum = 1.0;
        private double _minimum = -1.0;
        private string _format = "{0}";
        private double _labelsStep;
        private double _ticksStep;
        private Color _ticksColour;

        public GuageAngularTileViewModel(TileConfiguration tile, IDashboardController dashboardController, ITileLayoutController layoutController, IServices services)
        {
            _services = services;
            _tileProperties = new TileProperties(tile, this, services);
            Formatter = x => string.Format(Format, x);

            var parameters = GetConfigurationParameters();
            ConfigureCommand = new TileConfigureCommand(tile, "Data Value Tile Configuration", parameters, dashboardController, layoutController, services);

            _services.Timer.QueueCallback(TimeSpan.FromMilliseconds(100), this);
        }

        private IPropertyViewModel[] GetConfigurationParameters()
        {
            var parameters = new IPropertyViewModel[]
            {
                new TextPropertyAutoCompleteViewModel("Label", _tileProperties.Properties, _services),
                new TextPropertyAutoCompleteViewModel("Value", _tileProperties.Properties, _services),
                new TextPropertyAutoCompleteViewModel("Minimum", _tileProperties.Properties, _services),
                new TextPropertyAutoCompleteViewModel("Maximum", _tileProperties.Properties, _services),
                new TextPropertyViewModel("Format", _tileProperties.Properties),
                new DividerPropertyViewModel(),
                new ColourPropertyViewModel("LabelsStep", _tileProperties.Properties, _services),
                new ColourPropertyViewModel("TicksStep", _tileProperties.Properties, _services),
                new TextPropertyAutoCompleteViewModel("Wedge", _tileProperties.Properties, _services),
                new TextPropertyViewModel("InnerRadius", _tileProperties.Properties),
                new ColourPropertyViewModel("TicksColour", _tileProperties.Properties, _services),
                new ColourPropertyViewModel("LowerColour", _tileProperties.Properties, _services),
                new ColourPropertyViewModel("MidColour", _tileProperties.Properties, _services),
                new ColourPropertyViewModel("UpperColour", _tileProperties.Properties, _services),
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

        private void Update()
        {
            var namedValueReader = _tileProperties.NamedValueRepository;
            Label = namedValueReader.GetString("Label", "Label");
            Minimum = namedValueReader.GetDouble("Minimum");
            Maximum = namedValueReader.GetDouble("Maximum", 100.0);
            Value = namedValueReader.GetDouble("Value");
            Format = namedValueReader.GetString("Format", "{0} %");
            LabelsStep = namedValueReader.GetDouble("LabelsStep", (Maximum-Minimum)/2.0);
            TicksStep = namedValueReader.GetDouble("TicksStep", (Maximum-Minimum)/ DefaultTickStepRatio);
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            Update();
            _services.Timer.QueueCallback(_updatePeriod, this);
        }

        public void SetView(AngularGauge view)
        {
            Update();

            var namedValueReader = _tileProperties.NamedValueRepository;
            var ticksColour = namedValueReader.GetColour("TicksColour", "Gray");
            var wedge = namedValueReader.GetDouble("Wedge", 300.0);
            view.Wedge = wedge;
            view.TicksForeground = new SolidColorBrush(ticksColour);

            InitialiseSpans(view, namedValueReader);
        }

        private void InitialiseSpans(AngularGauge view, INamedValueRepository namedValueRepository)
        {
            var lowerColour = namedValueRepository.GetColour("LowerColour", "LightGray");
            var midColour = namedValueRepository.GetColour("MidColour", "#F8A725");
            var upperColour = namedValueRepository.GetColour("UpperColour", "#FF3939");
            var midSpanStart = Minimum + TicksStep;
            var midSpanEnd = Maximum - TicksStep;
            view.Sections.Add(new AngularSection() {FromValue = Minimum, ToValue = midSpanStart, Fill = new SolidColorBrush(lowerColour)});
            view.Sections.Add(new AngularSection() {FromValue = midSpanStart, ToValue = midSpanEnd, Fill = new SolidColorBrush(midColour)});
            view.Sections.Add(new AngularSection() {FromValue = midSpanEnd, ToValue = Maximum, Fill = new SolidColorBrush(upperColour)});
            view.SectionsInnerRadius = namedValueRepository.GetDouble("InnerRadius", 0.5);
        }
    }
}