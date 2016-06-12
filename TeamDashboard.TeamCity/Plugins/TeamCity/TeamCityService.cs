using System;
using System.Threading.Tasks;
using log4net;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.Controllers;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Agents;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity
{
    public sealed class TeamCityService : IConfigurationChangeListener, ITimerListener, ITeamCityService
    {
        private readonly IDashboardController _dashboardController;
        private readonly IServices _services;
        private readonly ITeamCityServiceConfiguration _configuration;
        private readonly IDataSource _repository;
        private ILog _logger;
        private readonly IStateEngine<ITeamCityChannel> _stateEngine;

        public TeamCityService(IServices services, IDataSource repository, ITeamCityServiceConfiguration configuration, IStateEngine<ITeamCityChannel> stateEngine)
        {
            _repository = repository;
            _dashboardController = services.DashboardController;
            _services = services;
            _configuration = configuration;
            _stateEngine = stateEngine;
            _logger = LogManager.GetLogger("DateSources.TeamCity.Connected");
            _services.Timer.QueueCallback(TimeSpan.FromMilliseconds(10), this);

            _repository.Write("Service.URL", _configuration.Url);
            _repository.Write("Service.Status", services.RunOptions.EmulateMode ? "Connected" : "Stopped");
            _repository.Write("Service.Connected", false);
            _repository.Write("Service.Mode", services.RunOptions.EmulateMode ? "Emulated" : "Run");
        }

        public string[] ProjectNames => _stateEngine.Current.ProjectNames;

        public bool IsConnected
        {
            get { return _stateEngine.Current.IsConnected; }
        }

        public string Name => "TeamCity";

        public void Connect()
        {
            _repository.Write("Service.Status", "Connecting");
            _stateEngine.Current.Connect();
        }

        public void Disconnect()
        {
            _repository.Write("Service.Status", "Disconnecting");
            _stateEngine.Current.Disconnect();
        }

        public Task<IBuildAgent[]> GetAgents()
        {
            return _stateEngine.Current.GetAgents();
        }

        public Task<IBuildAgent> GetAgent(string name)
        {
            return _stateEngine.Current.GetAgent(name);
        }

        public Task<Build> GetLastBuild(string projectName, string buildConfigurationName)
        {
            return _stateEngine.Current.GetLastBuild(projectName, buildConfigurationName);
        }

        public Task<Build> GetLastSuccessfulBuild(string projectName, string buildConfigurationName)
        {
            return _stateEngine.Current.GetLastSuccessfulBuild(projectName, buildConfigurationName);
        }

        public Task<Build[]> GetRunningBuilds(string projectName, string buildConfigurationName, string branchName)
        {
            return _stateEngine.Current.GetRunningBuilds(projectName, buildConfigurationName, branchName);
        }

        public Task<Build[]> GetRunningBuilds(string projectName, string buildConfigurationName)
        {
            return _stateEngine.Current.GetRunningBuilds(projectName, buildConfigurationName);
        }

        public Task<string[]> GetConfigurationNames(string projectName)
        {
            return _stateEngine.Current.GetConfigurationNames(projectName);
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
                _stateEngine.Current.Disconnect();
            }
            _stateEngine.Stop();
        }

        public void Start()
        {
            if (_services.RunOptions.EmulateMode)
            {
                return;
            }
            _stateEngine.Start();
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            if (!IsConnected)
            {
                Connect();
            }
        }
    }
}