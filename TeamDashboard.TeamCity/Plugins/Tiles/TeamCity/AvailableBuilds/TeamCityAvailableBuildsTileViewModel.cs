﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using log4net;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.TeamCity.AvailableBuilds;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Channel;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.Tiles.TeamCity.AvailableBuilds
{
    internal sealed class TeamCityAvailableBuildsTileViewModel : ITimerListener, IConfigurationChangeListener, ITileViewModel
    {
        public class BuildDetails
        {
            public string BuildConfiguration { get; set; }
            public string Version { get; set; }
            public ImageSource ImageSource { get; set; }
            public int DaysOld { get; set; }
        }

        private const int MaxNumberOfBuilds = 8;
        public const string TileTypeId = "TeamCity.AvailableBuilds";
        private readonly ITeamCityChannel _channel;
        private readonly TimeSpan _tickPeriod = TimeSpan.FromSeconds(15);
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private readonly IServices _services;
        private readonly ILog _logger;
        private readonly TeamCityAvailableBuildsListControl _view;
        private static int _nextInstanceId = 1;

        public TeamCityAvailableBuildsTileViewModel(ITeamCityChannel channel, TileConfiguration tile, IDashboardController dashboardController, 
            ITileLayoutController layoutController, IServices services, TeamCityAvailableBuildsListControl view)
        {
            _channel = channel;
            _services = services;
            _view = view;
            _tileConfigurationConverter = new TileConfigurationConverter(tile, this);
            Builds = new ObservableCollection<BuildDetails>();
            _logger = LogManager.GetLogger($"Tiles.TeamCity.AvailableBuilds.{_nextInstanceId++}");
            var parameters = GetConfigurtionParameters();
            ConfigureCommand = new TileConfigureCommand(tile, "TeamCity Available Builds Tile", parameters, dashboardController, layoutController, _services);
            _view.DataContext = this;
            _view.buildsList.ItemsSource = Builds;
            _services.Timer.QueueCallback(TimeSpan.FromSeconds(_channel.IsConnected ? 1 : 4), this);
        }

        public ICommand ConfigureCommand { get; }
        public ObservableCollection<BuildDetails> Builds { get; }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
            _logger.Info("Configuration changed.");

            if (_view == null)
            {
                return;
            }

            _view.Dispatcher.InvokeAsync(() => _view.projectName.Text = _tileConfigurationConverter.GetString("Title"));
            UpdateView();
        }

        public void OnTimeElapsed(TimerToken token)
        {
            UpdateView();
        }

        private IPropertyViewModel[] GetConfigurtionParameters()
        {
            var parameters = new List<IPropertyViewModel>
            {
                new PropertyViewModel("Title", "Text", _tileConfigurationConverter),
                new DividerPropertyViewModel()
            };
            for (var buildNumber = 1; buildNumber <= MaxNumberOfBuilds; buildNumber++)
            {
                string diplayName = $"Display_name_{buildNumber}";
                string project = $"Project_{buildNumber}";
                string configuration = $"Configuration_{buildNumber}";
                parameters.Add(new PropertyViewModel(diplayName, "Text", _tileConfigurationConverter));
                parameters.Add(new PropertyViewModel(project, "Text", _tileConfigurationConverter));
                parameters.Add(new PropertyViewModel(configuration, "Text", _tileConfigurationConverter));
                parameters.Add(new DividerPropertyViewModel());
            }
            return parameters.ToArray();
        }

        private static string CleanupVersion(string version)
        {
            var length = version.IndexOf(".Rel", StringComparison.Ordinal);
            if (length <= 0)
            {
                return version;
            }
            return version.Substring(0, length);
        }

        private void UpdateView()
        {
            _logger.Debug("Update UI.");

            var builds = GetCurrentBuilds();
            _view.Dispatcher.InvokeAsync(() =>
            {
                Builds.Clear();
                foreach (var build in builds)
                {
                    Builds.Add(build);
                }
                _services.Timer.QueueCallback(_tickPeriod, this);
            });
        }

        private BuildDetails[] GetCurrentBuilds()
        {
            var builds = new List<BuildDetails>();

            for (var buildNumber = 1; buildNumber <= MaxNumberOfBuilds; buildNumber++)
            {
                var displayName = _tileConfigurationConverter.GetString($"Display_name_{buildNumber}");
                var projectName = _tileConfigurationConverter.GetString($"Project_{buildNumber}");
                var configurationName = _tileConfigurationConverter.GetString($"Configuration_{buildNumber}");

                if (!string.IsNullOrWhiteSpace(displayName) &&
                    !displayName.Equals("EMPTY", StringComparison.InvariantCultureIgnoreCase))
                {
                    var build = _channel.Projects.Get(projectName).GetConfiguration(configurationName).GetLastSuccessfulBuild();
                    if (build != null)
                    {
                        var ageInDays = (DateTime.Now - build.StartDate).Days;
                        var imageSource = ageInDays < 2
                            ? new BitmapImage(
                                new Uri("pack://application:,,,/NoeticTools.TeamStatusBoard.Framework;component/Images/YellowStar_32x32.png"))
                            : ageInDays < 7
                                ? new BitmapImage(
                                    new Uri("pack://application:,,,/NoeticTools.TeamStatusBoard.Framework;component/Images/WhiteStar_20x20.png"))
                                : ageInDays < 14
                                    ? new BitmapImage(
                                        new Uri(
                                            "pack://application:,,,/NoeticTools.TeamStatusBoard.Framework;component/Images/TransparentStar_20x20.png"))
                                    : new BitmapImage();

                        builds.Add(new BuildDetails
                        {
                            BuildConfiguration = displayName,
                            Version = CleanupVersion(build.Number),
                            ImageSource = imageSource,
                            DaysOld = ageInDays
                        });
                    }
                }
            }

            return builds.ToArray();
        }
    }
}