using System.Windows;
using System.Windows.Controls;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Tiles;
using NoeticTools.Dashboard.Framework.Tiles.Dashboards;
using NoeticTools.Dashboard.Framework.Tiles.Help;
using NoeticTools.Dashboard.Framework.Time;


namespace NoeticTools.Dashboard.Framework
{
    public class DashboardController : IDashboardController
    {
        private readonly DashboardConfigurations _config;
        private readonly IDashboardNavigator _dashboardNavigator;
        private readonly DashboardConfigurationManager _configurationManager;
        private readonly ITimerService _timerService;
        private readonly DockPanel _sidePanel;

        public DashboardController(DashboardConfigurationManager configurationManager, ITimerService timerService,
            DockPanel sidePanel, DashboardConfigurations config, IDashboardNavigator dashboardNavigator)
        {
            _configurationManager = configurationManager;
            _timerService = timerService;
            _sidePanel = sidePanel;
            _config = config;
            _dashboardNavigator = dashboardNavigator;
        }

        public void Start()
        {
        }

        public void Stop()
        {
            Save();
        }

        public void ShowHelpPane()
        {
            ShowOnSidePane(new HelpTileViewModel());
        }

        public void ShowNavigationPane()
        {
            ShowOnSidePane(new DashboardsNavigationTileViewModel(_config, _dashboardNavigator));
        }

        public void ShowOnSidePane(ITileViewModel viewModel)
        {
            _sidePanel.Visibility = Visibility.Collapsed;
            _sidePanel.Children.Clear();

            var view = viewModel.CreateView();
            _sidePanel.Children.Add(view);
            view.Width = double.NaN;
            view.Height = double.NaN;
            _sidePanel.Visibility = view.Visibility;

            view.IsVisibleChanged += View_IsVisibleChanged;
        }

        public void Refresh()
        {
            _timerService.FireAll();
        }

        private void Save()
        {
            _configurationManager.Save(_config);
        }

        private void View_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var view = (UIElement) sender;

            if (_sidePanel.Children.Contains(view))
            {
                if (view.Visibility == Visibility.Collapsed)
                {
                    _sidePanel.Children.Remove(view);
                }
                if (_sidePanel.Children.Count == 0)
                {
                    _sidePanel.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}