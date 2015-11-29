using System.Windows;
using System.Windows.Controls;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.DataSources.TeamCity;
using NoeticTools.Dashboard.Framework.Plugins;
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
        private readonly Clock _clock;
        private readonly TimerService _timerService;
        private readonly TileDragAndDropController _dragAndDropController;
        private readonly DashboardTileNavigator _tileNavigator;
        private readonly RunOptions _runOptions;
        private readonly Services _services;

        public TeamDashboardRunner(Grid tileGrid, DockPanel sidePanel)
        {
            _clock = new Clock();
            _timerService = new TimerService(_clock);
            _runOptions = new RunOptions();
            _dragAndDropController = new TileDragAndDropController();
            _tileNavigator = new DashboardTileNavigator(tileGrid);

            var tileProviderRegistry = new TileProviderRegistry();
            var tileControllerFactory = new TileControllerFactory(tileProviderRegistry);
            var dashboardConfigurationManager = new DashboardConfigurationManager();
            _config = dashboardConfigurationManager.Load();
            var loaderConduit = new DashBoardLoaderConduit();

            var tileLayoutControllerRegistry = new TileLayoutControllerRegistry(tileControllerFactory, _dragAndDropController);
            _dashboardNavigator = new DashboardNavigator(loaderConduit, _config, tileLayoutControllerRegistry);
            _dashboardController = new DashboardController(dashboardConfigurationManager, _timerService, sidePanel, _config, _dashboardNavigator, tileProviderRegistry, _dragAndDropController);

            var rootTileLayoutController = new TileLayoutController(tileGrid, tileControllerFactory, tileLayoutControllerRegistry, new Thickness(0), _dragAndDropController);
            _loader = new DashBoardLoader(rootTileLayoutController);
            loaderConduit.SetTarget(_loader);
            KeyboardHandler = new KeyboardHandler(_dashboardController);

            _services = new Services(tileControllerFactory, tileProviderRegistry, KeyboardHandler);

            RegisterPlugins();
        }

        private void RegisterPlugins()
        {
            var teamCityService = new TeamCityService(_config.Services, _runOptions, _clock, _dashboardController);

            var plugins = new IPlugin[]
            {
                new KeyboardTileNavigationPlugin(_tileNavigator), 
                new KeyboardDashboardNavigationPlugin(_dashboardNavigator, _dashboardController), 

                new InsertTilePlugin(_dashboardController, _services, _dragAndDropController),
                new HelpTilePlugin(_dashboardController),

                new TeamCityLastBuildStatusTilePlugin(teamCityService, _timerService, _dashboardController),
                new TeamCityLAvailbleBuildSTilePlugin(teamCityService, _timerService, _dashboardController),
                new DaysLeftCountDownTilePlugin(_dashboardController, _clock, _timerService),
                new DateTilePlugin(_timerService, _clock),
                new MessageTilePlugin(_dashboardController),
                new WebPageTilePlugin(_dashboardController),
                new ServerStatusTilePlugin(_dashboardController),
            };
            foreach (var plugin in plugins)
            {
                plugin.Register(_services);
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