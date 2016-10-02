using System;
using System.Windows.Input;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Config.XmlTypes;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Tiles.WebPage
{
    internal class WebPageTileViewModel : IConfigurationChangeListener, ITileViewModel
    {
        public static readonly string TileTypeId = "WebBrowser";
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private readonly WebPageTileControl _view;

        public WebPageTileViewModel(TileConfiguration tile, IDashboardController dashboardController, ITileLayoutController layoutController, IServices services, WebPageTileControl view)
        {
            _view = view;
            _tileConfigurationConverter = new TileConfigurationConverter(tile, this);
            ConfigureCommand = new TileConfigureCommand(tile, "Web Page Tile Configuration", new[]
                {
                    new TextPropertyViewModel("Url", _tileConfigurationConverter, services)
                },
                dashboardController, layoutController, services);
            _view.DataContext = this;
            UpdateView();
        }

        public ICommand ConfigureCommand { get; }

        public void OnConfigurationChanged(INamedValueRepository converter)
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