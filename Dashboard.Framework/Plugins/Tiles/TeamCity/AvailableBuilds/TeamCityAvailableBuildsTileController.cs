using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NoeticTools.Dashboard.Framework.Commands;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Config.Parameters;
using NoeticTools.Dashboard.Framework.DataSources.TeamCity;
using NoeticTools.Dashboard.Framework.Tiles;
using NoeticTools.Dashboard.Framework.Tiles.TeamCityAvailableBuilds;
using NoeticTools.Dashboard.Framework.Time;


namespace NoeticTools.Dashboard.Framework.Plugins.Tiles.TeamCity.AvailableBuilds
{
    internal class TeamCityAvailableBuildsTileController : IViewController, ITimerListener
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
        private readonly TeamCityService _service;
        private readonly TileConfiguration _tile;
        private readonly ITimerService _timerService;
        private readonly IDashboardController _dashboardController;
        private readonly TileLayoutController _tileLayoutController;
        private readonly TimeSpan _tickPeriod = TimeSpan.FromSeconds(15);
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private TeamCityAvailableBuildsListControl _view;

        public TeamCityAvailableBuildsTileController(TeamCityService service, TileConfiguration tile, ITimerService timerService, IDashboardController dashboardController, TileLayoutController tileLayoutController)
        {
            _service = service;
            _tile = tile;
            _timerService = timerService;
            _dashboardController = dashboardController;
            _tileLayoutController = tileLayoutController;
            _tileConfigurationConverter = new TileConfigurationConverter(tile, this);
            Builds = new ObservableCollection<BuildDetails>();
        }

        public ICommand ConfigureCommand { get; private set; }
        public ObservableCollection<BuildDetails> Builds { get; }

        public FrameworkElement CreateView()
        {
            var parameters = new List<IElementViewModel>
            {
                new ElementViewModel("Title", ElementType.Text, _tileConfigurationConverter),
                new DividerElementViewModel()
            };
            for (var buildNumber = 1; buildNumber <= MaxNumberOfBuilds; buildNumber++)
            {
                string diplayName = $"Display_name_{buildNumber}";
                string project = $"Project_{buildNumber}";
                string configuration = $"Configuration_{buildNumber}";
                parameters.Add(new ElementViewModel(diplayName, ElementType.Text, _tileConfigurationConverter));
                parameters.Add(new ElementViewModel(project, ElementType.Text, _tileConfigurationConverter));
                parameters.Add(new ElementViewModel(configuration, ElementType.Text, _tileConfigurationConverter));
                parameters.Add(new DividerElementViewModel());
            }

            ConfigureCommand = new TileConfigureCommand(_tile, "TeamCity Available Builds Tile", parameters.ToArray(), _dashboardController, _tileLayoutController);

            _service.Connect();

            _view = new TeamCityAvailableBuildsListControl {DataContext = this};

            _timerService.QueueCallback(TimeSpan.FromSeconds(1), this);
            return _view;
        }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
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

        private static string CleanupVersion(string version)
        {
            var length = version.IndexOf(".Rel", StringComparison.Ordinal);
            if (length <= 0)
            {
                return version;
            }
            return version.Substring(0, length);
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

            _view.projectName.Text = _tileConfigurationConverter.GetString("Title");
            Builds.Clear();

            for (var buildNumber = 1; buildNumber <= MaxNumberOfBuilds; buildNumber++)
            {
                var displayName = _tileConfigurationConverter.GetString($"Display_name_{buildNumber}");
                var projectName = _tileConfigurationConverter.GetString($"Project_{buildNumber}");
                var configurationName = _tileConfigurationConverter.GetString($"Configuration_{buildNumber}");

                if (!string.IsNullOrWhiteSpace(displayName) &&
                    !displayName.Equals("EMPTY", StringComparison.InvariantCultureIgnoreCase))
                {
                    var build = _service.GetLastSuccessfulBuild(projectName, configurationName);
                    if (build != null)
                    {
                        var ageInDays = (DateTime.Now - build.StartDate).Days;
                        var imageSource = ageInDays < 2
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
                            DaysOld = ageInDays
                        });
                    }
                }
            }
        }
    }
}