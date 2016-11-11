using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using log4net;
using NoeticTools.TeamStatusBoard.Common.ViewModels;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Persistence.Xml;


namespace NoeticTools.TeamStatusBoard.Tiles.Image
{
    internal sealed class ImageFileWatcherViewModel : NotifyingViewModelBase, IConfigurationChangeListener, ITileViewModel
    {
        private readonly ImageFileWatcherTileControl _view;
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private readonly FileSystemWatcher _fileWatcher;
        private readonly ILog _logger;
        private ImageSource _source;
        private string _imageFilePath = string.Empty;
        private static int _nextInstanceId = 1;

        public ImageFileWatcherViewModel(TileConfiguration tile, IDashboardController dashboardController, ITileLayoutController tileLayoutController, IServices services, ImageFileWatcherTileControl view)
        {
            _view = view;
            _tileConfigurationConverter = new TileConfigurationConverter(tile, this);
            var parameters = new IPropertyViewModel[] {new TextPropertyViewModel("ImagePath", _tileConfigurationConverter, services)};
            ConfigureCommand = new TileConfigureCommand(tile, "Image Tile Configuration", parameters, dashboardController, tileLayoutController, services);
            _logger = LogManager.GetLogger($"Tiles.Image.File.Watcher.{_nextInstanceId++}");
            _fileWatcher = new FileSystemWatcher
            {
                IncludeSubdirectories = false,
                EnableRaisingEvents = false
            };
            _fileWatcher.Changed += OnFileChanged;
            _fileWatcher.Deleted += OnFileChanged;
            _fileWatcher.Created += OnFileChanged;
            _fileWatcher.Renamed += OnFileChanged;
            _view.DataContext = this;
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

        public ICommand ConfigureCommand { get; }

        public void OnConfigurationChanged(INamedValueRepository converter)
        {
            Update();
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            _view.Dispatcher.InvokeAsync(Update);
        }

        private void Update()
        {
            try
            {
                Thread.Sleep(50);
                var imageFilePath = _tileConfigurationConverter.GetString("ImagePath").Trim('"');

                _logger.Info($"Setting image source to {imageFilePath}");
                _imageFilePath = imageFilePath;

                if (File.Exists(imageFilePath))
                {
                    using (var stream = File.Open(imageFilePath, FileMode.Open))
                    {
                        var image = (Bitmap) System.Drawing.Image.FromStream(stream);

                        using (var memory = new MemoryStream())
                        {
                            image.Save(memory, ImageFormat.Png);
                            memory.Position = 0;
                            var bitmapImage = new BitmapImage();
                            bitmapImage.BeginInit();
                            bitmapImage.StreamSource = memory;
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.EndInit();
                            Source = bitmapImage;
                        }

                        stream.Close();
                    }
                }
                else
                {
                    Source = new BitmapImage();
                }

                _fileWatcher.EnableRaisingEvents = false;
                _fileWatcher.Path = Path.GetDirectoryName(_imageFilePath);
                _fileWatcher.Filter = Path.GetFileName(_imageFilePath);
                _fileWatcher.EnableRaisingEvents = true;
            }
            catch (Exception)
            {
            }
        }
    }
}