using System;
using System.Threading.Tasks;
using log4net;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.Controllers;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.DataSources.TeamCity
{
    public sealed class TeamCityService : IStateEngine<ITeamCityChannel>, IConfigurationChangeListener, ITimerListener, ITeamCityService
    {
        private readonly IDashboardController _dashboardController;
        private readonly IServices _services;
        private readonly TeamCityServiceConfiguration _configuration;
        private readonly TeamCityChannelConnectedState _connectedState;
        private readonly ITeamCityChannel _disconnectedState;
        private readonly IDataSource _repository;
        private ITeamCityChannel _current;
        private ILog _logger;

        public TeamCityService(IServices services, IBuildAgentRepository buildAgentRepository, IDataSource repository)
        {
            _repository = repository;
            _dashboardController = services.DashboardController;
            _services = services;
            _configuration = new TeamCityServiceConfiguration(services.Configuration.Services.GetService("TeamCity"));
            var client = new TeamCityClient(_configuration.Url);
            _disconnectedState = new TeamCityChannelDisconnectedState(client, this, _configuration, buildAgentRepository, _services);
            _connectedState = new TeamCityChannelConnectedState(client, this, buildAgentRepository, _services);
            _current = services.RunOptions.EmulateMode ? new TeamCityChannelEmulatedState(repository) : _disconnectedState;
            _logger = LogManager.GetLogger("DateSources.TeamCity.Connected");
            _services.Timer.QueueCallback(TimeSpan.FromMilliseconds(10), this);

            _repository.Write("Service.URL", _configuration.Url);
            _repository.Write("Service.Status", services.RunOptions.EmulateMode ? "Connected" : "Stopped");
            _repository.Write("Service.Connected", false);
            _repository.Write("Service.Mode", services.RunOptions.EmulateMode ? "Emulated" : "Run");
        }

        public string[] ProjectNames => _current.ProjectNames;

        public bool IsConnected
        {
            get { return _current.IsConnected; }
        }

        public string Name => "TeamCity";

        public void Connect()
        {
            _repository.Write("Service.Status", "Connecting");
            _current.Connect();
        }

        public void Disconnect()
        {
            _repository.Write("Service.Status", "Disconnecting");
            _current.Disconnect();
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
            var controller = new ConfigationViewController(title, new TsbCommands(), parameters, _services);
            _dashboardController.ShowOnSidePane(controller.CreateView(), title);
        }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
            _configuration.Password = converter.GetString("Password");
        }

        public void Stop()
        {
            if (_services.RunOptions.EmulateMode)
            {
                return;
            }
            if (IsConnected)
            {
                _current.Disconnect();
            }
            _current = new TeamCityChannelStoppedState();
        }

        public void Start()
        {
            if (_services.RunOptions.EmulateMode)
            {
                return;
            }
            _current = _disconnectedState;
        }

        void IStateEngine<ITeamCityChannel>.OnConnected()
        {
            _current = _connectedState;
            _repository.Write("Service.Status", "Connected");
            _repository.Write("Service.Connected", true);
            _connectedState.Enter();
        }

        void IStateEngine<ITeamCityChannel>.OnDisconnected()
        {
            _current = _disconnectedState;
            _repository.Write("Service.Status", "Disconnected");
            _repository.Write("Service.Connected", false);
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            if (!IsConnected)
            {
                Connect();
            }
        }

        ITeamCityChannel IStateEngine<ITeamCityChannel>.Current => _current;
    }
}