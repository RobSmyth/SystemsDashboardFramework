using System;
using System.Windows;
using System.Windows.Input;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Config.Properties;
using NoeticTools.SystemsDashboard.Framework.Tiles.WebPage;
using NoeticTools.SystemsDashboard.Framework.Commands;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.WebPage
{
    internal class WebPageTileController : IViewController
    {
        private readonly IDashboardController _dashboardController;
        public static readonly string TileTypeId = "WebBrowser";
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private readonly TileLayoutController _layoutController;
        private readonly IServices _services;
        private WebPageTileControl _view;

        public WebPageTileController(TileConfiguration tile, IDashboardController dashboardController, TileLayoutController tileLayoutController, IServices services)
        {
            Tile = tile;
            _dashboardController = dashboardController;
            _layoutController = tileLayoutController;
            _services = services;
            _tileConfigurationConverter = new TileConfigurationConverter(tile, this);
        }

        public ICommand ConfigureCommand { get; private set; }

        public TileConfiguration Tile { get; }

        public FrameworkElement CreateView()
        {
            ConfigureCommand = new TileConfigureCommand(Tile, "Web Page Tile Configuration", new[]
            {
                new PropertyViewModel("Url", "Text", _tileConfigurationConverter
/* "http://www.google.com"*/)
            },
                _dashboardController, _layoutController, _services);
            _view = new WebPageTileControl {DataContext = this};

            UpdateView();
            return _view;
        }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
            UpdateView();
        }

        private void UpdateView()
        {
            try
            {
                _view.webBrowser.Source = new Uri(_tileConfigurationConverter.GetString("Url"));
            }
            catch (Exception)
            {
                _view.webBrowser.Source = new Uri("http://www.google.com");
            }
        }
    }
}