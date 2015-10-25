using System.Windows.Controls;
using Dashboard.Framework.Config;
using Dashboard.Tiles.HelpTile;
using Dashboard.Tiles.NavigationTile;
using NoeticTools.Dashboard.Framework;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.DataSources.TeamCity;

namespace NoeticTools.TeamDashboard
{
    public class DashboardController
    {
        private readonly DashboardConfigurations _config;
        private readonly DashboardConfigurationManager _configurationManager;
        private readonly RunOptions _runOptions;
        private int _dashboardIndex = 0;
        private Grid _tileGrid;

        public DashboardController(DashboardConfigurationManager configurationManager, RunOptions runOptions)
        {
            _configurationManager = configurationManager;
            _runOptions = runOptions;
            _config = new DashboardConfigurationManager().Load();
        }

        public void Start(Grid tileGrid)
        {
            _tileGrid = tileGrid;
            Load(tileGrid, _config.Configurations[_dashboardIndex]);
        }

        private void Load(Grid tileGrid, DashboardConfiguration activeConfiguration)
        {
            ClearGrid();

            var channel = new TeamCityService(_config.Services, _runOptions);
            var layout = new TileLayoutController(tileGrid, channel, activeConfiguration);

            foreach (var tile in activeConfiguration.RootTile.Tiles)
            {
                layout.AddTile(tile);
            }
        }

        private void ClearGrid()
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
            if (++_dashboardIndex >= _config.Configurations.Length)
            {
                _dashboardIndex = 0;
            }
            Load(_tileGrid, _config.Configurations[_dashboardIndex]);
        }

        public void PrevDashboard()
        {
            if (--_dashboardIndex < 0)
            {
                _dashboardIndex = _config.Configurations.Length-1;
            }
            Load(_tileGrid, _config.Configurations[_dashboardIndex]);
        }

        public void ShowFirstDashboard()
        {
            _dashboardIndex = 0;
            Load(_tileGrid, _config.Configurations[_dashboardIndex]);
        }

        public void ShowLastDashboard()
        {
            _dashboardIndex = _config.Configurations.Length - 1;
            Load(_tileGrid, _config.Configurations[_dashboardIndex]);
        }

        public void ShowHelp()
        {
            ClearGrid();
            new HelpTileViewModel().Start(_tileGrid);
        }

        public void ShowNavigation()
        {
            ClearGrid();
            new NavigationTileViewModel().Start(_tileGrid);
        }

        public void ShowCurrentDashboard()
        {
            Load(_tileGrid, _config.Configurations[_dashboardIndex]);
        }
    }
}