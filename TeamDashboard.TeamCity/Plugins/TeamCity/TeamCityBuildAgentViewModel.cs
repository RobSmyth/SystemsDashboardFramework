using System;
using System.Linq;
using System.Threading.Tasks;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Agents;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.TcSharpInterop;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity
{
    public class TeamCityBuildAgentViewModel : NotifyingViewModelBase, IBuildAgent, ITimerListener
    {
        private readonly TimeSpan _tickPeriod = TimeSpan.FromSeconds(30);
        private readonly ITcSharpTeamCityClient _teamCityClient;
        private readonly ITimerService _timer;
        private readonly IDataSource _outerRepository;
        private TimerToken _timerToken;
        private BuildAgentStatus _status;
        private bool _isRunning;
        private string _statusText;

        public TeamCityBuildAgentViewModel(string name, ITcSharpTeamCityClient teamCityClient, ITimerService timer, IDataSource outerRepository, IChannelConnectionStateBroadcaster channelStateBroadcaster)
        {
            _teamCityClient = teamCityClient;
            _timer = timer;
            _outerRepository = outerRepository;
            Name = name;
            _statusText = string.Empty;
            channelStateBroadcaster.OnDisconnected.AddListener(OnDisconnected);
            channelStateBroadcaster.OnConnected.AddListener(OnConnected);
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

        public string StatusText
        {
            get { return _statusText; }
            private set
            {
                if (!_statusText.Equals(value, StringComparison.InvariantCulture))
                {
                    _statusText = value;
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
            Task.Run(() =>
            {
                try
                {
                    var runningBuilds = _teamCityClient.Builds.ByBuildLocator(BuildLocator.WithDimensions(running: true));
                    return runningBuilds.ToArray();
                }
                catch (Exception)
                {
                    StatusText = "Error";
                    return new Build[0];
                }
            })
                .ContinueWith(x =>
                {
                    var buildsUsingAgent = x.Result.Where(y => Name.Equals(y.Agent?.Name, StringComparison.InvariantCultureIgnoreCase)).ToArray();
                    if (buildsUsingAgent.Length == 0 && x.Result.Any(y => y.Agent == null))
                    {
                        // TeamCity 7 does not give agent
                        IsRunning = false;
                        Status = BuildAgentStatus.Unknown;
                        StatusText = "Unknown";
                    }
                    else
                    {
                        IsRunning = buildsUsingAgent.Length > 0;
                        Status = IsRunning ? BuildAgentStatus.Running : BuildAgentStatus.Idle;
                        StatusText = IsRunning ? "Running" : "Idle";
                    }
                    UpdateBuildAgentParameters();
                    _timerToken.Requeue(_tickPeriod);
                });
        }

        private void UpdateBuildAgentParameters()
        {
            _outerRepository.Write($"Agent.{Name}.Status", Status);
        }

        private void OnConnected()
        {
            _timerToken = _timer.QueueCallback(TimeSpan.FromMilliseconds(10), this);
        }

        private void OnDisconnected()
        {
            _timerToken.Cancel();
            _timerToken = null;
        }
    }
}