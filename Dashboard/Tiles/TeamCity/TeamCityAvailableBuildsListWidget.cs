using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Dashboard.TeamCity;

namespace Dashboard.Tiles.TeamCity
{
    internal class TeamCityAvailableBuildsListWidget
    {
        private readonly TeamCityChannel _channel;
        private TeamCityAvailableBuildsListControl _view;
        private readonly DispatcherTimer _timer;
        private readonly string _projectName;
        private readonly string[] _buildConfigurations;

        public TeamCityAvailableBuildsListWidget(TeamCityChannel channel, string projectName,
            params string[] buildConfigurations)
        {
            _channel = channel;
            _projectName = projectName;
            _buildConfigurations = buildConfigurations;
            Builds = new ObservableCollection<BuildDetails>();

            _timer = new DispatcherTimer();
        }

        public void Start(Panel placeholderPanel)
        {
            _channel.Connect();

            _view = new TeamCityAvailableBuildsListControl();
            placeholderPanel.Children.Add(_view);

            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        public ObservableCollection<BuildDetails> Builds { get; set; }

        private void _timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();

            if (_view.buildsList.ItemsSource == null)
            {
                _view.buildsList.ItemsSource = Builds;
            }

            _view.projectName.Text = string.Format("Available builds");
            Builds.Clear();

            foreach (var buildConfiguration in _buildConfigurations)
            {
                var build = _channel.GetLastSuccessfulBuild(_projectName, buildConfiguration);
                if (build != null)
                {
                    var ageInDays = (DateTime.Now - build.StartDate).Days;
                    var imageSource = ageInDays < 2
                        ? new BitmapImage(new Uri("pack://application:,,,/Dashboard;component/Images/YellowStar_32x32.png"))
                        : ageInDays < 7
                        ? new BitmapImage(new Uri("pack://application:,,,/Dashboard;component/Images/WhiteStar_20x20.png"))
                        : ageInDays < 21
                        ? new BitmapImage(new Uri("pack://application:,,,/Dashboard;component/Images/TransparentStar_20x20.png"))
                        : new BitmapImage();

                    Builds.Add(new BuildDetails()
                    {
                        BuildConfiguration = buildConfiguration,
                        Version = build.Number,
                        ImageSource = imageSource,
                        DaysOld = ageInDays,
                    });
                }
            }

            _timer.Interval = TimeSpan.FromSeconds(15);
            _timer.Start();
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
