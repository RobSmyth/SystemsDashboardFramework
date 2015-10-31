using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Dashboard.Config.Parameters;
using Dashboard.Tiles.TeamCityLastBuildStatus;
using NoeticTools.Dashboard.Framework;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Config.Commands;
using NoeticTools.Dashboard.Framework.Config.Parameters;
using NoeticTools.Dashboard.Framework.DataSources.TeamCity;
using NoeticTools.Dashboard.Framework.Time;

namespace NoeticTools.TeamDashboard.Tiles.TeamCityLastBuildStatus
{
    internal class TeamCityLastBuildStatusTileViewModel : ITileViewModel, ITimerListener
    {
        public static readonly string TileTypeId = "TeamCity.Build.Status";

        private readonly Dictionary<string, Brush> _runningStatusBrushes = new Dictionary<string, Brush>
        {
            {"SUCCESS", Brushes.Yellow},
            {"FAILURE", Brushes.OrangeRed},
            {"UNKNOWN", Brushes.Gray},
        };

        private readonly Dictionary<string, Brush> _runningStatusTextBrushes = new Dictionary<string, Brush>
        {
            {"SUCCESS", Brushes.DarkSlateGray},
            {"FAILURE", Brushes.White},
            {"UNKNOWN", Brushes.White},
        };

        private readonly TeamCityService _service;
        private readonly IDashboardController _dashboardController;

        private readonly Dictionary<string, Brush> _statusBrushes = new Dictionary<string, Brush>
        {
            {"SUCCESS", (SolidColorBrush) (new BrushConverter().ConvertFrom("#FF448032"))},
            {"FAILURE", Brushes.Firebrick},
            {"UNKNOWN", Brushes.Gray},
        };

        private readonly Dictionary<string, Brush> _statusTextBrushes = new Dictionary<string, Brush>
        {
            {"SUCCESS", Brushes.White},
            {"FAILURE", Brushes.White},
            {"UNKNOWN", Brushes.White},
        };

        private readonly TileConfiguration _tileConfiguration;
        private readonly TimeSpan _updatePeriod = TimeSpan.FromSeconds(30);
        private TeamCityBuildStatusTileControl _view;
        private readonly TimerToken _timerToken;

        public TeamCityLastBuildStatusTileViewModel(TeamCityService service, DashboardTileConfiguration tileConfiguration, ITimerService timerService, IDashboardController dashboardController)
        {
            _service = service;
            _dashboardController = dashboardController;
            _tileConfiguration = new TileConfiguration(tileConfiguration, this);
            Id = tileConfiguration.Id;
            ConfigureService = new TeamCityServiceConfigureCommand(service);
            _timerToken = timerService.QueueCallback(TimeSpan.FromDays(10000), this);
        }

        public ICommand ConfigureService { get; }

        public ICommand ConfigureCommand { get; private set; }

        public void Start(Panel placeholderPanel)
        {
            ConfigureCommand = new TileConfigureCommand("Last Build Status Configuration", _tileConfiguration,
                new IConfigurationView[]
                {
                    new ConfigurationParameter("Project", "", _tileConfiguration),
                    new ConfigurationParameter("Configuration", "", _tileConfiguration),
                    new ConfigurationParameter("Description", "", _tileConfiguration),
                    new ConfigurationHyperlink("TeamCity service", ConfigureService)
                },
                _dashboardController);

            _view = new TeamCityBuildStatusTileControl();
            placeholderPanel.Children.Add(_view);
            _view.DataContext = this;

            _service.Connect();

            _timerToken.Requeue(TimeSpan.FromSeconds(1));
        }

        public string TypeId => TileTypeId;

        public Guid Id { get; private set; }

        public void OnConfigurationChanged()
        {
            _timerToken.Requeue(TimeSpan.FromMilliseconds(100));
        }

        public void OnTimeElapsed(TimerToken token)
        {
            var projectName = _tileConfiguration.GetString("Project");
            var configurationName = _tileConfiguration.GetString("Configuration");

            var build = _service.GetRunningBuild(projectName, configurationName);
            var running = build != null;

            if (build == null)
            {
                build = _service.GetLastBuild(projectName, configurationName);
            }

            string status;

            if (build == null)
            {
                status = "UNKNOWN";
                _view.statusText.Text = "ERROR";
                _view.buildVer.Text = "";
                _view.headerText.Text = "";
            }
            else
            {
                status = build.Status;
                if (!_statusBrushes.ContainsKey(status))
                {
                    status = "UNKNOWN";
                }
                _view.statusText.Text = _tileConfiguration.GetString("Description");
                _view.buildVer.Text = build.Number;
                _view.headerText.Text = running ? "Running" : "";
            }

            _view.root.Background = running ? _runningStatusBrushes[status] : _statusBrushes[status];
            SetTextForeground(running, status);

            _timerToken.Requeue(_updatePeriod);
        }

        private void SetTextForeground(bool running, string status)
        {
            var textBrush = running ? _runningStatusTextBrushes[status] : _statusTextBrushes[status];
            _view.headerText.Foreground = textBrush;
            _view.statusText.Foreground = textBrush;
            _view.buildVer.Foreground = textBrush;
        }
    }
}