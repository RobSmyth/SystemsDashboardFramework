using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Dashboard.Config;
using Dashboard.Framework.Config;

namespace Dashboard
{
    public partial class MainWindow : Window
    {
        private readonly DashboardController _controller;

        public MainWindow()
        {
            InitializeComponent();

            _controller = new DashboardController(new DashboardConfigurationManager());

            Loaded += LoadedHandler;
            Closed += ClosedHandler;
            KeyDown += MainWindow_KeyDown;
        }

        void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var keyDownHandlers = new Dictionary<Key, Action<Key>>
            {
                {Key.Home, key => _controller.ShowFirstDashboard()},
                {Key.End, key => _controller.ShowLastDashboard()},
                {Key.PageDown, key => _controller.NextDashboard()},
                {Key.PageUp, key => _controller.PrevDashboard()},
                {Key.F1, key => _controller.ShowHelp()},
                {Key.F3, key => _controller.ShowNavigation()},
                {Key.Escape, key => _controller.ShowCurrentDashboard()},
            };

            if (e.IsDown && keyDownHandlers.ContainsKey(e.Key))
            {
                keyDownHandlers[e.Key](e.Key);
            }
        }

        private void ClosedHandler(object sender, EventArgs e)
        {
            _controller.Stop();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            _controller.Start(tileGrid);
        }
    }
}