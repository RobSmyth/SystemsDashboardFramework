using System.Collections.ObjectModel;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Config.Commands;


namespace NoeticTools.Dashboard.Framework.Tiles.Dashboards
{
    public class DashboardsNavigationViewModel : NotifyingViewModelBase, ICloseableViewModel
    {
        private readonly IDashboardNavigator _dashboardNavigator;
        private int _dashboardIndex;

        public DashboardsNavigationViewModel(DashboardConfigurations config, IDashboardNavigator dashboardNavigator)
        {
            _dashboardNavigator = dashboardNavigator;
            DashboardIndex = _dashboardNavigator.CurrentDashboardIndex;
            CloseCommand = new NullCommand();
            Items = new ObservableCollection<DashboardConfiguration>(config.Configurations);
            ConfigureCommand = new NullCommand();
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

        public ICommand ConfigureCommand { get; }

        public ObservableCollection<DashboardConfiguration> Items { get; }

        public ICommand CloseCommand { get; private set; }

    }
}