using System;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NoeticTools.SystemsDashboard.Framework.Commands;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Config.Properties;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.Image
{
    internal sealed class ImageViewModel : NotifyingViewModelBase, IConfigurationChangeListener
    {
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private ImageSource _source;

        public ImageViewModel(TileConfiguration tile, IDashboardController dashboardController, TileLayoutController tileLayoutController, IServices services)
        {
            _tileConfigurationConverter = new TileConfigurationConverter(tile, this);
            var parameters = new IPropertyViewModel[] {new PropertyViewModel("ImagePath", "Text", _tileConfigurationConverter)};
            ConfigureCommand = new TileConfigureCommand(tile, "Image Tile Configuration", parameters, dashboardController, tileLayoutController, services);
            Update();
        }

        public ImageSource Source
        {
            get { return _source; }
            private set
            {
                if (value == _source) return;
                _source = value;
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
                var imageFilePath = _tileConfigurationConverter.GetString("ImagePath");
                Source = new BitmapImage(new Uri(imageFilePath));
            }
            catch (Exception exception)
            {
            }
        }
    }
}