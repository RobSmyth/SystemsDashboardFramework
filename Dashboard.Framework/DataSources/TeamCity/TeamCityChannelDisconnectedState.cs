using System;
using System.Threading.Tasks;
using log4net;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.SystemsDashboard.Framework.DataSources.TeamCity
{
    internal class TeamCityChannelDisconnectedState : ITeamCityChannel
    {
        private readonly TeamCityClient _client;
        private readonly TeamCityServiceConfiguration _configuration;
        private readonly IStateEngine<ITeamCityChannel> _stateEngine;
        private bool _testingConnection;
        private readonly ILog _logger;

        public TeamCityChannelDisconnectedState(TeamCityClient client, IStateEngine<ITeamCityChannel> stateEngine,
            TeamCityServiceConfiguration configuration)
        {
            _client = client;
            _stateEngine = stateEngine;
            _configuration = configuration;

            _logger = LogManager.GetLogger("DateSources.TeamCity.Disconnected");
        }

        public string[] ProjectNames => new string[0];
        public bool IsConnected => false;

        public void Connect()
        {
            if (_testingConnection)
            {
                _logger.Debug("Connect request ignored (busy testing the connection).");
            }
            _testingConnection = true;

            _logger.Debug("Attempt to connect.");

            Task.Run(() =>
            {
                _client.Connect(_configuration.UserName, _configuration.Password);
            })
            .ContinueWith(x =>
            {
                try
                {
                    if (_client.Authenticate())
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
    }
}