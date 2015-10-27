using System.Windows.Controls;

namespace NoeticTools.Dashboard.Framework.Panes.HelpPane
{
    public class HelpPaneViewModel : IPaneViewModel
    {
        private readonly Panel _placeholderPanel;
        private HelpPaneControl _view;

        public HelpPaneViewModel(Panel placeholderPanel)
        {
            _placeholderPanel = placeholderPanel;
        }

        public void Show()
        {
            _view = new HelpPaneControl { DataContext = this};
            _placeholderPanel.Children.Add(_view);
            _view.Width = double.NaN;
            _view.Height = double.NaN;

            UpdateView();
        }

        private void UpdateView()
        {
            _view.message.Text =
                $@"Dashboard version {GetType().Assembly.GetName().Version
                    }.

Esc - Show current dashboard.
F1 - Show this help pane.
F3 - Show navigation pane.
PageUp - Show previous dashboard.
PageDown - Show next dashboard.
Home - Show first dashboard.
End - Show last dashboard.

Click on a tile to configure the tile.
";
        }
    }
}