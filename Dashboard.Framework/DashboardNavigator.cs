using System.Windows;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Tiles;

namespace NoeticTools.Dashboard.Framework
{
    public class DashboardNavigator : IDashboardNavigator
    {
        private readonly IDashBoardLoader _dashboardLoader;
        private readonly DashboardConfigurations _config;
        private readonly ITileRegistry _tileRegistry;
        private readonly ITileLayoutControllerRegistry _layoutControllerRegistry;

        public DashboardNavigator(IDashBoardLoader dashboardLoader, DashboardConfigurations config, ITileRegistry tileRegistry, ITileLayoutControllerRegistry layoutControllerRegistry)
        {
            _dashboardLoader = dashboardLoader;
            _config = config;
            _tileRegistry = tileRegistry;
            _layoutControllerRegistry = layoutControllerRegistry;
        }   

        public int CurrentDashboardIndex { get; private set; } = 0;

        public void ShowDashboard(int index)
        {
            CurrentDashboardIndex = index;
            _dashboardLoader.Load(_config.Configurations[CurrentDashboardIndex]);
        }

        public void ToggleShowPanesMode()
        {
            foreach (var layoutController in _layoutControllerRegistry.GetAll())
            {
                layoutController.ToggleDisplayMode();
            }
        }

        public void NextDashboard()
        {
            if (++CurrentDashboardIndex >= _config.Configurations.Length)
            {
                CurrentDashboardIndex = 0;
            }
            _dashboardLoader.Load(_config.Configurations[CurrentDashboardIndex]);
        }

        public void PrevDashboard()
        {
            if (--CurrentDashboardIndex < 0)
            {
                CurrentDashboardIndex = _config.Configurations.Length - 1;
            }
            _dashboardLoader.Load(_config.Configurations[CurrentDashboardIndex]);
        }

        public void ShowFirstDashboard()
        {
            CurrentDashboardIndex = 0;
            _dashboardLoader.Load(_config.Configurations[CurrentDashboardIndex]);
        }

        public void ShowLastDashboard()
        {
            CurrentDashboardIndex = _config.Configurations.Length - 1;
            _dashboardLoader.Load(_config.Configurations[CurrentDashboardIndex]);
        }

        public void ShowCurrentDashboard()
        {
            _dashboardLoader.Load(_config.Configurations[CurrentDashboardIndex]);
        }
    }
}