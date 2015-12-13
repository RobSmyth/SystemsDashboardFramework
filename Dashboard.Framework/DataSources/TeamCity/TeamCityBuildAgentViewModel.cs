using System;
using System.Linq;
using System.Threading.Tasks;
using NoeticTools.SystemsDashboard.Framework.Time;
using TeamCitySharp;
using TeamCitySharp.Locators;


namespace NoeticTools.SystemsDashboard.Framework.DataSources.TeamCity
{
    public class TeamCityBuildAgentViewModel : NotifyingViewModelBase, IBuildAgent, ITimerListener
    {
        private readonly TimeSpan _tickPeriod = TimeSpan.FromSeconds(30);
        private readonly TeamCityClient _teamCityClient;
        private readonly TimerToken _timerToken;
        private BuildAgentStatus _status;
        private bool _isRunning;

        public TeamCityBuildAgentViewModel(string name, TeamCityClient teamCityClient, ITimerService timer)
        {
            _teamCityClient = teamCityClient;
            Name = name;
            _timerToken = timer.QueueCallback(TimeSpan.FromMilliseconds(10), this);
        }

        public string Name { get; }

        public bool IsRunning
        {
            get { return _isRunning; }
            private set
            {
                if (_isRunning != value)
                {
                    _isRunning = value;
                    OnPropertyChanged();
                }
            }
        }

        public BuildAgentStatus Status
        {
            get { return _status; }
            private set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            Update();
        }

        private void Update()
        {
            Task.Run(() => { return _teamCityClient.Builds.ByBuildLocator(BuildLocator.WithDimensions(running: true)).Where(x => Name.Equals(x.Agent.Name, StringComparison.InvariantCultureIgnoreCase)).ToArray(); })
                .ContinueWith(x =>
                {
                    IsRunning = x.Result.Length == 1;
                    Status = IsRunning ? BuildAgentStatus.Running : BuildAgentStatus.Idle;
                    _timerToken.Requeue(_tickPeriod);
                });
        }
    }
}