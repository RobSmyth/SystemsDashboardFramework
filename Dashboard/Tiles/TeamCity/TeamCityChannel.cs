using Dashboard.TeamCity;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;

namespace Dashboard.Tiles.TeamCity
{
    class TeamCityChannel : ITeamCityChannel, IStateEngine<ITeamCityChannel>
    {
        private TeamCityClient _client;
        private ITeamCityChannel _disconnectedState;
        private ITeamCityChannel _connectedState;
        private ITeamCityChannel _current;

        public TeamCityChannel(string teamCityUrl, string userName, string password)
        {
            _client = new TeamCityClient(teamCityUrl);
            _disconnectedState = new TeamCityChannelDisconnectedState(teamCityUrl, userName, password, _client, this);
            _connectedState = new TeamCityChannelConnectedState(_client, this);
            _current = _disconnectedState;
        }

        ITeamCityChannel IStateEngine<ITeamCityChannel>.Current { get { return _current; } }

        void IStateEngine<ITeamCityChannel>.OnConnected()
        {
            _current = _connectedState;
        }

        void IStateEngine<ITeamCityChannel>.OnDisconnected()
        {
            _current = _disconnectedState;
        }

        public void Connect()
        {
            _disconnectedState.Connect();
        }

        public Build GetLastBuild(string projectName, string buildConfigurationName)
        {
            return _current.GetLastBuild(projectName, buildConfigurationName);
        }

        public Build GetLastSuccessfulBuild(string projectName, string buildConfigurationName)
        {
            return _current.GetLastSuccessfulBuild(projectName, buildConfigurationName);
        }

        public Build GetLastSuccessfulBuild(string projectName, string buildConfigurationName, string branchName)
        {
            return _current.GetLastSuccessfulBuild(projectName, buildConfigurationName, branchName);
        }

        public Build GetRunningBuild(string projectName, string buildConfigurationName, string branchName)
        {
            return _current.GetRunningBuild(projectName, buildConfigurationName, branchName);
        }

        public Build GetRunningBuild(string projectName, string buildConfigurationName)
        {
            return _current.GetRunningBuild(projectName, buildConfigurationName);
        }

        public void Disconnect()
        {
            // does nothing
        }
    }
}
