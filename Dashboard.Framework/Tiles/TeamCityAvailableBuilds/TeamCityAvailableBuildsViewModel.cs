using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Config.Commands;
using NoeticTools.Dashboard.Framework.Config.Parameters;
using NoeticTools.Dashboard.Framework.DataSources.TeamCity;
using NoeticTools.Dashboard.Framework.Time;
using TeamCitySharp.DomainEntities;

namespace NoeticTools.Dashboard.Framework.Tiles.TeamCityAvailableBuilds
{
    internal class TeamCityAvailableBuildsViewModel : ITileViewModel, ITimerListener
    {
        private const int MaxNumberOfBuilds = 8;
        public static readonly string TileTypeId = "TeamCity.AvailableBuilds";
        private readonly TeamCityService _service;
        private readonly ITimerService _timerService;
        private readonly IDashboardController _dashboardController;
        private readonly TimeSpan _tickPeriod = TimeSpan.FromSeconds(15);
        private readonly TileConfiguration _tileConfiguration;
        private TeamCityAvailableBuildsListControl _view;

        public TeamCityAvailableBuildsViewModel(TeamCityService service, DashboardTileConfiguration tileConfiguration, ITimerService timerService, IDashboardController dashboardController)
        {
            _service = service;
            _timerService = timerService;
            _dashboardController = dashboardController;
            _tileConfiguration = new TileConfiguration(tileConfiguration, this);
            Builds = new ObservableCollection<BuildDetails>();
        }

        public ICommand ConfigureCommand { get; private set; }
        public ObservableCollection<BuildDetails> Builds { get; private set; }

        public FrameworkElement CreateView()
        {
            var parameters = new List<IConfigurationView>
            {
                new ConfigurationParameter("Title", "EMPTY", _tileConfiguration)
            };
            for (int buildNumber = 1; buildNumber <= MaxNumberOfBuilds; buildNumber++)
            {
                string diplayName = $"Display_name_{buildNumber}";
                string project = $"Project_{buildNumber}";
                string configuration = $"Configuration_{buildNumber}";
                parameters.Add(new ConfigurationParameter(diplayName, "EMPTY", _tileConfiguration));
                parameters.Add(new ConfigurationParameter(project, "EMPTY", _tileConfiguration));
                parameters.Add(new ConfigurationParameter(configuration, "EMPTY", _tileConfiguration));
            }

            ConfigureCommand = new TileConfigureCommand("Message Tile Configuration", _tileConfiguration,
                parameters.ToArray(), _dashboardController);

            _service.Connect();

            _view = new TeamCityAvailableBuildsListControl {DataContext = this};

            _timerService.QueueCallback(TimeSpan.FromSeconds(1), this);
            return _view;
        }

        public void OnConfigurationChanged()
        {
            if (_view == null)
            {
                return;
            }

            Tick();
        }


        public void OnTimeElapsed(TimerToken token)
        {
            Tick();
        }

        private void Tick()
        {
            UpdateView();
            _timerService.QueueCallback(_tickPeriod, this);
        }

        private void UpdateView()
        {
            if (_view.buildsList.ItemsSource == null)
            {
                _view.buildsList.ItemsSource = Builds;
            }

            _view.projectName.Text = _tileConfiguration.GetString("Title");
            Builds.Clear();

            for (int buildNumber = 1; buildNumber <= MaxNumberOfBuilds; buildNumber++)
            {
                string displayName = _tileConfiguration.GetString($"Display_name_{buildNumber}");
                string projectName = _tileConfiguration.GetString($"Project_{buildNumber}");
                string configurationName = _tileConfiguration.GetString($"Configuration_{buildNumber}");

                if (!string.IsNullOrWhiteSpace(displayName) &&
                    !displayName.Equals("EMPTY", StringComparison.InvariantCultureIgnoreCase))
                {
                    Build build = _service.GetLastSuccessfulBuild(projectName, configurationName);
                    if (build != null)
                    {
                        int ageInDays = (DateTime.Now - build.StartDate).Days;
                        BitmapImage imageSource = ageInDays < 2
                            ? new BitmapImage(
                                new Uri("pack://application:,,,/Dashboard;component/Images/YellowStar_32x32.png"))
                            : ageInDays < 7
                                ? new BitmapImage(
                                    new Uri("pack://application:,,,/Dashboard;component/Images/WhiteStar_20x20.png"))
                                : ageInDays < 14
                                    ? new BitmapImage(
                                        new Uri(
                                            "pack://application:,,,/Dashboard;component/Images/TransparentStar_20x20.png"))
                                    : new BitmapImage();

                        Builds.Add(new BuildDetails
                        {
                            BuildConfiguration = displayName,
                            Version = CleanupVersion(build.Number),
                            ImageSource = imageSource,
                            DaysOld = ageInDays,
                        });
                    }
                }
            }
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

        public class BuildDetails
        {
            public string BuildConfiguration { get; set; }
            public string Version { get; set; }
            public ImageSource ImageSource { get; set; }
            public int DaysOld { get; set; }
        }
    }
}