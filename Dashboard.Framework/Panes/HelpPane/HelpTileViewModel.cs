using System.Windows.Controls;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Config.Commands;

namespace NoeticTools.Dashboard.Framework.Panes.HelpPane
{
    public class HelpTileViewModel : IPaneViewModel
    {
        private HelpTileControl _view;

        public HelpTileViewModel()
        {
            Save = new NullCommand();
        }

        public UserControl Show()
        {
            _view = new HelpTileControl { DataContext = this};
            UpdateView();
            return _view;
        }

        public ICommand Save { get; }

        private void UpdateView()
        {
            _view.message.Text =
                $@"Dashboard version {GetType().Assembly.GetName().Version
                    }.

Esc - Show current dashboard.
F1 - Show this help pane.
F3 - Show dashboards navigation pane.
F5 - Refresh all tiles.
PageUp - Show previous dashboard.
PageDown - Show next dashboard.
Home - Show first dashboard.
End - Show last dashboard.

Click on a tile to configure the tile.
";
        }
    }
}