using System.Windows;
using System.Windows.Controls;
using NoeticTools.Dashboard.Framework;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.DataSources.TeamCity;
using NoeticTools.TeamDashboard.Panes.HelpPane;
using NoeticTools.TeamDashboard.Panes.NavigationPane;

namespace NoeticTools.TeamDashboard
{
    public class DashboardController
    {
        private readonly DashboardConfigurations _config;
        private readonly DashboardConfigurationManager _configurationManager;
        private readonly RunOptions _runOptions;
        private readonly IClock _clock;
        private int _dashboardIndex = 0;
        private Grid _tileGrid;
        private DockPanel _sidePanel;

        public DashboardController(DashboardConfigurationManager configurationManager, RunOptions runOptions, IClock clock)
        {
            _configurationManager = configurationManager;
            _runOptions = runOptions;
            _clock = clock;
            _config = new DashboardConfigurationManager().Load();
        }

        public void Start(Grid tileGrid, DockPanel sidePanel)
        {
            _tileGrid = tileGrid;
            _sidePanel = sidePanel;
            Load(tileGrid, _config.Configurations[_dashboardIndex]);
        }

        private void Load(Grid tileGrid, DashboardConfiguration activeConfiguration)
        {
            ClearTileGrid();

            var channel = new TeamCityService(_config.Services, _runOptions);
            var layout = new TileLayoutController(tileGrid, channel, activeConfiguration, _clock);

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

        public void NextDashboard()
        {
            _sidePanel.Visibility = Visibility.Collapsed;
            if (++_dashboardIndex >= _config.Configurations.Length)
            {
                _dashboardIndex = 0;
            }
            Load(_tileGrid, _config.Configurations[_dashboardIndex]);
        }

        public void PrevDashboard()
        {
            _sidePanel.Visibility = Visibility.Collapsed;
            if (--_dashboardIndex < 0)
            {
                _dashboardIndex = _config.Configurations.Length-1;
            }
            Load(_tileGrid, _config.Configurations[_dashboardIndex]);
        }

        public void ShowFirstDashboard()
        {
            _sidePanel.Visibility = Visibility.Collapsed;
            _dashboardIndex = 0;
            Load(_tileGrid, _config.Configurations[_dashboardIndex]);
        }

        public void ShowLastDashboard()
        {
            _sidePanel.Visibility = Visibility.Collapsed;
            _dashboardIndex = _config.Configurations.Length - 1;
            Load(_tileGrid, _config.Configurations[_dashboardIndex]);
        }

        public void ShowHelp()
        {
            _sidePanel.Visibility = Visibility.Collapsed;
            _sidePanel.Children.Clear();
            new HelpPaneViewModel(_sidePanel).Start();
            _sidePanel.Visibility = Visibility.Visible;
        }

        public void ShowNavigation()
        {
            _sidePanel.Visibility = Visibility.Collapsed;
            _sidePanel.Children.Clear();
            new NavigationSideViewModel(_sidePanel).Start();
            _sidePanel.Visibility = Visibility.Visible;
        }

        public void ShowCurrentDashboard()
        {
            _sidePanel.Visibility = Visibility.Collapsed;
            Load(_tileGrid, _config.Configurations[_dashboardIndex]);
        }
    }
}