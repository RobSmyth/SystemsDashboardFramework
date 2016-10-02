using System;
using System.Threading.Tasks;
using log4net;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.Controllers;
using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Config.SuggestionProviders;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Agents;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Channel
{
    public sealed class TeamCityChannel : IConfigurationChangeListener, ITimerListener, ITeamCityChannel
    {
        private const string PropertyTag = "TeamCity.Channel";
        private readonly IDashboardController _dashboardController;
        private readonly IServices _services;
        private readonly ITeamCityDataSourceConfiguration _configuration;
        private readonly IDataSource _repository;
        private ILog _logger;
        private readonly IStateEngine<ITeamCityIoChannel> _stateEngine;
        private readonly INamedValueRepository _configurationNamedValues;

        public TeamCityChannel(IServices services, IDataSource repository, ITeamCityDataSourceConfiguration configuration, IStateEngine<ITeamCityIoChannel> stateEngine, IBuildAgentRepository buildAgentRepository, IChannelConnectionStateBroadcaster stateBroadcaster, ITileProperties properties)
        {
            Agents = buildAgentRepository;
            StateBroadcaster = stateBroadcaster;
            _configurationNamedValues = properties.Properties;
            _repository = repository;
            _dashboardController = services.DashboardController;
            _services = services;
            _configuration = configuration;
            _stateEngine = stateEngine;
            _logger = LogManager.GetLogger("DataSources.TeamCity.Channel");
            _services.Timer.QueueCallback(TimeSpan.FromMilliseconds(10), this);

            _repository.Set("Service.URL", _configuration.Url, PropertiesFlags.ReadOnly, PropertyTag);
            _repository.Set("Service.Status", services.RunOptions.EmulateMode ? "Connected" : "Stopped", PropertiesFlags.ReadOnly, PropertyTag);
            _repository.Set("Service.Connected", false, PropertiesFlags.ReadOnly, PropertyTag);
            _repository.Set("Service.Mode", services.RunOptions.EmulateMode ? "Emulated" : "Run", PropertiesFlags.ReadOnly, PropertyTag);
        }

        public string[] ProjectNames => _stateEngine.Current.ProjectNames;

        public bool IsConnected => _stateEngine.Current.IsConnected;

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

        public Task<IBuildAgent> GetAgent(string name)
        {
            return Current.GetAgent(name);
        }

        private IBuildAgentRepository Agents { get; }
        private IChannelConnectionStateBroadcaster StateBroadcaster { get; set; }

        public void Configure()
        {
            var parameters = new IPropertyViewModel[]
            {
                new TextPropertyViewModel("Url", _configurationNamedValues, _services),
                new TextPropertyViewModel("UserName", _configurationNamedValues, _services),
                new TextPropertyViewModel("Password", _configurationNamedValues, _services),
                new TextPropertyViewModel("AgentsFilter", _configurationNamedValues, _services), 
            };

            const string title = "TeamCity Server Configuration";
            var controller = new ConfigationViewController(title, new TsbCommands(), parameters, _services);
            _dashboardController.ShowOnSidePane(controller.CreateView(), title);
        }

        public void OnConfigurationChanged(INamedValueRepository converter)
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