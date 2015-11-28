using System.Windows;
using System.Windows.Controls;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.DataSources.TeamCity;
using NoeticTools.Dashboard.Framework.Registries;
using NoeticTools.Dashboard.Framework.Tiles;
using NoeticTools.Dashboard.Framework.Tiles.Date;
using NoeticTools.Dashboard.Framework.Tiles.DaysLeftCountDown;
using NoeticTools.Dashboard.Framework.Tiles.Help;
using NoeticTools.Dashboard.Framework.Tiles.InsertTile;
using NoeticTools.Dashboard.Framework.Tiles.Message;
using NoeticTools.Dashboard.Framework.Tiles.ServerStatus;
using NoeticTools.Dashboard.Framework.Tiles.TeamCity.AvailableBuilds;
using NoeticTools.Dashboard.Framework.Tiles.TeamCity.LastBuildStatus;
using NoeticTools.Dashboard.Framework.Tiles.TeamCityAvailableBuilds;
using NoeticTools.Dashboard.Framework.Tiles.WebPage;
using NoeticTools.Dashboard.Framework.Time;


namespace NoeticTools.Dashboard.Framework
{
    public class TeamDashboardRunner
    {
        private readonly IDashboardController _dashboardController;
        private readonly DashBoardLoader _loader;
        private readonly DashboardConfigurations _config;
        private readonly DashboardNavigator _dashboardNavigator;
        public readonly KeyboardHandler KeyboardHandler;

        public TeamDashboardRunner(Grid tileGrid, DockPanel sidePanel)
        {
            var clock = new Clock();
            var timerService = new TimerService(clock);
            var runOptions = new RunOptions();
            var tileProviderRegistry = new TileProviderRegistry();
            var tileControllerFactory = new TileControllerFactory(tileProviderRegistry);

            var tileNavigator = new DashboardTileNavigator(tileGrid);
            var dashboardConfigurationManager = new DashboardConfigurationManager();
            _config = dashboardConfigurationManager.Load();
            var loaderConduit = new DashBoardLoaderConduit();
            var tileRegistryConduit = new TileFactoryConduit();
            var tileLayoutControllerRegistry = new TileLayoutControllerRegistry(tileRegistryConduit);
            _dashboardNavigator = new DashboardNavigator(loaderConduit, _config, tileLayoutControllerRegistry);
            _dashboardController = new DashboardController(dashboardConfigurationManager, timerService, sidePanel, _config, _dashboardNavigator, tileProviderRegistry);
            var teamCityService = new TeamCityService(_config.Services, runOptions, clock, _dashboardController);

            tileRegistryConduit.SetTarget(tileControllerFactory);
            var rootTileLayoutController = new TileLayoutController(tileGrid, tileControllerFactory, tileLayoutControllerRegistry, new Thickness(0));
            _loader = new DashBoardLoader(rootTileLayoutController);
            loaderConduit.SetTarget(_loader);
            KeyboardHandler = new KeyboardHandler(tileNavigator, _dashboardNavigator, _dashboardController);

            var services = new Services(tileControllerFactory, tileProviderRegistry, KeyboardHandler);

            RegisterPlugins(teamCityService, timerService, clock, services);
        }

        private void RegisterPlugins(TeamCityService teamCityService, TimerService timerService, Clock clock, Services services)
        {
            var plugins = new IPlugin[]
            {
                new InsertTilePlugin(_dashboardController, services),
                new HelpTilePlugin(_dashboardController),

                new TeamCityLastBuildStatusTilePlugin(teamCityService, timerService, _dashboardController),
                new TeamCityLAvailbleBuildSTilePlugin(teamCityService, timerService, _dashboardController),
                new DaysLeftCountDownTilePlugin(_dashboardController, clock, timerService),
                new DateTilePlugin(timerService, clock),
                new MessageTilePlugin(_dashboardController),
                new WebPageTilePlugin(_dashboardController),
                new ServerStatusTilePlugin(_dashboardController),
            };
            foreach (var plugin in plugins)
            {
                plugin.Register(services);
            }
        }

        public void Start()
        {
            _dashboardController.Start();
            _loader.Load(_config.Configurations[_dashboardNavigator.CurrentDashboardIndex]);
        }

        public void Stop()
        {
            _dashboardController.Stop();
        }
    }
}