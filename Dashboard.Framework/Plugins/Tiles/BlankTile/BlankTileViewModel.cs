using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using NoeticTools.Dashboard.Framework.Commands;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Config.Properties;


namespace NoeticTools.Dashboard.Framework.Plugins.Tiles.BlankTile
{
    public class BlankTileViewModel : NotifyingViewModelBase, IConfigurationChangeListener
    {
        private Brush _background;
        private readonly TileConfigurationConverter _tileConfigurationConverter;

        public BlankTileViewModel(TileConfiguration tile, IDashboardController dashboardController, TileLayoutController layoutController, IServices services)
        {
            _tileConfigurationConverter = new TileConfigurationConverter(tile, this);
            var parameters = new IPropertyViewModel[] { new PropertyViewModel("Colour", "Text", _tileConfigurationConverter) };
            ConfigureCommand = new TileConfigureCommand(tile, "Blank Tile Configuration", parameters, dashboardController, layoutController, services);
            Update();
        }

        public Brush Background
        {
            get { return _background; }
            private set
            {
                if (value.Equals(_background)) return;
                _background = value;
                OnPropertyChanged();
            }
        }

        public ICommand ConfigureCommand { get; private set; }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
            Update();
        }

        private void Update()
        {
            try
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_tileConfigurationConverter.GetString("Colour")));
            }
            catch (Exception)
            {
                Background = Brushes.Red;
            }
        }
    }
}
