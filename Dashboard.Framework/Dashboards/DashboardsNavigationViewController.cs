using System.Windows;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Dashboards;


namespace NoeticTools.Dashboard.Framework.Tiles.Dashboards
{
    public class DashboardsNavigationViewController : NotifyingViewModelBase, IViewController
    {
        private readonly DashboardConfigurations _config;
        private readonly IDashboardNavigator _dashboardNavigator;

        public DashboardsNavigationViewController(DashboardConfigurations config, IDashboardNavigator dashboardNavigator)
        {
            _config = config;
            _dashboardNavigator = dashboardNavigator;
        }

        public FrameworkElement CreateView()
        {
            var view = new DashboardNavigationTileControl();
            view.DataContext = new DashboardsNavigationViewModel(_config, _dashboardNavigator);
            return view;
        }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
        }
    }
}