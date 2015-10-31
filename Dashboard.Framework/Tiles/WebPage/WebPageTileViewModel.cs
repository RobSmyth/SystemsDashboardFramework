using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Config.Commands;
using NoeticTools.Dashboard.Framework.Config.Parameters;
using NoeticTools.Dashboard.Framework.Tiles;

namespace NoeticTools.TeamDashboard.Tiles.WebPage
{
    internal class WebPageTileViewModel : ITileViewModel
    {
        private readonly IDashboardController _dashboardController;
        public static readonly string TileTypeId = "WebBrowser";
        private readonly TileConfiguration _tileConfiguration;
        private WebPageTileControl _view;

        public WebPageTileViewModel(DashboardTileConfiguration tileConfiguration, IDashboardController dashboardController)
        {
            _dashboardController = dashboardController;
            _tileConfiguration = new TileConfiguration(tileConfiguration, this);
        }

        public ICommand ConfigureCommand { get; private set; }

        public FrameworkElement CreateView()
        {
            ConfigureCommand = new TileConfigureCommand("Web Page Tile Configuration", _tileConfiguration, new[]
            {
                new ConfigurationParameter("Url", "http://www.google.com", _tileConfiguration)
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
                _view.webBrowser.Source = new Uri(_tileConfiguration.GetString("Url"));
            }
            catch (Exception)
            {
                _view.webBrowser.Source = new Uri("http://www.google.com");
            }
        }
    }
}