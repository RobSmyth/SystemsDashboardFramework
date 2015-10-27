using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Dashboard.Tiles.TeamCityAvailableBuilds;
using NoeticTools.Dashboard.Framework;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Config.Commands;
using NoeticTools.Dashboard.Framework.Config.Parameters;
using NoeticTools.Dashboard.Framework.DataSources.TeamCity;
using NoeticTools.Dashboard.Framework.Time;
using TeamCitySharp.DomainEntities;

namespace NoeticTools.TeamDashboard.Tiles.TeamCityAvailableBuilds
{
    internal class TeamCityAvailableBuildsViewModel : ITileViewModel, ITimerListener
    {
        private const int MaxNumberOfBuilds = 8;
        public static readonly string TileTypeId = "TeamCity.AvailableBuilds";
        private readonly TeamCityService _service;
        private readonly ITimerService _timerService;
        private readonly TimeSpan _tickPeriod = TimeSpan.FromSeconds(15);
        private readonly TileConfiguration _tileConfiguration;
        private TeamCityAvailableBuildsListControl _view;

        public TeamCityAvailableBuildsViewModel(TeamCityService service, DashboardTileConfiguration tileConfiguration, ITimerService timerService)
        {
            _service = service;
            _timerService = timerService;
            _tileConfiguration = new TileConfiguration(tileConfiguration, this);
            Id = tileConfiguration.Id;
            Builds = new ObservableCollection<BuildDetails>();
        }

        public ICommand ConfigureCommand { get; private set; }
        public ObservableCollection<BuildDetails> Builds { get; set; }

        public void Start(Panel placeholderPanel)
        {
            var parameters = new List<ConfigurationParameter>
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
                parameters.ToArray());

            _service.Connect();

            _view = new TeamCityAvailableBuildsListControl {DataContext = this};
            placeholderPanel.Children.Add(_view);

            _timerService.QueueCallback(TimeSpan.FromSeconds(1), this);
        }

        public string TypeId => TileTypeId;

        public Guid Id { get; private set; }

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
                string displayName = _tileConfiguration.GetString(string.Format("Display_name_{0}", buildNumber));
                string projectName = _tileConfiguration.GetString(string.Format("Project_{0}", buildNumber));
                string configurationName = _tileConfiguration.GetString(string.Format("Configuration_{0}", buildNumber));

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

        private string CleanupVersion(string version)
        {
            var length = version.IndexOf(".Rel");
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