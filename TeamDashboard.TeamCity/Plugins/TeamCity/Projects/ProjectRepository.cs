using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using NoeticTools.TeamStatusBoard.Framework.DataSources.Jira;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.TcSharpInterop;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Projects
{
    public sealed class ProjectRepository : IChannelConnectionStateListener, ITimerListener
    {
        private readonly TimeSpan _updatePeriod = TimeSpan.FromMinutes(1);
        private readonly IDataSource _outerRepository;
        private TcSharpTeamCityClient _teamCityClient;
        private IServices _services;
        private IChannelConnectionStateBroadcaster _channelStateBroadcaster;
        private Action _onConnected = () => { };
        private Action _onDisconnected = () => { };
        private ITimerToken _timerToken = new NullTimerToken();
        private readonly ILog _logger;
        private readonly object _syncRoot = new object();
        private readonly Dictionary<Project, TimeCachedArray<BuildConfig>> _buildConfigurations = new Dictionary<Project, TimeCachedArray<BuildConfig>>();

        public ProjectRepository(IDataSource outerRepository, TcSharpTeamCityClient teamCityClient, IServices services, IChannelConnectionStateBroadcaster channelStateBroadcaster)
        {
            _outerRepository = outerRepository;
            _teamCityClient = teamCityClient;
            _services = services;
            _channelStateBroadcaster = channelStateBroadcaster;
            _channelStateBroadcaster.Add(this);
            _outerRepository.Write($"Agents.Count", 0);
            _logger = LogManager.GetLogger("Repositories.Projects");
            SetDisconnectedState();
        }

        public Project[] GetRunningBuilds()
        {
            return new Project[0];
        }

        public async Task<Build[]> GetRunningBuilds(string projectName, string buildConfigurationName)
        {
            _logger.DebugFormat("Request for running build: {0} / {1}.", projectName, buildConfigurationName);

            try
            {
                var project = _teamCityClient.Projects.ByName(projectName);
                if (project == null)
                {
                    _logger.WarnFormat("Could not find project {0}.", projectName);
                    return new Build[0];
                }

                var buildConfiguration = await GetConfiguration(project, buildConfigurationName);
                if (buildConfiguration == null)
                {
                    _logger.WarnFormat("Could not find configuration: {0} / {1}.", projectName, buildConfigurationName);
                    return new Build[0];
                }

                var builds = _teamCityClient.Builds.ByBuildLocator(BuildLocator.WithDimensions(running: true, branch: "default:any")).Where(x => x.WebUrl.EndsWith(buildConfiguration.Id)).ToArray();

                foreach (var build in builds)
                {
                    build.Status = build.Status == "FAILED" ? "RUNNING FAILED" : "RUNNING";
                }

                return builds;
            }
            catch (Exception exception)
            {
                _logger.Error("Exception while getting running build.", exception);
                return new Build[0];
            }
        }

        private async Task<BuildConfig> GetConfiguration(Project project, string buildConfigurationName)
        {
            var buildConfigurations = await GetConfigurations(project);
            return buildConfigurations.Items.SingleOrDefault(x => x.Name.Equals(buildConfigurationName, StringComparison.InvariantCultureIgnoreCase));
        }

        private Task<TimeCachedArray<BuildConfig>> GetConfigurations(Project project)
        {
            _logger.DebugFormat("Request for configurations on project {0}.", project.Name);

            return Task.Run(() =>
            {
                lock (_syncRoot)
                {
                    if (!_buildConfigurations.ContainsKey(project))
                    {
                        var timeCachedArray = new TimeCachedArray<BuildConfig>(() => _teamCityClient.BuildConfigs.ByProjectId(project.Id), TimeSpan.FromHours(12), _services.Clock);
                        _buildConfigurations.Add(project, timeCachedArray);
                    }
                    return _buildConfigurations[project];
                }
            });
        }

        private void Update()
        {
            // todo - get running builds
        }

        void IChannelConnectionStateListener.OnConnected()
        {
            var action = _onConnected;
            SetConnectedState();
            action();
        }

        void IChannelConnectionStateListener.OnDisconnected()
        {
            var action = _onDisconnected;
            SetDisconnectedState();
            action();
        }

        private void SetDisconnectedState()
        {
            _onDisconnected = () => { };
            _onConnected = () =>
            {
                _timerToken.Cancel();
                _timerToken = _services.Timer.QueueCallback(TimeSpan.FromMilliseconds(10), this);
            };
        }

        private void SetConnectedState()
        {
            _onDisconnected = () =>
            {
                var token = _timerToken;
                _timerToken = new NullTimerToken();
                token.Cancel();
            };
            _onConnected = () => { };
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            Update();
            _timerToken = _services.Timer.QueueCallback(_updatePeriod, this);
        }
    }
}
