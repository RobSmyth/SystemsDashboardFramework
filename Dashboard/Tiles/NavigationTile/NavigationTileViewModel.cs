using System;
using System.Windows.Controls;
using System.Windows.Input;
using Dashboard.Framework.Config.Commands;
using Dashboard.Tiles.Message;
using NoeticTools.TeamDashboard.Tiles;

namespace Dashboard.Tiles.NavigationTile
{
    internal class NavigationTileViewModel : ITileViewModel
    {
        public static readonly string TileTypeId = "Navigation";
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

        public string TypeId
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