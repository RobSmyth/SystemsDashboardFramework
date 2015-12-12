using System;
using System.Windows.Input;
using NoeticTools.SystemsDashboard.Framework.Commands;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Config.Properties;
using NoeticTools.SystemsDashboard.Framework.Tiles.WebPage;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.WebPage
{
    internal class WebPageTileViewModel : IConfigurationChangeListener
    {
        public static readonly string TileTypeId = "WebBrowser";
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private readonly WebPageTileControl _view;

        public WebPageTileViewModel(TileConfiguration tile, IDashboardController dashboardController, TileLayoutController layoutController, IServices services, WebPageTileControl view)
        {
            _view = view;
            _tileConfigurationConverter = new TileConfigurationConverter(tile, this);
            ConfigureCommand = new TileConfigureCommand(tile, "Web Page Tile Configuration", new[]
            {
                new PropertyViewModel("Url", "Text", _tileConfigurationConverter)
            },
                dashboardController, layoutController, services);
            _view.DataContext = this;
            UpdateView();
        }

        public ICommand ConfigureCommand { get; private set; }

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