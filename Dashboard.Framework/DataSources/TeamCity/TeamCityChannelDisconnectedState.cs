using System;
using System.Threading.Tasks;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.SystemsDashboard.Framework.DataSources.TeamCity
{
    internal class TeamCityChannelDisconnectedState : ITeamCityChannel
    {
        private readonly TeamCityClient _client;
        private readonly TeamCityServiceConfiguration _configuration;
        private readonly IStateEngine<ITeamCityChannel> _stateEngine;
        private readonly Build _nullBuild = new NullBuild();
        private bool _testingConnection;

        public TeamCityChannelDisconnectedState(TeamCityClient client, IStateEngine<ITeamCityChannel> stateEngine,
            TeamCityServiceConfiguration configuration)
        {
            _client = client;
            _stateEngine = stateEngine;
            _configuration = configuration;
        }

        public string[] ProjectNames => new string[0];

        public void Connect()
        {
            if (_testingConnection)
            {
                return;
            }
            _testingConnection = true;

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
                        _stateEngine.OnConnected();
                    }
                }
                catch (Exception)
                {
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
                return _nullBuild;
            });
        }

        public Task<Build> GetLastSuccessfulBuild(string projectName, string buildConfigurationName)
        {
            return Task.Run(() =>
            {
                Connect();
                return _nullBuild;
            });
        }

        public Task<Build> GetRunningBuild(string projectName, string buildConfigurationName, string branchName)
        {
            return Task.Run(() =>
            {
                Connect();
                return _nullBuild;
            });
        }

        public Task<Build> GetRunningBuild(string projectName, string buildConfigurationName)
        {
            return Task.Run(() =>
            {
                Connect();
                return _nullBuild;
            });
        }

        public Task<string[]> GetConfigurationNames(string projectName)
        {
            return Task.Run(() => new string[0]);
        }
    }
}