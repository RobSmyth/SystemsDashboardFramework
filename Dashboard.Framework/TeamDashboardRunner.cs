using System.Linq;
using System.Windows;
using System.Windows.Controls;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Commands;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Dashboards;
using NoeticTools.SystemsDashboard.Framework.Input;
using NoeticTools.SystemsDashboard.Framework.Registries;
using NoeticTools.SystemsDashboard.Framework.Time;
using NoeticTools.SystemsDashboard.Framework.DataSources.TeamCity;
using NoeticTools.SystemsDashboard.Framework.Plugins;
using NoeticTools.SystemsDashboard.Framework.Plugins.PropertyEditControls;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.BlankTile;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.Date;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.DaysLeftCountDown;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.Help;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.Image;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.InsertTile;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.MessageTile;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.ServerStatus;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.TeamCity.AgentStatus;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.TeamCity.AvailableBuilds;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.TeamCity.LastBuildStatus;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.WebPage;
using NoeticTools.SystemsDashboard.Framework.Services;


namespace NoeticTools.SystemsDashboard.Framework
{
    public sealed class TeamDashboardRunner
    {
        private readonly IDashboardController _dashboardController;
        private readonly DashBoardLoader _loader;
        private readonly DashboardConfigurations _config;
        private readonly DashboardNavigator _dashboardNavigator;
        public readonly KeyboardHandler KeyboardHandler;
        private readonly Clock _clock;
        private readonly TimerService _timerService;
        private readonly TileDragAndDropController _dragAndDropController;
        private readonly DashboardTileNavigator _tileNavigator;
        private readonly RunOptions _runOptions;
        private readonly ApplicationServices _applicationServices;
        private readonly RoutedCommands _commands;

        public TeamDashboardRunner(Grid tileGrid, DockPanel sidePanel)
        {
            _clock = new Clock();
            _timerService = new TimerService(_clock);
            _runOptions = new RunOptions();
            _dragAndDropController = new TileDragAndDropController();
            _tileNavigator = new DashboardTileNavigator(tileGrid);
            _commands = new RoutedCommands();

            var tileProviderRegistry = new TileProviderRegistry();
            var tileControllerFactory = new TileFactory(tileProviderRegistry);
            var propertyEditControlRegistry = new PropertyEditControlRegistry();

            var dashboardConfigurationManager = new DashboardConfigurationManager();
            _config = dashboardConfigurationManager.Load();
            var loaderConduit = new DashBoardLoaderConduit();

            var tileLayoutControllerRegistry = new TileLayoutControllerRegistry(tileControllerFactory, _dragAndDropController, _tileNavigator, _commands);
            _dashboardNavigator = new DashboardNavigator(loaderConduit, _config, tileLayoutControllerRegistry);
            _dashboardController = new DashboardController(dashboardConfigurationManager, _timerService, sidePanel, _config, _dashboardNavigator, tileProviderRegistry, _dragAndDropController);
            KeyboardHandler = new KeyboardHandler(_dashboardController);
            _applicationServices = new ApplicationServices(tileProviderRegistry, KeyboardHandler, propertyEditControlRegistry, _timerService);

            var rootTileLayoutController = new TileLayoutController(tileGrid, tileControllerFactory, tileLayoutControllerRegistry, new Thickness(0), _dragAndDropController, _tileNavigator, null, _commands);
            _loader = new DashBoardLoader(rootTileLayoutController);
            loaderConduit.SetTarget(_loader);

            RegisterPlugins();
        }

        public void Start()
        {
            _applicationServices.Start();
            _dashboardController.Start();
            _loader.Load(_config.Configurations[_dashboardNavigator.CurrentDashboardIndex]);
        }

        public void Stop()
        {
            _dashboardController.Stop();
            _applicationServices.Stop();
        }

        private void RegisterPlugins()
        {
            var buildAgentRepository = new BuildAgentRepository();
            var teamCityService = new TeamCityService(_config.Services, _runOptions, _clock, _dashboardController, _applicationServices, buildAgentRepository);

            _applicationServices.Register(teamCityService);

            var plugins = new IPlugin[]
            {
                new TextPropertyViewPlugin(),
                new DatePropertyViewPlugin(),
                new CheckboxPropertyViewPlugin(),
                new ComboboxTextPropertyViewPlugin(),
                new KeyboardTileNavigationPlugin(_tileNavigator),
                new KeyboardDashboardNavigationPlugin(_dashboardNavigator, _dashboardController),
                new InsertTilePlugin(_dashboardController, _applicationServices, _dragAndDropController),
                new HelpTilePlugin(_dashboardController),
                new BlankTilePlugin(_dashboardController, _applicationServices),
                new ImageFileWatcherTilePlugin(_dashboardController, _applicationServices), 
                new DateTilePlugin(_timerService, _clock),
                new MessageTilePlugin(_dashboardController, _applicationServices),
                new TeamCityAgentStatusTilePlugin(teamCityService, _dashboardController, _applicationServices), 
                new TeamCityLastBuildStatusTilePlugin(teamCityService, _dashboardController, _applicationServices),
                new TeamCityLAvailbleBuildSTilePlugin(teamCityService, _dashboardController, _applicationServices),
                new DaysLeftCountDownTilePlugin(_dashboardController, _clock, _applicationServices),
                new WebPageTilePlugin(_dashboardController, _applicationServices),
                new WmiTilePlugin(_dashboardController, _applicationServices),
            };

            // todo - load third-party plug-ins via app config file

            foreach (var plugin in plugins.OrderBy(x => x.Rank))
            {
                plugin.Register(_applicationServices);
            }
        }
    }
}