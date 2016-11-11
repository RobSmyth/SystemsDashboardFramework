using NoeticTools.TeamStatusBoard.Framework.Persistence;
using NoeticTools.TeamStatusBoard.Framework.Registries;
using NoeticTools.TeamStatusBoard.Persistence;
using NoeticTools.TeamStatusBoard.Persistence.Xml;


namespace NoeticTools.TeamStatusBoard.Framework.Dashboards
{
    public class DashboardNavigator : IDashboardNavigator
    {
        private readonly IDashBoardLoader _dashboardLoader;
        private readonly DashboardConfigurations _config;
        private readonly ITileLayoutControllerRegistry _layoutControllerRegistry;

        public DashboardNavigator(IDashBoardLoader dashboardLoader, DashboardConfigurations config, ITileLayoutControllerRegistry layoutControllerRegistry)
        {
            _dashboardLoader = dashboardLoader;
            _config = config;
            _layoutControllerRegistry = layoutControllerRegistry;
        }

        public int CurrentDashboardIndex { get; private set; }

        public void ShowDashboard(int index)
        {
            CurrentDashboardIndex = index;
            _dashboardLoader.Load(_config.Configurations[CurrentDashboardIndex]);
        }

        public void ToggleShowGroupPanelsDetailsMode()
        {
            foreach (var layoutController in _layoutControllerRegistry.GetAll())
            {
                layoutController.ToggleShowGroupPanelDetailsMode();
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