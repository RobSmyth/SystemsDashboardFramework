using System.Windows.Controls;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Config.Commands;

namespace NoeticTools.Dashboard.Framework.Panes.NavigationPane
{
    public class NavigationSideViewModel : IPaneViewModel
    {
        private NavigationPaneControl _view;

        public NavigationSideViewModel()
        {
            Save = new NullCommand();
        }

        public UserControl Show()
        {
            return new NavigationPaneControl() { DataContext = this};
        }

        public ICommand Save { get; }
    }
}