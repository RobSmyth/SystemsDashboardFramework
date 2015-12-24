using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using log4net;
using NoeticTools.SystemsDashboard.Framework.Commands;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Config.Properties;
using NoeticTools.SystemsDashboard.Framework.DataSources.TeamCity;
using NoeticTools.SystemsDashboard.Framework.Time;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.TeamCity.LastBuildStatus
{
    internal sealed class TeamCityLastBuildStatusTileViewModel : NotifyingViewModelBase, ITimerListener, IConfigurationChangeListener, ITileViewModel
    {
        public const string TileTypeId = "TeamCity.Build.Status";

        private readonly Dictionary<string, Brush> _statusTextBrushes = new Dictionary<string, Brush>
        {
            {"RUNNING FAILED", Brushes.White},
            {"RUNNING", Brushes.DarkSlateGray},
            {"SUCCESS", Brushes.White},
            {"FAILURE", Brushes.White},
            {"UNKNOWN", Brushes.White}
        };

        private readonly TeamCityService _service;
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private readonly TimeSpan _connectedUpdatePeriod = TimeSpan.FromSeconds(30);
        private readonly TimeSpan _disconnectedUpdatePeriod = TimeSpan.FromSeconds(2);
        private readonly TimerToken _timerToken;
        private readonly TeamCityBuildStatusTileControl _view;
        private readonly ILog _logger;
        private readonly object _syncRoot = new object();
        private string _status;
        private static int _nextInstanceId = 1;

        public TeamCityLastBuildStatusTileViewModel(TeamCityService service, TileConfiguration tile, IDashboardController dashboardController, TileLayoutController layoutController, IServices services,
            TeamCityBuildStatusTileControl view)
        {
            lock (_syncRoot)
            {
                _logger = LogManager.GetLogger($"Tiles.TeamCity.LastBuildStatus.{_nextInstanceId++}");
            }

            _service = service;
            _view = view;
            _tileConfigurationConverter = new TileConfigurationConverter(tile, this);
            _status = "UNKNOWN";
            ConfigureServiceCommand = new TeamCityServiceConfigureCommand(service);
            var configurationParameters = GetConfigurationParameters();
            ConfigureCommand = new TileConfigureCommand(tile, "Last Build Status Tile Configuration", configurationParameters, dashboardController, layoutController, services);
            _view.DataContext = this;
            _timerToken = services.Timer.QueueCallback(TimeSpan.FromSeconds(_service.IsConnected ? 1 : 3), this);
        }

        public string Status
        {
            get { return _status; }
            private set
            {
                if (!_status.Equals(value, StringComparison.InvariantCulture))
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand ConfigureCommand { get; private set; }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
            _logger.Debug("Configuration changed.");
            _timerToken.Requeue(TimeSpan.FromMilliseconds(300));
        }

        public void OnTimeElapsed(TimerToken token)
        {
            if (!_service.IsConnected)
            {
                SetUiToError();
                _timerToken.Requeue(_disconnectedUpdatePeriod);
                return;
            }

            _logger.Debug("Timer elapsed. Update.");

            var projectName = _tileConfigurationConverter.GetString("Project");
            var configurationName = _tileConfigurationConverter.GetString("Configuration");

            Task.Factory.StartNew(() => GetBuilds(projectName, configurationName)).ContinueWith(x => _view.Dispatcher.InvokeAsync(() => Update(x.Result)));
        }

        private Build[] GetBuilds(string projectName, string configurationName)
        {
            var build = _service.GetRunningBuilds(projectName, configurationName).Result;
            if (build.Any())
            {
                return build;
            }

            _logger.Debug("No build running, getting last build.");
            var lastBuild = _service.GetLastBuild(projectName, configurationName).Result;
            if (lastBuild == null)
            {
                return new Build[0];
            }
            return new[] {lastBuild};
        }

        private void Update(Build[] builds)
        {
            var build = builds.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(build?.Status))
            {
                SetUiToError();
                _timerToken.Requeue(_disconnectedUpdatePeriod);
                return;
            }

            Status = build.Status;

            var running = build.Status.StartsWith("RUNNING");

            var status = build.Status;
            _view.statusText.Text = _tileConfigurationConverter.GetString("Description");
            _view.buildVer.Text = build.Number;
            _view.headerText.Text = running ? "Running" : "";
            //_view.root.Background = _statusBrushes[status];
            _view.agentsCount.Visibility = running ? Visibility.Visible : Visibility.Collapsed;
            _view.agentsCount.Text = builds.Length.ToString();
            SetTextForeground(running, status);

            _logger.DebugFormat("Updated UI. Build is {0}.", status);

            _timerToken.Requeue(_connectedUpdatePeriod);
        }

        private void SetUiToError()
        {
            _logger.Warn("Update UI - unable to get build state.");
            Status = "UNKNOWN";

            _view.statusText.Text = "ERROR";
            _view.buildVer.Text = "";
            _view.headerText.Text = "";
            //_view.root.Background = _statusBrushes["UNKNOWN"];
            _view.agentsCount.Visibility = Visibility.Collapsed;
        }

        private IPropertyViewModel[] GetConfigurationParameters()
        {
            var projectElementViewModel = new TeamCityProjectPropertyViewModel("Project", _tileConfigurationConverter, _service);
            var configurationParameters = new IPropertyViewModel[]
            {
                projectElementViewModel,
                new DependantPropertyViewModel("Configuration", "TextFromCombobox", _tileConfigurationConverter, projectElementViewModel,
                    () => _service.GetConfigurationNames((string) projectElementViewModel.Value).Result.Cast<object>().ToArray()),
                new PropertyViewModel("Description", "Text", _tileConfigurationConverter),
                new HyperlinkPropertyViewModel("TeamCity service", ConfigureServiceCommand)
            };
            return configurationParameters;
        }

        private void SetTextForeground(bool running, string status)
        {
            var textBrush = running ? _statusTextBrushes[status] : _statusTextBrushes[status];
            _view.headerText.Foreground = textBrush;
            _view.statusText.Foreground = textBrush;
            _view.buildVer.Foreground = textBrush;
            _view.agentsCount.Foreground = textBrush;
        }

        private ICommand ConfigureServiceCommand { get; }
    }
}