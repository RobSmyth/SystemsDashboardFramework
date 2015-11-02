using System;
using System.Windows;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Commands;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Config.Commands;
using NoeticTools.Dashboard.Framework.Config.Parameters;


namespace NoeticTools.Dashboard.Framework.Tiles.WebPage
{
    internal class WebPageViewController : IViewController
    {
        private readonly IDashboardController _dashboardController;
        public static readonly string TileTypeId = "WebBrowser";
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private WebPageTileControl _view;

        public WebPageViewController(TileConfiguration tileConfiguration,
            IDashboardController dashboardController)
        {
            _dashboardController = dashboardController;
            _tileConfigurationConverter = new TileConfigurationConverter(tileConfiguration, this);
        }

        public ICommand ConfigureCommand { get; private set; }

        public FrameworkElement CreateView()
        {
            ConfigureCommand = new TileConfigureCommand("Web Page Tile Configuration", _tileConfigurationConverter, new[]
            {
                new ConfigurationParameter("Url", "http://www.google.com", _tileConfigurationConverter)
            },
                _dashboardController);
            _view = new WebPageTileControl {DataContext = this};

            UpdateView();
            return _view;
        }

        public void OnConfigurationChanged()
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