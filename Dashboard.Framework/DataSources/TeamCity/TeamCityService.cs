using System.Threading.Tasks;
using Dashboard.Config;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Commands;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Config.Properties;
using NoeticTools.SystemsDashboard.Framework.DataSources.TeamCity;
using NoeticTools.SystemsDashboard.Framework.Config.Controllers;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.SystemsDashboard.Framework.DataSources.TeamCity
{
    public class TeamCityService : ITeamCityChannel, IStateEngine<ITeamCityChannel>, IConfigurationChangeListener
    {
        private readonly IDashboardController _dashboardController;
        private readonly IServices _services;
        private readonly TeamCityServiceConfiguration _configuration;
        private readonly ITeamCityChannel _connectedState;
        private readonly ITeamCityChannel _disconnectedState;
        private ITeamCityChannel _current;

        public TeamCityService(DashboardConfigurationServices servicesConfiguration, RunOptions runOptions, IClock clock, IDashboardController dashboardController, IServices services)
        {
            _dashboardController = dashboardController;
            _services = services;
            _configuration = new TeamCityServiceConfiguration(servicesConfiguration.GetService("TeamCity"));
            var client = new TeamCityClient(_configuration.Url);
            _disconnectedState = new TeamCityChannelDisconnectedState(client, this, _configuration);
            _connectedState = new TeamCityChannelConnectedState(client, clock);
            _current = runOptions.EmulateMode ? new TeamCityChannelEmulatedState() : _disconnectedState;
        }

        public string[] ProjectNames => _current.ProjectNames;

        public void Connect()
        {
            _current.Connect();
        }

        public Task<Build> GetLastBuild(string projectName, string buildConfigurationName)
        {
            return _current.GetLastBuild(projectName, buildConfigurationName);
        }

        public Task<Build> GetLastSuccessfulBuild(string projectName, string buildConfigurationName)
        {
            return _current.GetLastSuccessfulBuild(projectName, buildConfigurationName);
        }

        public Task<Build> GetRunningBuild(string projectName, string buildConfigurationName, string branchName)
        {
            return _current.GetRunningBuild(projectName, buildConfigurationName, branchName);
        }

        public Task<Build> GetRunningBuild(string projectName, string buildConfigurationName)
        {
            return _current.GetRunningBuild(projectName, buildConfigurationName);
        }

        public Task<string[]> GetConfigurationNames(string projectName)
        {
            return _current.GetConfigurationNames(projectName);
        }

        public void Disconnect()
        {
        }

        public void Configure()
        {
            var configurationConverter = new TileConfigurationConverter(_configuration, this);

            var parameters = new IPropertyViewModel[]
            {
                new TextPropertyViewModel("Url", configurationConverter),
                new TextPropertyViewModel("UserName", configurationConverter),
                new PasswordPropertyViewModel("Password", configurationConverter)
            };

            const string title = "TeamCity Server Configuration";
            var controller = new ConfigationViewController(title, new RoutedCommands(), parameters, _services);
            _dashboardController.ShowOnSidePane(controller, title);
        }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
            _configuration.Password = converter.GetString("Password");
        }

        void IStateEngine<ITeamCityChannel>.OnConnected()
        {
            _current = _connectedState;
        }

        void IStateEngine<ITeamCityChannel>.OnDisconnected()
        {
            _current = _disconnectedState;
        }

        ITeamCityChannel IStateEngine<ITeamCityChannel>.Current => _current;
    }
}