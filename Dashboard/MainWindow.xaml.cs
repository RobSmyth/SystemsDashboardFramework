using System;
using System.Windows;
using System.Windows.Input;
using log4net;
using NoeticTools.TeamStatusBoard.Runner;


namespace NoeticTools.TeamStatusBoard
{
    public partial class MainWindow : Window
    {
        private readonly TeamDashboardRunner _runner;
        private ILog _logger;

        public MainWindow()
        {
            InitializeComponent();

            _logger = LogManager.GetLogger("UI.MainWindow");

            _runner = new TeamDashboardRunner(tileGrid, sidePanel);
            sidePanel.IsVisibleChanged += OnConfigurationVisibleChanged;

            Loaded += LoadedHandler;
            Closed += ClosedHandler;
            KeyDown += MainWindow_KeyDown;
            SizeChanged += MainWindow_SizeChanged;

            log4net.Config.XmlConfigurator.Configure();
            _logger.Info("===========================================================================");
            _logger.Info("Main window created.");
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var sidePanelWidth = Math.Min(ActualWidth / 2, 800.0);
            configTileLeftSpace.Height = ActualHeight * sidePanelWidth / ActualWidth;
            sidePanel.Width = sidePanelWidth;
        }

        private void OnConfigurationVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            configTileLeftSpace.Visibility = sidePanel.Visibility;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.IsDown)
            {
                _logger.DebugFormat("Key {0} down.", e.Key);
                _runner.KeyboardHandler.OnKeyDown(e.Key);
            }
        }

        private void ClosedHandler(object sender, EventArgs e)
        {
            _logger.Info("Closing.");
            _runner.Stop();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            _logger.Debug("Loading.");
            _runner.Start();
            MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }
    }
}