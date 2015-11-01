using System.Windows;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Config.Commands;


namespace NoeticTools.Dashboard.Framework.Tiles.Help
{
    public class HelpTileViewModel : ITileViewModel
    {
        private HelpTileControl _view;

        public HelpTileViewModel()
        {
            SaveCommand = new NullCommand();
            ConfigureCommand = new NullCommand();
        }

        public ICommand ConfigureCommand { get; }

        public ICommand SaveCommand { get; }

        public FrameworkElement CreateView()
        {
            _view = new HelpTileControl {DataContext = this};
            UpdateView();
            return _view;
        }

        public void OnConfigurationChanged()
        {
        }

        private void UpdateView()
        {
            _view.message.Text =
                $@"Dashboard version {GetType().Assembly.GetName().Version
                    }.

Esc - Show current dashboard.
F1 - Show this help pane.
F2 - Show tile details/configuration.
F3 - Show dashboards navigation pane.
F4 - Toggle show group panels.
F5 - Refresh all tiles.
PageUp - Show previous dashboard.
PageDown - Show next dashboard.
Home - Show first dashboard.
End - Show last dashboard.
LEFT,RIGT,UP,DOWN - Select tile.

Click on a tile to configure the tile.
";
        }
    }
}