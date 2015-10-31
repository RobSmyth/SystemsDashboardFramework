using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Config.Commands;
using NoeticTools.Dashboard.Framework.Config.ViewModels;

namespace NoeticTools.Dashboard.Framework.Panes.NavigationPane
{
    public class DashboardsNavigationTileViewModel : NotifyingViewModelBase, IPaneViewModel
    {
        private readonly IDashboardController _dashboardController;
        private int _dashboardIndex;

        public int DashboardIndex
        {
            get { return _dashboardIndex; }
            set
            {
                if (value == _dashboardIndex) return;
                _dashboardIndex = value;
                OnPropertyChanged();
            }
        }

        public DashboardsNavigationTileViewModel(DashboardConfigurations config, IDashboardController dashboardController)
        {
            _dashboardController = dashboardController;
            DashboardIndex = dashboardController.DashboardIndex;
            Close = new NullCommand();
            Items = new ObservableCollection<DashboardConfiguration>(config.Configurations);
            PropertyChanged += NavigationPaneViewModel_PropertyChanged;
        }

        private void NavigationPaneViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (DashboardIndex != _dashboardController.DashboardIndex)
            {
                _dashboardController.ShowDashboard(DashboardIndex);
            }
        }

        public UserControl Show()
        {
            var view = new DashboardNavigationTileControl();
            Close = new CloseCommand(view);
            view.DataContext = this;
            return view;
        }

        public ObservableCollection<DashboardConfiguration> Items { get; }

        public ICommand Close { get; private set; }
    }
}