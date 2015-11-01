﻿using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Config.Commands;

namespace NoeticTools.Dashboard.Framework.Tiles.Dashboards
{
    public class DashboardsNavigationTileViewModel : NotifyingViewModelBase, ITileViewModel
    {
        private readonly IDashboardNavigator _dashboardNavigator;
        private int _dashboardIndex;

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

        public DashboardsNavigationTileViewModel(DashboardConfigurations config, IDashboardNavigator dashboardNavigator)
        {
            _dashboardNavigator = dashboardNavigator;
            DashboardIndex = _dashboardNavigator.CurrentDashboardIndex;
            Close = new NullCommand();
            Items = new ObservableCollection<DashboardConfiguration>(config.Configurations);
            ConfigureCommand = new NullCommand();
        }

        public FrameworkElement CreateView()
        {
            var view = new DashboardNavigationTileControl();
            Close = new CloseCommand(view);
            view.DataContext = this;
            return view;
        }

        public ICommand ConfigureCommand { get; }

        public ObservableCollection<DashboardConfiguration> Items { get; }

        public ICommand Close { get; private set; }

        public void OnConfigurationChanged()
        {
        }
    }
}