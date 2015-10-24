using Dashboard.Config;
using Dashboard.TeamCity;
using Dashboard.Tiles.TeamCity;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;

namespace Dashboard.Services.TeamCity
{
    public class TeamCityService : ITeamCityChannel, IStateEngine<ITeamCityChannel>
    {
        private readonly TeamCityServiceConfiguration _configuration;
        private readonly ITeamCityChannel _connectedState;
        private readonly ITeamCityChannel _disconnectedState;
        private ITeamCityChannel _current;

        public TeamCityService(DashboardConfigurationServices servicesConfiguration)
        {
            _configuration = new TeamCityServiceConfiguration(servicesConfiguration.GetService("TeamCity"));
            var client = new TeamCityClient(_configuration.Url);
            _disconnectedState = new TeamCityChannelDisconnectedState(client, this, _configuration);
            _connectedState = new TeamCityChannelConnectedState(client, this);
            _current = _disconnectedState;
        }

        ITeamCityChannel IStateEngine<ITeamCityChannel>.Current
        {
            get { return _current; }
        }

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

        public void Configure()
        {
            var view = new TeamCityConfigurationView();
            var viewModel = new TeamCityConfigurationViewModel(_configuration, view);
            viewModel.Show();
        }
    }
}