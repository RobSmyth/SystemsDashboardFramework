using System;
using System.Windows;
using System.Windows.Input;
using NoeticTools.SystemsDashboard.Framework;


namespace NoeticTools.SystemsDashboard
{
    public partial class MainWindow : Window
    {
        private readonly TeamDashboardRunner _runner;

        public MainWindow()
        {
            InitializeComponent();

            _runner = new TeamDashboardRunner(tileGrid, sidePanel);

            Loaded += LoadedHandler;
            Closed += ClosedHandler;
            KeyDown += MainWindow_KeyDown;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.IsDown)
            {
                _runner.KeyboardHandler.OnKeyDown(e.Key);
            }
        }

        private void ClosedHandler(object sender, EventArgs e)
        {
            _runner.Stop();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            _runner.Start();
            MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }
    }
}