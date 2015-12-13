using System;
using System.Threading.Tasks;
using Dashboard.Config;
using log4net;
using NoeticTools.SystemsDashboard.Framework.Commands;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Config.Properties;
using NoeticTools.SystemsDashboard.Framework.Config.Controllers;
using NoeticTools.SystemsDashboard.Framework.Time;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.SystemsDashboard.Framework.DataSources.TeamCity
{
    public class TeamCityService : ITeamCityChannel, IStateEngine<ITeamCityChannel>, IConfigurationChangeListener, ITimerListener
    {
        private readonly IDashboardController _dashboardController;
        private readonly IServices _services;
        private readonly TeamCityServiceConfiguration _configuration;
        private readonly TeamCityChannelConnectedState _connectedState;
        private readonly ITeamCityChannel _disconnectedState;
        private ITeamCityChannel _current;
        private ILog _logger;

        public TeamCityService(DashboardConfigurationServices servicesConfiguration, RunOptions runOptions, IClock clock, IDashboardController dashboardController, IServices services, IBuildAgentRepository buildAgentRepository)
        {
            _dashboardController = dashboardController;
            _services = services;
            _configuration = new TeamCityServiceConfiguration(servicesConfiguration.GetService("TeamCity"));
            var client = new TeamCityClient(_configuration.Url);
            _disconnectedState = new TeamCityChannelDisconnectedState(client, this, _configuration, buildAgentRepository, _services);
            _connectedState = new TeamCityChannelConnectedState(client, clock, buildAgentRepository, _services);
            _current = runOptions.EmulateMode ? new TeamCityChannelEmulatedState() : _disconnectedState;
            _logger = LogManager.GetLogger("DateSources.TeamCity.Connected");
            _services.Timer.QueueCallback(TimeSpan.FromMilliseconds(10), this);
        }

        public string[] ProjectNames => _current.ProjectNames;

        public void Connect()
        {
            _current.Connect();
        }

        public Task<IBuildAgent[]> GetAgents()
        {
            return _current.GetAgents();
        }

        public Task<IBuildAgent> GetAgent(string name)
        {
            return _current.GetAgent(name);
        }

        public Task<Build> GetLastBuild(string projectName, string buildConfigurationName)
        {
            return _current.GetLastBuild(projectName, buildConfigurationName);
        }

        public Task<Build> GetLastSuccessfulBuild(string projectName, string buildConfigurationName)
        {
            return _current.GetLastSuccessfulBuild(projectName, buildConfigurationName);
        }

        public Task<Build[]> GetRunningBuilds(string projectName, string buildConfigurationName, string branchName)
        {
            return _current.GetRunningBuilds(projectName, buildConfigurationName, branchName);
        }

        public Task<Build[]> GetRunningBuilds(string projectName, string buildConfigurationName)
        {
            return _current.GetRunningBuilds(projectName, buildConfigurationName);
        }

        public Task<string[]> GetConfigurationNames(string projectName)
        {
            return _current.GetConfigurationNames(projectName);
        }

        public bool IsConnected
        {
            get { return _current.IsConnected; }
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
            _dashboardController.ShowOnSidePane(controller.CreateView(), title);
        }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
            _configuration.Password = converter.GetString("Password");
        }

        void IStateEngine<ITeamCityChannel>.OnConnected()
        {
            _current = _connectedState;
            _connectedState.Enter();
        }

        void IStateEngine<ITeamCityChannel>.OnDisconnected()
        {
            _current = _disconnectedState;
        }

        ITeamCityChannel IStateEngine<ITeamCityChannel>.Current => _current;

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            if (!IsConnected)
            {
                Connect();
            }
        }
    }
}