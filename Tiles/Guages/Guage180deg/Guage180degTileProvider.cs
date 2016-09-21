using System.Windows;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Tiles.Guages.Guage180deg
{
    internal sealed class Guage180degTileProvider : ITileControllerProvider
    {
        private const string TileTypeId = "Guage180deg";
        private readonly IDashboardController _dashboardController;
        private readonly IServices _services;

        public Guage180degTileProvider(IDashboardController dashboardController, IServices services)
        {
            _dashboardController = dashboardController;
            _services = services;
        }

        public string Name => "180/360 deg Guage";

        public string TypeId => TileTypeId;

        public bool MatchesId(string id)
        {
            return id == TileTypeId;
        }

        public FrameworkElement CreateTile(TileConfiguration tileConfigturation, TileLayoutController layoutController)
        {
            var conduit = new ConfigurationChangeListenerConduit();
            var tileProperties = new TileProperties(tileConfigturation, conduit, _services);
            var viewModel = new Guage180DegTileViewModel(tileConfigturation, _dashboardController, layoutController, _services, tileProperties);
            conduit.SetTarget(viewModel);
            return new Guage180degTileControl {DataContext = viewModel};
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