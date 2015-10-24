using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Dashboard.TeamCity;

namespace Dashboard.Tiles.TeamCity
{
    class TeamCityLastBuildStatusWidget
    {
        private TimeSpan _updatePeriod = TimeSpan.FromSeconds(30);
        private readonly TeamCityChannel _channel;
        private TeamCityBuildStatusControl _view;
        private readonly DispatcherTimer _timer;
        private readonly Dictionary<string, Brush> _statusBrushes = new Dictionary<string, Brush>
        {
            {"SUCCESS", (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF448032")) },
            {"RUNNING", Brushes.OliveDrab },// not sure if this exists
            {"FAILED", Brushes.Red },
            {"UNKNOWN", Brushes.Gray },
        };
        private readonly string _projectName;
        private readonly string _buildConfiguration;
        private readonly string _description;

        public TeamCityLastBuildStatusWidget(TeamCityChannel channel, string projectName, string buildConfiguration, string description)
        {
            _channel = channel;
            _projectName = projectName;
            _buildConfiguration = buildConfiguration;
            _description = description;

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


            var build = _channel.GetRunningBuild(_projectName, _buildConfiguration) ??
                        _channel.GetLastBuild(_projectName, _buildConfiguration);

            if (build != null)
            {
                var status = build.Status;
                if (!_statusBrushes.ContainsKey(status))
                {
                    status = "UNKNOWN";
                }
                _view.root.Background = _statusBrushes[status];
                _view.statusText.Text = _description;
                _view.buildVer.Text = build.Number;
            }

            _timer.Interval = _updatePeriod;
            _timer.Start();
        }


    }
}
