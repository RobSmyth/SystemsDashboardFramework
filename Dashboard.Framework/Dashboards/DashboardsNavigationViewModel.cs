using System.Collections.ObjectModel;
using NoeticTools.TeamStatusBoard.Common.ViewModels;
using NoeticTools.TeamStatusBoard.Framework.Config;


namespace NoeticTools.TeamStatusBoard.Framework.Dashboards
{
    public class DashboardsNavigationViewModel : NotifyingViewModelBase
    {
        private readonly IDashboardNavigator _dashboardNavigator;
        private int _dashboardIndex;

        public DashboardsNavigationViewModel(DashboardConfigurations config, IDashboardNavigator dashboardNavigator)
        {
            _dashboardNavigator = dashboardNavigator;
            DashboardIndex = _dashboardNavigator.CurrentDashboardIndex;
            Items = new ObservableCollection<DashboardConfiguration>(config.Configurations);
        }

        public int DashboardIndex
        {
            get { return _dashboardIndex; }
            set
            {
                if (value == _dashboardIndex) return;
                _dashboardIndex = value;
                OnPropertyChanged();
                _dashboardNavigator.ShowDashboard(DashboardIndex);
            }
        }

        public ObservableCollection<DashboardConfiguration> Items { get; }
    }
}