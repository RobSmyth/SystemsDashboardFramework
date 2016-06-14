using System;
using System.Collections.Generic;
using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Channel;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Projects;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.TcSharpInterop;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.TcSharpInterop;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Configurations
{
    public sealed class BuildConfigurationRepository : IChannelConnectionStateListener, ITimerListener, IBuildConfigurationRepository
    {
        private readonly ITcSharpTeamCityClient _teamCityClient;
        private readonly IServices _services;
        private readonly IProject _project;
        private ITimerToken _timerToken = new NullTimerToken();
        private Action _onConnected = () => { };
        private Action _onDisconnected = () => { };
        private readonly IDictionary<string, IBuildConfiguration> _configurations = new Dictionary<string, IBuildConfiguration>();
        private object _syncRoot = new object();

        public BuildConfigurationRepository(ITcSharpTeamCityClient teamCityClient, IServices services, IChannelConnectionStateBroadcaster channelStateBroadcaster, IProject project)
        {
            _teamCityClient = teamCityClient;
            _services = services;
            _project = project;
            EnterDisconnectedState();
            channelStateBroadcaster.Add(this);
        }

        void IChannelConnectionStateListener.OnConnected()
        {
            var action = _onConnected;
            EnterConnectedState();
            action();
        }

        void IChannelConnectionStateListener.OnDisconnected()
        {
            var action = _onDisconnected;
            EnterDisconnectedState();
            action();
        }

        private void EnterConnectedState()
        {
            _onConnected = () => { };
            _onDisconnected = () => { };
            {
                _timerToken.Cancel();
            };
        }

        private void EnterDisconnectedState()
        {
            _onConnected = () =>
            {
                _timerToken.Cancel();
                _timerToken = _services.Timer.QueueCallback(TimeSpan.FromMilliseconds(10), this);
            };
            _onDisconnected = () => { };
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            try
            {
                lock (_syncRoot)
                {
                    var tcsConfigurations = _teamCityClient.BuildConfigs.ByProjectId(_project.Id).ToArray();
                    var found = new List<IBuildConfiguration>();
                    foreach (var tcsConfiguration in tcsConfigurations)
                    {
                        var normalisedName = tcsConfiguration.Name.ToLower();
                        if (!_configurations.ContainsKey(normalisedName))
                        {
                            _configurations.Add(normalisedName, new BuildConfiguration(tcsConfiguration, _project, _teamCityClient));
                        }
                        else
                        {
                            _configurations[normalisedName].Update(tcsConfiguration);
                        }
                        found.Add(_configurations[normalisedName]);
                    }

                    foreach (var orphan in _configurations.Values.Except(found).ToArray())
                    {
                        orphan.Update(new NullInteropBuildConfig(orphan.Name));
                    }
                }
            }
            catch (Exception)
            {
                // todo - log exception
            }

            _timerToken = _services.Timer.QueueCallback(TimeSpan.FromMinutes(10), this);
        }

        public IBuildConfiguration[] GetAll()
        {
            return _configurations.Values.ToArray();
        }
    }
}
