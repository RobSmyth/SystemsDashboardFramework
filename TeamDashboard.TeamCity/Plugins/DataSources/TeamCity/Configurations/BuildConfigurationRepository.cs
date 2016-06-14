using System;
using System.Collections.Generic;
using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Channel;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Projects;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Configurations;
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
        private IDictionary<string, IBuildConfiguration> _configurations = new Dictionary<string, IBuildConfiguration>();

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
            _timerToken = _services.Timer.QueueCallback(TimeSpan.FromMinutes(10), this);
        }

        public IBuildConfiguration[] GetAll()
        {
            try
            {
                return _teamCityClient.BuildConfigs.ByProjectId(_project.Id).Select(x => new BuildConfiguration(x, _project, _teamCityClient)).ToArray();
            }
            catch (Exception)
            {
                // todo - log exception
                return new IBuildConfiguration[0];
            }
        }
    }
}
