using System.Windows;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.XmlTypes;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Tiles.DataTiles.TextTile
{
    internal sealed class TextDataTileProvider : ITileControllerProvider
    {
        private readonly IDashboardController _dashboardController;
        private readonly IServices _services;
        private static readonly string TileTypeId = "TextData";

        public TextDataTileProvider(IDashboardController dashboardController, IServices services)
        {
            _dashboardController = dashboardController;
            _services = services;
        }

        public string Name => "Text property panel";

        public string TypeId => TileTypeId;

        public bool MatchesId(string id)
        {
            return id == TileTypeId;
        }

        public FrameworkElement CreateTile(TileConfiguration tileConfigturation, TileLayoutController layoutController)
        {
            var conduit = new ConfigurationChangeListenerConduit();
            var tileProperties = new TileProperties(tileConfigturation, conduit, _services);
            var viewModel = new TextDataTileViewModel(tileConfigturation, _dashboardController, layoutController, _services, tileProperties);
            conduit.SetTarget(viewModel);
            return new TextDataTileControl {DataContext = viewModel};
        }

        public TileConfiguration CreateDefaultConfiguration()
        {
            return new TileConfiguration
            {
                TypeId = TileTypeId,
                Tiles = new TileConfiguration[0]
            };
        }
    }
}