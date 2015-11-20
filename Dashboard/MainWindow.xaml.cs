using System;
using System.Windows;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework;
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
using NoeticTools.Dashboard.Framework.Tiles.TeamCityAvailableBuilds;
using NoeticTools.Dashboard.Framework.Tiles.TeamCityLastBuildStatus;
using NoeticTools.Dashboard.Framework.Tiles.WebPage;
using NoeticTools.Dashboard.Framework.Time;


namespace NoeticTools.TeamDashboard
{
    public partial class MainWindow : Window
    {
        private readonly IDashboardController _dashboardController;
        private readonly DashBoardLoader _loader;
        private readonly DashboardConfigurations _config;
        private readonly DashboardNavigator _dashboardNavigator;
        private readonly KeyboardHandler _keyboardHandler;

        public MainWindow()
        {
            InitializeComponent();

            var clock = new Clock();
            var timerService = new TimerService(clock);
            var runOptions = new RunOptions();
            var tileNavigator = new DashboardTileNavigator(tileGrid);
            var dashboardConfigurationManager = new DashboardConfigurationManager();
            _config = dashboardConfigurationManager.Load();
            var loaderConduit = new DashBoardLoaderConduit();
            var tileRegistryConduit = new TileRegistryConduit();
            var tileLayoutControllerRegistry = new TileLayoutControllerRegistry(tileRegistryConduit);
            _dashboardNavigator = new DashboardNavigator(loaderConduit, _config, tileLayoutControllerRegistry);
            _dashboardController = new DashboardController(dashboardConfigurationManager, timerService, sidePanel, _config, _dashboardNavigator);
            var teamCityService = new TeamCityService(_config.Services, runOptions, clock);
            var tileControllerRegistry = new TileControllerRegistry();
            tileRegistryConduit.SetTarget(tileControllerRegistry);
            var rootTileLayoutController = new TileLayoutController(tileGrid, tileControllerRegistry, tileLayoutControllerRegistry, new Thickness(0));
            _loader = new DashBoardLoader(rootTileLayoutController);
            loaderConduit.SetTarget(_loader);
            _keyboardHandler = new KeyboardHandler(tileNavigator, _dashboardNavigator, _dashboardController);

            var services = new Services(tileControllerRegistry, _keyboardHandler);

            RegisterPlugins(teamCityService, timerService, clock, services);

            Loaded += LoadedHandler;
            Closed += ClosedHandler;
            KeyDown += MainWindow_KeyDown;
        }

        private void RegisterPlugins(TeamCityService teamCityService, TimerService timerService, Clock clock, Services services)
        {
            var plugins = new IPlugin[]
            {
                new InsertTilePlugin(_dashboardController),
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

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.IsDown)
            {
                _keyboardHandler.OnKeyDown(e.Key);
            }
        }

        private void ClosedHandler(object sender, EventArgs e)
        {
            _dashboardController.Stop();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            _dashboardController.Start();
            _loader.Load(_config.Configurations[_dashboardNavigator.CurrentDashboardIndex]);
            MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }
    }
}