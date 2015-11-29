using System;
using System.Windows;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Config.Commands;
using NoeticTools.Dashboard.Framework.Plugins.Tiles.Date;
using NoeticTools.Dashboard.Framework.Tiles;
using NoeticTools.Dashboard.Framework.Time;


namespace NoeticTools.Dashboard.Framework.Plugins.Tiles.BlankTile
{
    internal class BlankTileController : IViewController
    {
        public const string TileTypeId = "Blank.Tile";
        private BlankTileControl _view;

        public BlankTileController()
        {
            ConfigureCommand = new NullCommand();
        }

        public ICommand ConfigureCommand { get; }

        public FrameworkElement CreateView()
        {
            _view = new BlankTileControl();
            UpdateView();
            return _view;
        }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
        }

        private void UpdateView()
        {
        }
    }
}