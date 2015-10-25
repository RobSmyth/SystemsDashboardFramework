using System;
using System.Windows.Controls;
using System.Windows.Input;
using Dashboard.Framework.Config.Commands;
using Dashboard.Tiles.Message;

namespace NoeticTools.TeamDashboard.Panes.HelpPane
{
    internal class HelpPaneViewModel
    {
        private readonly Panel _placeholderPanel;
        private HelpPaneControl _view;

        public HelpPaneViewModel(Panel placeholderPanel)
        {
            _placeholderPanel = placeholderPanel;
        }

        public void Start()
        {
            _view = new HelpPaneControl { DataContext = this};
            _placeholderPanel.Children.Add(_view);
            _view.Width = Double.NaN;
            _view.Height = Double.NaN;

            UpdateView();
        }

        private void UpdateView()
        {
            _view.message.Text = string.Format(
                @"Dashboard version {0}.

Esc - Show current dashboard.
F1 - Show this help.
F3 - Show navigation.
PageUp - Show previous dashboard.
PageDown - Show next dashboard.
Home - Show first dashboard.
End - Show last dashboard.

Click on a tile to configure the tile.
", GetType().Assembly.GetName().Version);
        }
    }
}