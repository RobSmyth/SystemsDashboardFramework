using System.Windows;
using System.Windows.Controls;
using log4net;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Input;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.Help;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.InsertTile;
using NoeticTools.SystemsDashboard.Framework.Registries;
using NoeticTools.SystemsDashboard.Framework.Services.TimeServices;


namespace NoeticTools.SystemsDashboard.Framework.Dashboards
{
    public sealed class DashboardController : IDashboardController
    {
        private readonly DashboardConfigurations _config;
        private readonly TileProviderRegistry _tileProviderRegistry;
        private readonly DashboardConfigurationManager _configurationManager;
        private readonly ITimerService _timerService;
        private readonly DockPanel _sidePanel;
        private readonly ILog _logger;

        public DashboardController(DashboardConfigurationManager configurationManager, ITimerService timerService, DockPanel sidePanel, DashboardConfigurations config, IDashboardNavigator dashboardNavigator,
            TileProviderRegistry tileProviderRegistry, ITileDragAndDropController dragAndDropController, IDashboardTileNavigator tileNavigator)
        {
            _configurationManager = configurationManager;
            _timerService = timerService;
            _sidePanel = sidePanel;
            _config = config;
            DashboardNavigator = dashboardNavigator;
            TileNavigator = tileNavigator;
            _tileProviderRegistry = tileProviderRegistry;
            DragAndDropController = dragAndDropController;
            _logger = LogManager.GetLogger("UI.Dashboard");
        }

        public IDashboardNavigator DashboardNavigator { get; }
        public IDashboardTileNavigator TileNavigator { get; }
        public ITileDragAndDropController DragAndDropController { get; }

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
            ShowOnSidePane(new DashboardsNavigationViewController(_config, DashboardNavigator).CreateView(), "Dashboards Navigation");
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
            DashboardNavigator.ToggleShowGroupPanelsDetailsMode();
            //ShowOnSidePane(new GroupPanelsEditTileViewModel(_config, _tileRegistryConduit));
        }

        public void ShowInsertPanel()
        {
            ShowOnSidePane(new InsertTileController(_tileProviderRegistry, DragAndDropController).CreateView(), "Help");
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