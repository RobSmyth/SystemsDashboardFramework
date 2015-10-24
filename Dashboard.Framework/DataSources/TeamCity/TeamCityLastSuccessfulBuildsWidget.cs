using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamCitySharp;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Media;


namespace Dashboard.TeamCity
{
    class TeamCityLastSuccessfulBuildsWidget
    {
        private TeamCityChannel _channel;
        private TeamCityBuildStatusControl _view;
        private DispatcherTimer _timer;
        private Dictionary<string, Brush> _statusBrushes = new Dictionary<string, Brush>
        {
            {"SUCCESS", Brushes.Green },
            {"UNKNOWN", Brushes.Gray },
        };
        private string _projectName;
        private string _buildConfiguration;

        public TeamCityLastSuccessfulBuildsWidget(string projectName, string buildConfiguration, TeamCityChannel channel)
        {
            _channel = channel;
            _projectName = projectName;
            _buildConfiguration = buildConfiguration;

            _timer = new DispatcherTimer();
        }

        public void Start(Panel placeholderPanel)
        {
            _channel.Connect();

            _view = new TeamCityBuildStatusControl();
            placeholderPanel.Children.Add(_view);

            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();

            var build = _channel.GetLastBuild(_projectName, _buildConfiguration);

            var status = build.Status;
            if (!_statusBrushes.ContainsKey(status))
            {
                status = "UNKNOWN";
            }
            _view.root.Background = _statusBrushes[status];
            _view.statusText.Text = build.Status;
            _view.projectName.Text = _projectName;
            _view.buildName.Text = _buildConfiguration;
            //_view.buildName.Text = string.Format("{0}\n{1}", _buildConfiguration, _branchName);
            _view.buildVer.Text = build.Number;

            _timer.Interval = TimeSpan.FromSeconds(15);
            _timer.Start();
        }


    }
}
