using Dashboard.Config;
using Dashboard.Services.TeamCity;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.Dashboard.Framework.DataSources.TeamCity
{
    public class TeamCityService : ITeamCityChannel, IStateEngine<ITeamCityChannel>
    {
        private readonly TeamCityServiceConfiguration _configuration;
        private readonly ITeamCityChannel _connectedState;
        private readonly ITeamCityChannel _disconnectedState;
        private ITeamCityChannel _current;

        public TeamCityService(DashboardConfigurationServices servicesConfiguration, RunOptions runOptions, IClock clock)
        {
            _configuration = new TeamCityServiceConfiguration(servicesConfiguration.GetService("TeamCity"));
            var client = new TeamCityClient(_configuration.Url);
            _disconnectedState = new TeamCityChannelDisconnectedState(client, this, _configuration);
            _connectedState = new TeamCityChannelConnectedState(client, this, clock);
            _current = runOptions.EmulateMode ? new TeamCityChannelEmulatedState(client, this) : _disconnectedState;
        }

        public void Connect()
        {
            _current.Connect();
        }

        public Build GetLastBuild(string projectName, string buildConfigurationName)
        {
            return _current.GetLastBuild(projectName, buildConfigurationName);
        }

        public Build GetLastSuccessfulBuild(string projectName, string buildConfigurationName)
        {
            return _current.GetLastSuccessfulBuild(projectName, buildConfigurationName);
        }

        public Build GetRunningBuild(string projectName, string buildConfigurationName, string branchName)
        {
            return _current.GetRunningBuild(projectName, buildConfigurationName, branchName);
        }

        public Build GetRunningBuild(string projectName, string buildConfigurationName)
        {
            return _current.GetRunningBuild(projectName, buildConfigurationName);
        }

        public string[] ProjectNames => _current.ProjectNames;

        public string[] GetConfigurationNames(string projectName)
        {
            return _current.GetConfigurationNames(projectName);
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

        ITeamCityChannel IStateEngine<ITeamCityChannel>.Current => _current;

        void IStateEngine<ITeamCityChannel>.OnConnected()
        {
            _current = _connectedState;
        }

        void IStateEngine<ITeamCityChannel>.OnDisconnected()
        {
            _current = _disconnectedState;
        }
    }
}