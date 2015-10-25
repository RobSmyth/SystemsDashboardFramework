using System;
using System.Windows.Controls;
using System.Windows.Input;
using Dashboard.Framework.Config.Commands;

namespace NoeticTools.TeamDashboard.Panes.NavigationPane
{
    internal class NavigationSideViewModel
    {
        private readonly DockPanel _placeholderPanel;
        private NavigationPaneControl _view;

        public NavigationSideViewModel(DockPanel placeholderPanel)
        {
            _placeholderPanel = placeholderPanel;
        }

        public void Start()
        {
            _view = new NavigationPaneControl() { DataContext = this};
            _placeholderPanel.Children.Add(_view);

            UpdateView();
        }

        private void UpdateView()
        {
        }
    }
}