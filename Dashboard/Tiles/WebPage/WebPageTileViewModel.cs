using System;
using System.Windows.Controls;
using System.Windows.Input;
using Dashboard.Config;
using Dashboard.Config.Parameters;
using Dashboard.Framework.Config.Commands;
using NoeticTools.Dashboard.Framework.Config;

namespace NoeticTools.TeamDashboard.Tiles.WebPage
{
    internal class WebPageTileViewModel : ITileViewModel
    {
        public static readonly string TileTypeId = "WebBrowser";
        private readonly TileConfiguration _tileConfiguration;
        private NoeticTools.TeamDashboard.Tiles.WebPage.WebPageTileControl _view;

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
            _view = new NoeticTools.TeamDashboard.Tiles.WebPage.WebPageTileControl {DataContext = this};
            placeholderPanel.Children.Add(_view);

            UpdateView();
        }

        public string TypeId
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