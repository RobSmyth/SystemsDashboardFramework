using System;
using System.Windows.Input;
using System.Windows.Media;
using NoeticTools.SystemsDashboard.Framework.Commands;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Config.Properties;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.Date;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.BlankTile
{
    public class BlankTileViewModel : NotifyingViewModelBase, IConfigurationChangeListener, ITileViewModel
    {
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private Brush _background;

        public BlankTileViewModel(TileConfiguration tile, IDashboardController dashboardController, TileLayoutController layoutController, IServices services, BlankTileControl view)
        {
            _tileConfigurationConverter = new TileConfigurationConverter(tile, this);
            var parameters = new IPropertyViewModel[] {new PropertyViewModel("Colour", "Text", _tileConfigurationConverter)};
            ConfigureCommand = new TileConfigureCommand(tile, "Blank Tile Configuration", parameters, dashboardController, layoutController, services);
            view.DataContext = this;
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
                var value = _tileConfigurationConverter.GetString("Colour", "Black");
                Background = new SolidColorBrush((Color) ColorConverter.ConvertFromString(value));
            }
            catch (Exception)
            {
                Background = Brushes.Red;
                _tileConfigurationConverter.SetParameter("Colour", "Red");
            }
        }
    }
}