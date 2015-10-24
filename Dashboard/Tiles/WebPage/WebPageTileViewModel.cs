using System;
using System.Windows.Controls;
using System.Windows.Input;
using Dashboard.Config;
using Dashboard.Config.Parameters;
using Dashboard.Framework.Config.Commands;

namespace Dashboard.Tiles.WebPage
{
    internal class WebPageTileViewModel : ITileViewModel
    {
        public static readonly Guid TileTypeId = new Guid("{92CE0D61-4748-4427-8EB7-DC8B8B741C15}");
        private readonly TileConfiguration _tileConfiguration;
        private WebPage.WebPageTileControl _view;

        public WebPageTileViewModel(DashboardTileConfiguration tileConfiguration)
        {
            _tileConfiguration = new TileConfiguration(tileConfiguration, this);
            Id = tileConfiguration.Id;
        }

        public ICommand ConfigureCommand { get; private set; }

        public void Start(Panel placeholderPanel)
        {
            ConfigureCommand = new TileConfigureCommand("Web Page Tile Configuration", _tileConfiguration, new[]
            {
                new ConfigurationParameter("Url", "http://www.google.com", _tileConfiguration)
            });
            _view = new WebPage.WebPageTileControl {DataContext = this};
            placeholderPanel.Children.Add(_view);

            UpdateView();
        }

        public Guid TypeId
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
            try
            {
                _view.webBrowser.Source = new Uri(_tileConfiguration.GetString("Url"));
            }
            catch (Exception)
            {
                _view.webBrowser.Source = new Uri("http://www.google.com");
            }
        }
    }
}