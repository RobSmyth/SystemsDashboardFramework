using System;
using System.Windows;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Persistence.Xml;


namespace NoeticTools.TeamStatusBoard.Tiles.ColouredTile
{
    public sealed class ColouredTileProvider : ITileControllerProvider
    {
        private const string TileTypeId = "Coloured.Tile";
        private readonly IDashboardController _dashboardController;
        private readonly IServices _services;

        public ColouredTileProvider(IDashboardController dashboardController, IServices services)
        {
            _dashboardController = dashboardController;
            _services = services;
        }

        public string TypeId => ColouredTileProvider.TileTypeId;

        public bool MatchesId(string id)
        {
            return id == TileTypeId;
        }

        public FrameworkElement CreateTile(TileConfiguration tileConfigturation, TileLayoutController layoutController)
        {
            var view = new ColouredTileControl();
            var conduit = new ConfigurationChangeListenerConduit();
            var tileProperties = new TileProperties(tileConfigturation, conduit, _services);
            var viewModel = new ColouredTileViewModel(tileConfigturation, _dashboardController, layoutController, _services, view, tileProperties);
            conduit.SetTarget(viewModel);
            return view;
        }

        public TileConfiguration CreateDefaultConfiguration()
        {
            return new TileConfiguration
            {
                TypeId = TileTypeId,
            };
        }
    }
}