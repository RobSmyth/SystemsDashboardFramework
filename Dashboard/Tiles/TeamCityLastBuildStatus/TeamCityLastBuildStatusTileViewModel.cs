using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Dashboard.Config.Parameters;
using Dashboard.Tiles.TeamCityLastBuildStatus;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Config.Commands;
using NoeticTools.Dashboard.Framework.Config.Parameters;
using NoeticTools.Dashboard.Framework.DataSources.TeamCity;

namespace NoeticTools.TeamDashboard.Tiles.TeamCityLastBuildStatus
{
    internal class TeamCityLastBuildStatusTileViewModel : ITileViewModel
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
        private readonly DispatcherTimer _timer;
        private readonly TimeSpan _updatePeriod = TimeSpan.FromSeconds(30);
        private TeamCityBuildStatusTileControl _view;

        public TeamCityLastBuildStatusTileViewModel(TeamCityService service,
            DashboardTileConfiguration tileConfiguration)
        {
            _service = service;
            _tileConfiguration = new TileConfiguration(tileConfiguration, this);
            ;
            Id = tileConfiguration.Id;
            _timer = new DispatcherTimer();
            ConfigureService = new TeamCityServiceConfigureCommand(service);
        }

        public ICommand ConfigureService { get; private set; }

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
                });

            _view = new TeamCityBuildStatusTileControl();
            placeholderPanel.Children.Add(_view);
            _view.DataContext = this;

            _service.Connect();

            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        public string TypeId => TileTypeId;

        public Guid Id { get; private set; }

        public void OnConfigurationChanged()
        {
            _timer.Stop();
            _timer.Interval = TimeSpan.FromMilliseconds(100);
            _timer.Start();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();

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

            _timer.Interval = _updatePeriod;
            _timer.Start();
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