using System.Windows;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Tiles.Guages.GuageAngular
{
    internal sealed class GuageAngularTileProvider : ITileControllerProvider
    {
        private readonly IDashboardController _dashboardController;
        private readonly IServices _services;
        private static readonly string TileTypeId = "GuageAngular";

        public GuageAngularTileProvider(IDashboardController dashboardController, IServices services)
        {
            _dashboardController = dashboardController;
            _services = services;
        }

        public string Name => "Angular Guage";

        public string TypeId => TileTypeId;

        public bool MatchesId(string id)
        {
            return id == TileTypeId;
        }

        public FrameworkElement CreateTile(TileConfiguration tileConfigturation, TileLayoutController layoutController)
        {
            var tile = new GuageAngularTileControl();
            var conduit = new ConfigurationChangeListenerConduit();
            var tileProperties = new TileProperties(tileConfigturation, conduit, _services);
            var viewModel = new GuageAngularTileViewModel(tileConfigturation, _dashboardController, layoutController, _services, tileProperties);
            conduit.SetTarget(viewModel);
            tile.SetModel(viewModel);
            return tile;
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