using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;


namespace NoeticTools.TeamStatusBoard.Tiles.PieChart
{
    internal sealed class PieChartTileViewModel : NotifyingViewModelBase, IConfigurationChangeListener, ITileViewModel, ITimerListener
    {
        private readonly string[] _defaultColours = { "#F8A725", "#FF3939", "LightGray", "YellowGreen", "Tomato", "LightGreen" };
        private readonly TimeSpan _updatePeriod = TimeSpan.FromSeconds(30);
        private readonly IServices _services;
        private readonly INamedValueRepository _configurationNamedValues;
        private readonly INamedValueRepository _namedValues;
        private double[] _values = new double[0];
        private string _label = "";
        private string _format = "{0}";
        private LegendLocation _legendLocation = LegendLocation.None;
        private LiveCharts.Wpf.PieChart _chart;
        private List<LiveCharts.Wpf.PieSeries> _series = new List<LiveCharts.Wpf.PieSeries>();
        private string[] _titles = new string[0];

        public PieChartTileViewModel(TileConfiguration tile, IDashboardController dashboardController, ITileLayoutController layoutController, IServices services, ITileProperties properties)
        {
            _services = services;
            _configurationNamedValues = properties.Properties;
            _namedValues = properties.NamedValueRepository;
            PointLabel = chartPoint => "";
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

                new CompountTextPropertyViewModel("Values", _configurationNamedValues, _services),
                new CompountTextPropertyViewModel("Titles", _configurationNamedValues, _services),
                new CompountColourPropertyViewModel("Colours", _configurationNamedValues, _services),

                new TextPropertyViewModel("Format", _configurationNamedValues, _services),
                new EnumPropertyViewModel("LegendLocation", _configurationNamedValues, "None", "Top", "Bottom", "Left", "Right"),
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

        public double[] Values
        {
            get { return _values; }
            set
            {
                _values = value;
                OnPropertyChanged();
            }
        }

        public string[] Titles
        {
            get { return _titles; }
            set
            {
                _titles = value;
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
            }
        }

        public LegendLocation LegendLocation
        {
            get { return _legendLocation; }
            private set
            {
                if (_legendLocation == value)
                {
                    return;
                }
                _legendLocation = value;
                OnPropertyChanged();
            }
        }

        public Func<ChartPoint, string> PointLabel { get; set; }

        public ICommand ConfigureCommand { get; }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
            Update();
        }

        private void Update()
        {
            Label = _namedValues.GetString("Label", "Label (alpha)");
            Values = _namedValues.GetDoubleArray("Values");
            Titles = _namedValues.GetStringArray("Titles");
            Format = _namedValues.GetString("Format", "{0} %");
            LegendLocation = (LegendLocation)Enum.Parse(typeof(LegendLocation), _configurationNamedValues.GetString("LegendLocation", "None"));
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            Update();
            _services.Timer.QueueCallback(_updatePeriod, this);
        }

        public void SetView(LiveCharts.Wpf.PieChart chart)
        {
            _chart = chart;

            Update();
            // todo - this needs to be a run-time update - bind?
            for(var index=0; index<Values.Length; index++)
            {
                var title = Titles.Length > index ? Titles[index] : "";
                AddValue(title, Values[index]);
            }

            _chart.Series.AddRange(_series);
        }

        private void AddValue(string title, double value)
        {
            var seriesNumber = _series.Count+1;
            _series.Add(new PieSeries { Title = title, Values = new ChartValues<double>() { value }, Fill = GetSeriesFill(seriesNumber) });
        }

        private SolidColorBrush GetSeriesFill(int seriesNumber)
        {
            return new SolidColorBrush(_configurationNamedValues.GetColour($"Series{seriesNumber}Colour", _defaultColours[seriesNumber-1]));
        }
    }
}