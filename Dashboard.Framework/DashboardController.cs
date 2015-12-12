using System.Windows;
using System.Windows.Controls;
using log4net;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Dashboards;
using NoeticTools.SystemsDashboard.Framework.Input;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.Help;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.InsertTile;
using NoeticTools.SystemsDashboard.Framework.Registries;
using NoeticTools.SystemsDashboard.Framework.Time;


namespace NoeticTools.SystemsDashboard.Framework
{
    public sealed class DashboardController : IDashboardController
    {
        private readonly DashboardConfigurations _config;
        private readonly IDashboardNavigator _dashboardNavigator;
        private readonly TileProviderRegistry _tileProviderRegistry;
        private readonly TileDragAndDropController _dragAndDropController;
        private readonly DashboardConfigurationManager _configurationManager;
        private readonly ITimerService _timerService;
        private readonly DockPanel _sidePanel;
        private ILog _logger;

        public DashboardController(DashboardConfigurationManager configurationManager, ITimerService timerService, DockPanel sidePanel,
            DashboardConfigurations config, IDashboardNavigator dashboardNavigator, TileProviderRegistry tileProviderRegistry,
            TileDragAndDropController dragAndDropController)
        {
            _configurationManager = configurationManager;
            _timerService = timerService;
            _sidePanel = sidePanel;
            _config = config;
            _dashboardNavigator = dashboardNavigator;
            _tileProviderRegistry = tileProviderRegistry;
            _dragAndDropController = dragAndDropController;
            _logger = LogManager.GetLogger("UI.Dashboard");
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
            ShowOnSidePane(new HelpViewController().CreateView(), "Help");
        }

        public void ShowNavigationPane()
        {
            _logger.Info("Show navigation.");
            ShowOnSidePane(new DashboardsNavigationViewController(_config, _dashboardNavigator).CreateView(), "Dashboards Navigation");
        }

        public void ShowOnSidePane(FrameworkElement view, string title)
        {
            _logger.Debug("Show on side panel.");
            _sidePanel.Visibility = Visibility.Collapsed;
            _sidePanel.Children.Clear();

            _sidePanel.Children.Add(view);
            view.IsVisibleChanged += View_IsVisibleChanged;

            _sidePanel.Visibility = Visibility.Visible;
        }

        public void Refresh()
        {
            _logger.Info("Refresh.");
            _timerService.FireAll();
        }

        public void ToggleGroupPanelsEditMode()
        {
            _dashboardNavigator.ToggleShowGroupPanelsDetailsMode();
            //ShowOnSidePane(new GroupPanelsEditTileViewModel(_config, _tileRegistryConduit));
        }

        public void ShowInsertPanel()
        {
            ShowOnSidePane(new InsertTileController(_tileProviderRegistry, _dragAndDropController).CreateView(), "Help");
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