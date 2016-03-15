using System.Linq;
using System.Windows;
using System.Windows.Controls;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Commands;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Dashboards;
using NoeticTools.SystemsDashboard.Framework.DataSources;
using NoeticTools.SystemsDashboard.Framework.Input;
using NoeticTools.SystemsDashboard.Framework.Registries;
using NoeticTools.SystemsDashboard.Framework.DataSources.TeamCity;
using NoeticTools.SystemsDashboard.Framework.Plugins;
using NoeticTools.SystemsDashboard.Framework.Plugins.DataSources.StaticProperties;
using NoeticTools.SystemsDashboard.Framework.Plugins.PropertyEditControls;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.BlankTile;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.Date;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.DaysLeftCountDown;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.ExpiredTimeAlert;
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
using NoeticTools.SystemsDashboard.Framework.Services.DataServices;
using NoeticTools.SystemsDashboard.Framework.Services.TimeServices;


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
        private readonly TileDragAndDropController _dragAndDropController;
        private readonly DashboardTileNavigator _tileNavigator;
        private readonly RunOptions _runOptions;
        private readonly ApplicationServices _applicationServices;

        public TeamDashboardRunner(Grid tileGrid, DockPanel sidePanel)
        {
            _clock = new Clock();
            _runOptions = new RunOptions();
            _dragAndDropController = new TileDragAndDropController();
            _tileNavigator = new DashboardTileNavigator(tileGrid);

            var timerService = new TimerService(_clock);
            var commands = new RoutedCommands();
            var tileProviderRegistry = new TileProviderRegistry();
            var tileControllerFactory = new TileFactory(tileProviderRegistry);
            var propertyEditControlRegistry = new PropertyEditControlRegistry();

            var dashboardConfigurationManager = new DashboardConfigurationManager();
            _config = dashboardConfigurationManager.Load();
            var loaderConduit = new DashBoardLoaderConduit();

            var tileLayoutControllerRegistry = new TileLayoutControllerRegistry(tileControllerFactory, _dragAndDropController, _tileNavigator, commands);
            _dashboardNavigator = new DashboardNavigator(loaderConduit, _config, tileLayoutControllerRegistry);
            _dashboardController = new DashboardController(dashboardConfigurationManager, timerService, sidePanel, _config, _dashboardNavigator, tileProviderRegistry, _dragAndDropController);
            KeyboardHandler = new KeyboardHandler(_dashboardController);
            _applicationServices = new ApplicationServices(
                tileProviderRegistry, KeyboardHandler, propertyEditControlRegistry, timerService, 
                new DataService(new DataRepositoryFactory()), 
                new DataPropertiesRepository(new DataPropertyViewModelFactory()), 
                _clock,
                _dashboardController);

            var rootTileLayoutController = new TileLayoutController(tileGrid, tileControllerFactory, tileLayoutControllerRegistry, new Thickness(0), _dragAndDropController, _tileNavigator, null, commands);
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
            var propertiesService = new StaticPropertiesService(_applicationServices);
            var buildAgentRepository = new BuildAgentRepository();
            var teamCityService = new TeamCityService(_config.Services, _runOptions, _clock, _dashboardController, _applicationServices, buildAgentRepository);

            _applicationServices.Register(teamCityService);

            var plugins = new IPlugin[]
            {
                new StaticPropertiesServicePlugIn(), 
                new TextPropertyViewPlugin(),
                new DatePropertyViewPlugin(),
                new TimeSpanPropertyViewPlugin(), 
                new CheckboxPropertyViewPlugin(),
                new ComboboxTextPropertyViewPlugin(),
                new KeyboardTileNavigationPlugin(_tileNavigator),
                new KeyboardDashboardNavigationPlugin(_dashboardNavigator, _dashboardController),
                new InsertTilePlugin(_dashboardController, _dragAndDropController),
                new HelpTilePlugin(),
                new BlankTilePlugin(),
                new ImageFileWatcherTilePlugin(_dashboardController, _applicationServices), 
                new DateTilePlugin(),
                new MessageTilePlugin(),
                new TeamCityAgentStatusTilePlugin(teamCityService, _dashboardController), 
                new TeamCityLastBuildStatusTilePlugin(teamCityService, _dashboardController),
                new TeamCityLAvailbleBuildSTilePlugin(teamCityService, _dashboardController),
                new DaysLeftCountDownTilePlugin(),
                new WebPageTilePlugin(),
                new WmiTilePlugin(),
                new ExpiredTimeAlertTilePlugin(),
            };

            foreach (var plugin in plugins.OrderBy(x => x.Rank))
            {
                plugin.Register(_applicationServices);
            }
        }
    }
}