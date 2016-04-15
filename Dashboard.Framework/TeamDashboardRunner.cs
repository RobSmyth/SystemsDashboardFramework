using System.Linq;
using System.Windows;
using System.Windows.Controls;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Dashboards;
using NoeticTools.SystemsDashboard.Framework.Input;
using NoeticTools.SystemsDashboard.Framework.Services;
using NoeticTools.SystemsDashboard.Framework.Services.DataServices;
using NoeticTools.SystemsDashboard.Framework.Services.TimeServices;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.DataSources.TeamCity;
using NoeticTools.TeamStatusBoard.Framework.Plugins;
using NoeticTools.TeamStatusBoard.Framework.Plugins.DataSources.DashboardData;
using NoeticTools.TeamStatusBoard.Framework.Plugins.PropertyEditControls;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.BlankTile;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.DataTiles.DateTimeTile;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.DataTiles.TextTile;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.Date;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.DaysLeftCountDown;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.ExpiredTimeAlert;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.Help;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.Image;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.InsertTile;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.MessageTile;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.ServerStatus;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.TeamCity.AgentStatus;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.TeamCity.AvailableBuilds;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.TeamCity.LastBuildStatus;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.WebPage;
using NoeticTools.TeamStatusBoard.Framework.Registries;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.Framework
{
    public sealed class TeamDashboardRunner
    {
        private readonly IDashboardController _dashboardController;
        private readonly DashBoardLoader _loader;
        private readonly DashboardConfigurations _config;
        private readonly DashboardNavigator _dashboardNavigator;
        public readonly KeyboardHandler KeyboardHandler;
        private readonly ApplicationServices _applicationServices;

        public TeamDashboardRunner(Grid tileGrid, DockPanel sidePanel)
        {
            var dragAndDropController = new TileDragAndDropController();
            var tileNavigator = new DashboardTileNavigator(tileGrid);
            var clock = new Clock();
            var timerService = new TimerService(clock);
            var commands = new TsbCommands();
            var tileProviderRegistry = new TileProviderRegistry();
            var tileControllerFactory = new TileFactory(tileProviderRegistry);
            var propertyEditControlRegistry = new PropertyEditControlRegistry();

            var dashboardConfigurationManager = new DashboardConfigurationManager();
            _config = dashboardConfigurationManager.Load();
            var loaderConduit = new DashBoardLoaderConduit();

            var tileLayoutControllerRegistry = new TileLayoutControllerRegistry(tileControllerFactory, dragAndDropController, tileNavigator, commands);
            _dashboardNavigator = new DashboardNavigator(loaderConduit, _config, tileLayoutControllerRegistry);
            _dashboardController = new DashboardController(dashboardConfigurationManager, timerService, sidePanel, _config, _dashboardNavigator, tileProviderRegistry, dragAndDropController, tileNavigator);
            KeyboardHandler = new KeyboardHandler(_dashboardController);

            _applicationServices = new ApplicationServices(
                tileProviderRegistry, KeyboardHandler, propertyEditControlRegistry, timerService, 
                new DataServer(new DataRepositoryFactory()), 
                clock,
                _dashboardController,
                _config,
                new RunOptions());

            TsbCommands.SetDefaultShowDataSources(new ShowDataSourcesCommand(_applicationServices));

            var rootTileLayoutController = new TileLayoutController(tileGrid, tileControllerFactory, tileLayoutControllerRegistry, new Thickness(0), dragAndDropController, tileNavigator, null, commands);
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
            var plugins = new IPlugin[]
            {
                new TeamCityServicePlugin(),
                new DashboardDataSourcePlugin(), 
                new TextPropertyViewPlugin(),
                new DatePropertyViewPlugin(),
                new TimeSpanPropertyViewPlugin(), 
                new CheckboxPropertyViewPlugin(),
                new ComboboxTextPropertyViewPlugin(),
                new KeyboardTileNavigationPlugin(),
                new KeyboardDashboardNavigationPlugin(),
                new InsertTilePlugin(),
                new HelpTilePlugin(),
                new BlankTilePlugin(),
                new TextDataTilePlugin(), 
                new DateTimeDataTilePlugin(), 
                new ImageFileWatcherTilePlugin(), 
                new DateTilePlugin(),
                new MessageTilePlugin(),
                new TeamCityAgentStatusTilePlugin(), 
                new TeamCityLastBuildStatusTilePlugin(),
                new TeamCityLAvailbleBuildSTilePlugin(),
                new DaysLeftCountDownTilePlugin(),
                new WebPageTilePlugin(),
                new WmiTilePlugin(),
                new ExpiredTimeAlertTilePlugin(),
            };

            foreach (var plugin in plugins.OrderByDescending(x => x.Rank))
            {
                plugin.Register(_applicationServices);
            }
        }
    }
}