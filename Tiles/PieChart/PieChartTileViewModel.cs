using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
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


namespace NoeticTools.TeamStatusBoard.Tiles.PieChart
{
    internal sealed class PieChartTileViewModel : ConfiguredTileViewModelBase, IConfigurationChangeListener, ITileViewModel, ITimerListener
    {
        private readonly string[] _defaultColours = { "#F8A725", "#FF3939", "LightGray", "YellowGreen", "Tomato", "LightGreen" };
        private readonly TimeSpan _updatePeriod = TimeSpan.FromSeconds(30);
        private readonly IServices _services;
        private IDataValue _label = new NullDataValue();
        private string _format = "{0}";
        private LegendLocation _legendLocation = LegendLocation.None;
        private LiveCharts.Wpf.PieChart _chart;
        private readonly List<LiveCharts.Wpf.PieSeries> _series = new List<LiveCharts.Wpf.PieSeries>();
        private string[] _titles = new string[0];
        private SolidColorBrush[] _colours = new SolidColorBrush[0];

        public PieChartTileViewModel(TileConfiguration tile, IDashboardController dashboardController, ITileLayoutController layoutController, IServices services, ITileProperties properties)
            : base(properties)
        {
            _services = services;
            Configuration = properties.Properties;
            NamedValues = properties.NamedValueRepository;
            Values = new ObservableCollection<ObservableValue>();
            PointLabel = chartPoint => "";
            Formatter = FormatValue;

            var parameters = GetConfigurationParameters();
            ConfigureCommand = new TileConfigureCommand(tile, "Data Value Tile Configuration", parameters, dashboardController, layoutController, services);

            Subscribe();

            _services.Timer.QueueCallback(TimeSpan.FromMilliseconds(100), this);
        }

        private void Subscribe()
        {
            _label = UpdateSubscription(_label, "Label", "Label");
            OnPropertyChanged("Label");
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
                new TextPropertyViewModel("Label", Configuration, _services),

                new CompountTextPropertyViewModel("Values", Configuration, _services),
                new CompountTextPropertyViewModel("Titles", Configuration, _services),
                new CompountColourPropertyViewModel("Colours", Configuration, _services),

                new TextPropertyViewModel("Format", Configuration, _services),
                new EnumPropertyViewModel("LegendLocation", Configuration, "None", "Top", "Bottom", "Left", "Right"),
            };
            return parameters;
        }

        public string Label => _label.String;

        public Func<double, string> Formatter { get; set; }

        public ObservableCollection<ObservableValue> Values { get; }

        public string[] Titles
        {
            get { return _titles; }
            set
            {
                if (_titles.SequenceEqual(value))
                {
                    return;
                }
                _titles = value;
                OnPropertyChanged();
                OnChartSeriesChanged();
            }
        }

        public SolidColorBrush[] Colours
        {
            get { return _colours; }
            set
            {
                if (_colours.Select(x => x.Color).SequenceEqual(value.Select(y => y.Color)))
                {
                    return;
                }
                _colours = value;
                OnPropertyChanged();
                OnChartSeriesChanged();
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
            var values = NamedValues.GetDoubleArray("Values");
            SynchroniseValues(values);

            Titles = NamedValues.GetStringArray("Titles");
            Colours = NamedValues.GetColourArray("Colours").Select(x => new SolidColorBrush(x)).ToArray();

            Format = NamedValues.GetString("Format", "{0} %");
            LegendLocation = (LegendLocation)Enum.Parse(typeof(LegendLocation), Configuration.GetString("LegendLocation", "None"));
        }

        private void SynchroniseValues(IReadOnlyList<double> values)
        {
            var priorCount = Values.Count;
            while (Values.Count > values.Count)
            {
                Values.Remove(Values.Last());
            }
            while (Values.Count < values.Count)
            {
                Values.Add(new ObservableValue());
            }
            for (var index = 0; index < values.Count; index++)
            {
                Values[index].Value = values[index];
            }
            if (Values.Count != priorCount)
            {
                OnChartSeriesChanged();
            }
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
            OnChartSeriesChanged();
        }

        private void OnChartSeriesChanged()
        {
            _series.Clear();
            for (var index = 0; index < Values.Count; index++)
            {
                var title = Titles.Length > index ? Titles[index] : "";
                AddValue(title, Values[index]);
            }
            _chart.Series.Clear();
            _chart.Series.AddRange(_series);
        }

        private void AddValue(string title, ObservableValue value)
        {
            var seriesNumber = _series.Count+1;
            var pieSeries = new PieSeries { Title = title, Values = new ChartValues<ObservableValue> { value }, Fill = GetSeriesFill(seriesNumber)};
            _series.Add(pieSeries);
        }

        private SolidColorBrush GetSeriesFill(int seriesNumber)
        {
            return Colours.Length >= seriesNumber ? Colours[seriesNumber - 1] : 
                new SolidColorBrush((Color)ColorConverter.ConvertFromString(_defaultColours[seriesNumber - 1]));
        }
    }
}