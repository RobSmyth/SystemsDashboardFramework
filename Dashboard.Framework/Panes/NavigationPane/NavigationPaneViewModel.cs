using System.Windows.Controls;

namespace NoeticTools.Dashboard.Framework.Panes.NavigationPane
{
    public class NavigationSideViewModel : IPaneViewModel
    {
        private readonly DockPanel _placeholderPanel;
        private NavigationPaneControl _view;

        public NavigationSideViewModel(DockPanel placeholderPanel)
        {
            _placeholderPanel = placeholderPanel;
        }

        public void Show()
        {
            _view = new NavigationPaneControl() { DataContext = this};
            _placeholderPanel.Children.Add(_view);
        }
    }
}