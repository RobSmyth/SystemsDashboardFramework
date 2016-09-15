using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.PieChart
{
    internal sealed class PieChartTileViewModel : NotifyingViewModelBase, IConfigurationChangeListener, ITileViewModel, ITimerListener
    {
        private readonly TimeSpan _updatePeriod = TimeSpan.FromSeconds(30);
        private readonly IServices _services;
        private readonly INamedValueRepository _configurationNamedValues;
        private readonly INamedValueRepository _namedValues;
        private double _value;
        private string _label = "";
        private string _format = "{0}";
        private LegendLocation _legendLocation = LegendLocation.None;

        public PieChartTileViewModel(TileConfiguration tile, IDashboardController dashboardController, ITileLayoutController layoutController, IServices services, ITileProperties properties)
        {
            _services = services;
            _configurationNamedValues = properties.Properties;
            _namedValues = properties.NamedValueRepository;
            //PointLabel = chartPoint => $"{chartPoint.Y} ({chartPoint.Participation:P})";
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
                new TextPropertyViewModel("Value", _configurationNamedValues, _services),
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

        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
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
            Label = _namedValues.GetString("Label", "Label");
            Value = _namedValues.GetDouble("Value");
            Format = _namedValues.GetString("Format", "{0} %");
            LegendLocation = (LegendLocation)Enum.Parse(typeof(LegendLocation), _configurationNamedValues.GetString("LegendLocation", "None"));
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            Update();
            _services.Timer.QueueCallback(_updatePeriod, this);
        }
    }
}