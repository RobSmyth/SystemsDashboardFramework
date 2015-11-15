using System;
using System.Windows;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.DataSources.TeamCity;
using NoeticTools.Dashboard.Framework.Tiles;
using NoeticTools.Dashboard.Framework.Time;


namespace NoeticTools.TeamDashboard
{
    public partial class MainWindow : Window
    {
        private readonly IDashboardController _controller;
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
            _controller = new DashboardController(dashboardConfigurationManager, timerService, sidePanel, _config, _dashboardNavigator);
            var tileRegistry = new TileRegistry(new TeamCityService(_config.Services, runOptions, clock), clock, timerService, _controller);
            tileRegistryConduit.SetTarget(tileRegistry);
            var rootTileLayoutController = new TileLayoutController(tileGrid, tileRegistry, tileLayoutControllerRegistry, new Thickness(0));
            _loader = new DashBoardLoader(rootTileLayoutController);
            loaderConduit.SetTarget(_loader);
            _keyboardHandler = new KeyboardHandler(tileNavigator, _dashboardNavigator, _controller);

            Loaded += LoadedHandler;
            Closed += ClosedHandler;
            KeyDown += MainWindow_KeyDown;
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
            _controller.Stop();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            _controller.Start();
            _loader.Load(_config.Configurations[_dashboardNavigator.CurrentDashboardIndex]);
            MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }
    }
}