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
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Agents;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Projects;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Channel
{
    public sealed class TeamCityChannel : IConfigurationChangeListener, ITimerListener, ITeamCityChannel
    {
        private readonly IDashboardController _dashboardController;
        private readonly IServices _services;
        private readonly ITeamCityDataSourceConfiguration _configuration;
        private readonly IDataSource _repository;
        private ILog _logger;
        private readonly IStateEngine<ITeamCityIoChannel> _stateEngine;

        public TeamCityChannel(IServices services, IDataSource repository, ITeamCityDataSourceConfiguration configuration, 
            IStateEngine<ITeamCityIoChannel> stateEngine, IProjectRepository projects, 
            IBuildAgentRepository buildAgentRepository, ChannelConnectionStateBroadcaster stateBroadcaster)
        {
            Agents = buildAgentRepository;
            StateBroadcaster = stateBroadcaster;
            _repository = repository;
            _dashboardController = services.DashboardController;
            _services = services;
            _configuration = configuration;
            _stateEngine = stateEngine;
            _logger = LogManager.GetLogger("DataSources.TeamCity.Channel");
            _services.Timer.QueueCallback(TimeSpan.FromMilliseconds(10), this);

            _repository.Write("URL", _configuration.Url);
            _repository.Write("Status", services.RunOptions.EmulateMode ? "Connected" : "Stopped");
            _repository.Write("Connected", false);
            _repository.Write("Mode", services.RunOptions.EmulateMode ? "Emulated" : "Run");
        }

        public string[] ProjectNames => _stateEngine.Current.ProjectNames;

        public bool IsConnected => _stateEngine.Current.IsConnected;

        public void Connect()
        {
            _repository.Write("Status", "Connecting");
            _stateEngine.Current.Connect();
        }

        public void Disconnect()
        {
            _repository.Write("Status", "Disconnecting");
            _stateEngine.Current.Disconnect();
        }

        public Task<IBuildAgent> GetAgent(string name)
        {
            return Current.GetAgent(name);
        }

        private IBuildAgentRepository Agents { get; }
        private IChannelConnectionStateBroadcaster StateBroadcaster { get; set; }

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

        private ITeamCityIoChannel Current { get { return _stateEngine.Current; } }
    }
}