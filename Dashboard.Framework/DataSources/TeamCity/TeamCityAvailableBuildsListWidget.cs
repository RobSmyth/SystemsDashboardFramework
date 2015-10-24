using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Media;


namespace Dashboard.TeamCity
{
    class TeamCityAvailableBuildsListWidget
    {
       private TeamCityChannel _channel;
       private TeamCityAvailableBuildsListControl _view;
        private DispatcherTimer _timer;
        private string _projectName;
        private string[] _buildConfigurations;

        public TeamCityAvailableBuildsListWidget(TeamCityChannel channel, string projectName, params string[] buildConfigurations)
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

        void _timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();

            if (_view.buildsList.ItemsSource == null)
            {
                _view.buildsList.ItemsSource = Builds;
            }

            _view.projectName.Text = _projectName;
            Builds.Clear();

            foreach (var buildConfiguration in _buildConfigurations)
            {
                var build = _channel.GetLastSuccessfulBuild(_projectName, buildConfiguration);
                if (build != null)
                {
                    Builds.Add(new BuildDetails() { BuildConfiguration = buildConfiguration, Version = build.Number });
                }
            }

            _timer.Interval = TimeSpan.FromSeconds(15);
            _timer.Start();
        }

        public class BuildDetails
        {
            public string BuildConfiguration { get; set; }
            public string Version { get; set; }
        }
    }
}
