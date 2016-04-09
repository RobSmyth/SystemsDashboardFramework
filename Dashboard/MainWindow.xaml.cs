using System;
using System.Windows;
using System.Windows.Input;
using log4net;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.TeamStatusBoard.Framework;


namespace NoeticTools.SystemsDashboard
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

            Loaded += LoadedHandler;
            Closed += ClosedHandler;
            KeyDown += MainWindow_KeyDown;

            log4net.Config.XmlConfigurator.Configure();
            _logger.Info("===========================================================================");
            _logger.Info("Main window created.");
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