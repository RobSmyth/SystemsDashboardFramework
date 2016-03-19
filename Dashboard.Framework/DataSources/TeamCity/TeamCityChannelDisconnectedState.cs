﻿using System;
using System.Threading.Tasks;
using log4net;
using NoeticTools.SystemsDashboard.Framework.Services;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.SystemsDashboard.Framework.DataSources.TeamCity
{
    internal class TeamCityChannelDisconnectedState : ITeamCityChannel
    {
        private readonly TeamCityClient _teamCityClient;
        private readonly TeamCityServiceConfiguration _configuration;
        private readonly IBuildAgentRepository _buildAgentRepository;
        private readonly IServices _services;
        private readonly IStateEngine<ITeamCityChannel> _stateEngine;
        private bool _testingConnection;
        private readonly ILog _logger;

        public TeamCityChannelDisconnectedState(TeamCityClient teamCityClient, IStateEngine<ITeamCityChannel> stateEngine, TeamCityServiceConfiguration configuration, IBuildAgentRepository buildAgentRepository, IServices services)
        {
            _teamCityClient = teamCityClient;
            _stateEngine = stateEngine;
            _configuration = configuration;
            _buildAgentRepository = buildAgentRepository;
            _services = services;

            _logger = LogManager.GetLogger("DateSources.TeamCity.Disconnected");
        }

        public string[] ProjectNames => new string[0];
        public bool IsConnected => false;

        public void Connect()
        {
            if (_testingConnection)
            {
                return;
            }
            _testingConnection = true;

            _logger.Debug("Attempt to connect.");

            Task.Run(() =>
            {
                _teamCityClient.Connect(_configuration.UserName, _configuration.Password);
            })
            .ContinueWith(x =>
            {
                try
                {
                    if (_teamCityClient.Authenticate())
                    {
                        _logger.Info("Has connected.");
                        _stateEngine.OnConnected();
                    }
                    else
                    {
                        _logger.Debug("Connection failed.");
                    }
                }
                catch (Exception exception)
                {
                    _logger.Debug("Connection failed.", exception);
                }
                finally
                {
                    _testingConnection = false;
                }
            });
        }

        public void Disconnect()
        {
        }

        public Task<Build> GetLastBuild(string projectName, string buildConfigurationName)
        {
            return Task.Run(() =>
            {
                Connect();
                return (Build)null;
            });
        }

        public Task<Build> GetLastSuccessfulBuild(string projectName, string buildConfigurationName)
        {
            return Task.Run(() =>
            {
                Connect();
                return (Build)null;
            });
        }

        public Task<Build[]> GetRunningBuilds(string projectName, string buildConfigurationName, string branchName)
        {
            return Task.Run(() =>
            {
                Connect();
                return new Build[0];
            });
        }

        public Task<Build[]> GetRunningBuilds(string projectName, string buildConfigurationName)
        {
            return Task.Run(() =>
            {
                Connect();
                return new Build[0];
            });
        }

        public Task<string[]> GetConfigurationNames(string projectName)
        {
            return Task.Run(() => new string[0]);
        }

        public Task<IBuildAgent[]> GetAgents()
        {
            return Task.Run(() => new IBuildAgent[0]);
        }

        public Task<IBuildAgent> GetAgent(string name)
        {
            return Task.Run(() =>
            {
                if (!_buildAgentRepository.Has(name))
                {
                    _buildAgentRepository.Add(new TeamCityBuildAgentViewModel(name, _teamCityClient, _services.Timer));
                }
                return _buildAgentRepository.Get(name);
            });
        }
    }
}