using System.Linq;
using System.Windows;
using System.Windows.Controls;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.DataSources.TeamCity;
using NoeticTools.Dashboard.Framework.Input;
using NoeticTools.Dashboard.Framework.Plugins;
using NoeticTools.Dashboard.Framework.Plugins.PropertyEditControls;
using NoeticTools.Dashboard.Framework.Plugins.Tiles;
using NoeticTools.Dashboard.Framework.Plugins.Tiles.BlankTile;
using NoeticTools.Dashboard.Framework.Plugins.Tiles.Date;
using NoeticTools.Dashboard.Framework.Plugins.Tiles.DaysLeftCountDown;
using NoeticTools.Dashboard.Framework.Plugins.Tiles.Help;
using NoeticTools.Dashboard.Framework.Plugins.Tiles.InsertTile;
using NoeticTools.Dashboard.Framework.Plugins.Tiles.MessageTile;
using NoeticTools.Dashboard.Framework.Plugins.Tiles.ServerStatus;
using NoeticTools.Dashboard.Framework.Plugins.Tiles.TeamCity.AvailableBuilds;
using NoeticTools.Dashboard.Framework.Plugins.Tiles.TeamCity.LastBuildStatus;
using NoeticTools.Dashboard.Framework.Plugins.Tiles.WebPage;
using NoeticTools.Dashboard.Framework.Registries;
using NoeticTools.Dashboard.Framework.Tiles;
using NoeticTools.Dashboard.Framework.Tiles.Help;
using NoeticTools.Dashboard.Framework.Time;


namespace NoeticTools.Dashboard.Framework
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
        private readonly Services _services;
        private readonly PropertyEditControlRegistry _propertyEditControlRegistry;

        public TeamDashboardRunner(Grid tileGrid, DockPanel sidePanel)
        {
            _clock = new Clock();
            _timerService = new TimerService(_clock);
            _runOptions = new RunOptions();
            _dragAndDropController = new TileDragAndDropController();
            _tileNavigator = new DashboardTileNavigator(tileGrid);

            var tileProviderRegistry = new TileProviderRegistry();
            var tileControllerFactory = new TileControllerFactory(tileProviderRegistry);
            _propertyEditControlRegistry = new PropertyEditControlRegistry();

            var dashboardConfigurationManager = new DashboardConfigurationManager();
            _config = dashboardConfigurationManager.Load();
            var loaderConduit = new DashBoardLoaderConduit();

            var tileLayoutControllerRegistry = new TileLayoutControllerRegistry(tileControllerFactory, _dragAndDropController, _tileNavigator);
            _dashboardNavigator = new DashboardNavigator(loaderConduit, _config, tileLayoutControllerRegistry);
            _dashboardController = new DashboardController(dashboardConfigurationManager, _timerService, sidePanel, _config, _dashboardNavigator, tileProviderRegistry, _dragAndDropController);
            KeyboardHandler = new KeyboardHandler(_dashboardController);
            _services = new Services(tileProviderRegistry, KeyboardHandler, _propertyEditControlRegistry, _timerService);

            var rootTileLayoutController = new TileLayoutController(tileGrid, tileControllerFactory, tileLayoutControllerRegistry, new Thickness(0), _dragAndDropController, _tileNavigator, null);
            _loader = new DashBoardLoader(rootTileLayoutController);
            loaderConduit.SetTarget(_loader);

            RegisterPlugins();
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

        private void RegisterPlugins()
        {
            var teamCityService = new TeamCityService(_config.Services, _runOptions, _clock, _dashboardController, _services);

            var plugins = new IPlugin[]
            {
                new TextPropertyViewPlugin(), 
                new DatePropertyViewPlugin(), 
                new CheckboxPropertyViewPlugin(), 
                new ComboboxTextPropertyViewPlugin(), 
                new KeyboardTileNavigationPlugin(_tileNavigator),
                new KeyboardDashboardNavigationPlugin(_dashboardNavigator, _dashboardController),
                new InsertTilePlugin(_dashboardController, _services, _dragAndDropController),
                new HelpTilePlugin(_dashboardController),
                new BlankTilePlugin(_dashboardController, _services),
                new DateTilePlugin(_timerService, _clock),
                new MessageTilePlugin(_dashboardController, _services),
                new TeamCityLastBuildStatusTilePlugin(teamCityService, _dashboardController, _services),
                new TeamCityLAvailbleBuildSTilePlugin(teamCityService, _dashboardController, _services),
                new DaysLeftCountDownTilePlugin(_dashboardController, _clock, _services),
                new WebPageTilePlugin(_dashboardController, _services),
                new ServerStatusTilePlugin()
            };

            // todo - load third-party plug-ins

            foreach (var plugin in plugins.OrderBy(x => x.Rank))
            {
                plugin.Register(_services);
            }
        }
    }
}