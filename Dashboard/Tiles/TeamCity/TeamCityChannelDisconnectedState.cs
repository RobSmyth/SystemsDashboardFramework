using System;
using Dashboard.TeamCity;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;

namespace Dashboard.Tiles.TeamCity
{
    class TeamCityChannelDisconnectedState : ITeamCityChannel
    {
        private string _teamCityUrl;
        private string _userName;
        private string _password;
        private TeamCityClient _client;
        private IStateEngine<ITeamCityChannel> _stateEngine;

        public TeamCityChannelDisconnectedState(string teamCityUrl, string userName, string password, TeamCityClient client, IStateEngine<ITeamCityChannel> stateEngine)
        {
            _teamCityUrl = teamCityUrl;
            _userName = userName;
            _password = password;
            _client = client;
            _stateEngine = stateEngine;
        }

        public void Connect()
        {
            _client.Connect("rsx", "W3!com3rs!");
            try
            {
                if (_client.Authenticate())
                {
                    _stateEngine.OnConnected();
                }
            }
            catch(Exception)
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
