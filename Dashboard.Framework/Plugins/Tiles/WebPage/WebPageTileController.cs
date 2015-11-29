using System;
using System.Windows;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Commands;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Config.Parameters;
using NoeticTools.Dashboard.Framework.Tiles;
using NoeticTools.Dashboard.Framework.Tiles.WebPage;


namespace NoeticTools.Dashboard.Framework.Plugins.Tiles.WebPage
{
    internal class WebPageTileController : IViewController
    {
        private readonly TileConfiguration _tile;
        private readonly IDashboardController _dashboardController;
        private readonly TileLayoutController _tileLayoutController;
        public static readonly string TileTypeId = "WebBrowser";
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private WebPageTileControl _view;

        public WebPageTileController(TileConfiguration tile, IDashboardController dashboardController, TileLayoutController tileLayoutController)
        {
            _tile = tile;
            _dashboardController = dashboardController;
            _tileLayoutController = tileLayoutController;
            _tileConfigurationConverter = new TileConfigurationConverter(tile, this);
        }

        public ICommand ConfigureCommand { get; private set; }

        public FrameworkElement CreateView()
        {
            ConfigureCommand = new TileConfigureCommand(_tile, "Web Page Tile Configuration", new[]
            {
                new ElementViewModel("Url", ElementType.Text, _tileConfigurationConverter
/* "http://www.google.com"*/)
            },
                _dashboardController, _tileLayoutController);
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