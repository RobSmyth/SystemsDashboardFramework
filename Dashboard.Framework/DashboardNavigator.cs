using System.Windows;
using NoeticTools.Dashboard.Framework.Config;

namespace NoeticTools.Dashboard.Framework
{
    public class DashboardNavigator : IDashboardNavigator
    {
        private readonly IDashBoardLoader _dashboardLoader;
        private readonly DashboardConfigurations _config;

        public DashboardNavigator(IDashBoardLoader dashboardLoader, DashboardConfigurations config)
        {
            _dashboardLoader = dashboardLoader;
            _config = config;
        }   

        public int CurrentDashboardIndex { get; private set; } = 0;

        public void ShowDashboard(int index)
        {
            CurrentDashboardIndex = index;
            _dashboardLoader.Load(_config.Configurations[CurrentDashboardIndex]);
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