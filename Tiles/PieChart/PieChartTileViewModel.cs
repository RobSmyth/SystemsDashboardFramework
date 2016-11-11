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
using NoeticTools.TeamStatusBoard.Framework.Config.SuggestionProviders;
using NoeticTools.TeamStatusBoard.Framework.Config.XmlTypes;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;


namespace NoeticTools.TeamStatusBoard.Tiles.PieChart
{
    internal sealed class PieChartTileViewModel : LiveChartTileViewModelBase, IConfigurationChangeListener, ITileViewModel, ITimerListener
    {
        private readonly string[] _defaultColours = {"#F8A725", "#FF3939", "LightGray", "YellowGreen", "Tomato", "LightGreen"};
        private readonly TimeSpan _updatePeriod = TimeSpan.FromSeconds(30);
        private readonly IServices _services;
        private readonly List<PieSeries> _series = new List<PieSeries>();
        private IDataValue _label = new NullDataValue();
        private IDataValue _format = new NullDataValue();
        private LegendLocation _legendLocation = LegendLocation.None;
        private LiveCharts.Wpf.PieChart _chart;
        private IEnumerable<IDataValue> _titles = new IDataValue[0];
        private IEnumerable<IDataValue> _colours = new IDataValue[0];
        private IEnumerable<IDataValue> _dataValues = new IDataValue[0];

        public PieChartTileViewModel(TileConfiguration tile, IDashboardController dashboardController, ITileLayoutController layoutController, IServices services, ITileProperties properties)
            : base(properties)
        {
            _services = services;
            Values = new ObservableCollection<ObservableValue>();
            Titles = new ObservableCollection<string>();
            Colours = new ObservableCollection<SolidColorBrush>();
            PointLabel = chartPoint => "";
            Formatter = FormatValue;

            var parameters = GetConfigurationParameters();
            ConfigureCommand = new TileConfigureCommand(tile, "Data Value Tile Configuration", parameters, dashboardController, layoutController, services);

            Subscribe();

            _services.Timer.QueueCallback(TimeSpan.FromMilliseconds(100), this);
        }

        public string Label => _label.String;

        public Func<double, string> Formatter { get; set; }

        public ObservableCollection<ObservableValue> Values { get; }

        public ObservableCollection<string> Titles { get; }

        public ObservableCollection<SolidColorBrush> Colours { get; }

        public string Format => _format.String;

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

        public void OnConfigurationChanged(INamedValueRepository converter)
        {
            Subscribe();
            Update();
        }

        public void SetView(LiveCharts.Wpf.PieChart chart)
        {
            _chart = chart;
            _chart.AnimationsSpeed = TimeSpan.FromSeconds(1.0);
            Update();
            OnChartSeriesChanged();
        }

        private void Subscribe()
        {
            _label = UpdateSubscription(_label, "Label", "Label");

            var priorCount = _dataValues.Count();
            _dataValues = UpdateSubscriptions(_dataValues, "Values", Values);
            if (_dataValues.Count() != priorCount)
            {
                OnChartSeriesChanged();
            }

            _titles = UpdateSubscription(_titles, "Titles", Titles, x => x.String);
            _colours = UpdateSubscription(_colours, "Colours", Colours, x => x.SolidColourBrush);

            _format = UpdateSubscription(_format, "Format", "{0} %");

            OnPropertyChanged("Label");
            OnPropertyChanged("Values");
            OnPropertyChanged("Titles");
            OnPropertyChanged("Format");
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
                new CompoundColourPropertyViewModel("Colours", Configuration, _services),
                new TextPropertyViewModel("Format", Configuration, _services),
                new EnumPropertyViewModel("LegendLocation", Configuration, new TextListSuggestionProvider("None", "Top", "Bottom", "Left", "Right")),
            };
            return parameters;
        }

        private void Update()
        {
            LegendLocation = (LegendLocation) Enum.Parse(typeof(LegendLocation), Configuration.GetString("LegendLocation", "None"));
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            Update();
            _services.Timer.QueueCallback(_updatePeriod, this);
        }

        private void OnChartSeriesChanged()
        {
            if (_chart == null)
            {
                return;
            }
            _series.Clear();
            for (var index = 0; index < Values.Count; index++)
            {
                var title = Titles.Count > index ? Titles[index] : "";
                AddValue(title, Values[index]);
            }
            _chart.Series.Clear();
            _chart.Series.AddRange(_series);
        }

        private void AddValue(string title, ObservableValue value)
        {
            var seriesNumber = _series.Count + 1;
            var pieSeries = new PieSeries {Title = title, Values = new ChartValues<ObservableValue> {value}, Fill = GetSeriesFill(seriesNumber)};
            _series.Add(pieSeries);
        }

        private SolidColorBrush GetSeriesFill(int seriesNumber)
        {
            return Colours.Count >= seriesNumber
                ? Colours[seriesNumber - 1]
                : new SolidColorBrush((Color) ColorConverter.ConvertFromString(_defaultColours[seriesNumber - 1]));
        }
    }
}