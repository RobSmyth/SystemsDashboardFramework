using System.Windows;
using System.Windows.Controls;
using NoeticTools.Dashboard.Framework;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.DataSources.TeamCity;
using NoeticTools.Dashboard.Framework.Tiles;
using NoeticTools.Dashboard.Framework.Tiles.Dashboards;
using NoeticTools.Dashboard.Framework.Tiles.Help;
using NoeticTools.Dashboard.Framework.Time;

namespace NoeticTools.TeamDashboard
{
    public class DashboardController : IDashboardController
    {
        private readonly DashboardConfigurations _config;
        private readonly DashboardConfigurationManager _configurationManager;
        private readonly RunOptions _runOptions;
        private readonly IClock _clock;
        private readonly ITimerService _timerService;
        private Grid _tileGrid;
        private DockPanel _sidePanel;

        public DashboardController(DashboardConfigurationManager configurationManager, RunOptions runOptions, IClock clock, ITimerService timerService)
        {
            _configurationManager = configurationManager;
            _runOptions = runOptions;
            _clock = clock;
            _timerService = timerService;
            _config = new DashboardConfigurationManager().Load();
        }

        public int DashboardIndex { get; private set; } = 0;

        public void Start(Grid tileGrid, DockPanel sidePanel)
        {
            _tileGrid = tileGrid;
            _sidePanel = sidePanel;
            Load(tileGrid, _config.Configurations[DashboardIndex]);
        }

        private void Load(Grid tileGrid, DashboardConfiguration activeConfiguration)
        {
            ClearTileGrid();

            var channel = new TeamCityService(_config.Services, _runOptions);
            var layout = new TileLayoutController(tileGrid, channel, activeConfiguration, _clock, _timerService, this);

            foreach (var tile in activeConfiguration.RootTile.Tiles)
            {
                layout.AddTile(tile);
            }
        }

        private void ClearTileGrid()
        {
            _tileGrid.Children.Clear();
            _tileGrid.RowDefinitions.Clear();
            _tileGrid.ColumnDefinitions.Clear();
        }

        private void Save()
        {
            _configurationManager.Save(_config);
        }

        public void Stop()
        {
            Save();
        }

        public void ShowDashboard(int index)
        {
            DashboardIndex = index;
            Load(_tileGrid, _config.Configurations[DashboardIndex]);
        }

        public void NextDashboard()
        {
            _sidePanel.Visibility = Visibility.Collapsed;
            if (++DashboardIndex >= _config.Configurations.Length)
            {
                DashboardIndex = 0;
            }
            Load(_tileGrid, _config.Configurations[DashboardIndex]);
        }

        public void PrevDashboard()
        {
            _sidePanel.Visibility = Visibility.Collapsed;
            if (--DashboardIndex < 0)
            {
                DashboardIndex = _config.Configurations.Length-1;
            }
            Load(_tileGrid, _config.Configurations[DashboardIndex]);
        }

        public void ShowFirstDashboard()
        {
            _sidePanel.Visibility = Visibility.Collapsed;
            DashboardIndex = 0;
            Load(_tileGrid, _config.Configurations[DashboardIndex]);
        }

        public void ShowLastDashboard()
        {
            _sidePanel.Visibility = Visibility.Collapsed;
            DashboardIndex = _config.Configurations.Length - 1;
            Load(_tileGrid, _config.Configurations[DashboardIndex]);
        }

        public void ShowCurrentDashboard()
        {
            _sidePanel.Visibility = Visibility.Collapsed;
            Load(_tileGrid, _config.Configurations[DashboardIndex]);
        }

        public void ShowHelp()
        {
            ShowOnSidePane(new HelpTileViewModel());
        }

        public void ShowNavigation()
        {
            ShowOnSidePane(new DashboardsNavigationTileViewModel(_config, this));
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

        public void Refresh()
        {
            _timerService.FireAll();
        }
    }
}