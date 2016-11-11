using System.Windows;
using NoeticTools.TeamStatusBoard.Common.ViewModels;
using NoeticTools.TeamStatusBoard.Framework.Persistence.Xml;


namespace NoeticTools.TeamStatusBoard.Framework.Dashboards
{
    public class DashboardsNavigationViewController : NotifyingViewModelBase
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
    }
}