using System;
using System.Windows.Controls;
using System.Windows.Input;
using Dashboard.Framework.Config.Commands;
using Dashboard.Tiles.Message;

namespace Dashboard.Tiles.NavigationTile
{
    internal class NavigationTileViewModel : ITileViewModel
    {
        public static readonly Guid TileTypeId = new Guid("{49D4EBB4-3C2E-44A6-AF42-FC9EDCDE9D95}");
        private NavigationTileControl _view;

        public NavigationTileViewModel()
        {
            Id = Guid.Empty;
        }

        public ICommand ConfigureCommand { get; private set; }

        public void Start(Panel placeholderPanel)
        {
            ConfigureCommand = new NullCommand();
            _view = new NavigationTileControl() { DataContext = this};
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
        }
    }
}