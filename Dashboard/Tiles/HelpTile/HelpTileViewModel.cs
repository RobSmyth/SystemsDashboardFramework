using System;
using System.Windows.Controls;
using System.Windows.Input;
using Dashboard.Framework.Config.Commands;
using Dashboard.Tiles.Message;

namespace Dashboard.Tiles.HelpTile
{
    internal class HelpTileViewModel : ITileViewModel
    {
        public static readonly Guid TileTypeId = new Guid("{14B30D27-DC55-466A-A7C7-835004FF94A6}");
        private HelpTileControl _view;

        public HelpTileViewModel()
        {
            Id = Guid.Empty;
        }

        public ICommand ConfigureCommand { get; private set; }

        public void Start(Panel placeholderPanel)
        {
            ConfigureCommand = new NullCommand();
            _view = new HelpTileControl { DataContext = this};
            placeholderPanel.Children.Add(_view);

            UpdateView();
        }

        public Guid TypeId
        {
            get { return TileTypeId; }
        }

        public Guid Id { get; private set; }

        public void OnConfigurationChanged()
        {
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