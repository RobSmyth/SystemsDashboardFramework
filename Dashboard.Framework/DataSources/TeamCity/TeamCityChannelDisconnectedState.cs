using System;
using Dashboard.Services.TeamCity;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;

namespace NoeticTools.Dashboard.Framework.DataSources.TeamCity
{
    internal class TeamCityChannelDisconnectedState : ITeamCityChannel
    {
        private readonly TeamCityClient _client;
        private readonly TeamCityServiceConfiguration _configuration;
        private readonly IStateEngine<ITeamCityChannel> _stateEngine;

        public TeamCityChannelDisconnectedState(TeamCityClient client, IStateEngine<ITeamCityChannel> stateEngine,
            TeamCityServiceConfiguration configuration)
        {
            _client = client;
            _stateEngine = stateEngine;
            _configuration = configuration;
        }

        public void Connect()
        {
            _client.Connect(_configuration.UserName, _configuration.Password);
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
        }

        public void Disconnect()
        {
        }

        public Build GetLastBuild(string projectName, string buildConfigurationName)
        {
            Connect();
            return null;
        }

        public Build GetLastSuccessfulBuild(string projectName, string buildConfigurationName)
        {
            Connect();
            return null;
        }

        public Build GetLastSuccessfulBuild(string projectName, string buildConfigurationName, string branchName)
        {
            Connect();
            return null;
        }

        public Build GetRunningBuild(string projectName, string buildConfigurationName, string branchName)
        {
            Connect();
            return null;
        }

        public Build GetRunningBuild(string projectName, string buildConfigurationName)
        {
            Connect();
            return null;
        }
    }
}