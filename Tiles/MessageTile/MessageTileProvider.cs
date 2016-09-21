using System;
using System.Windows;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Tiles.MessageTile
{
    internal sealed class MessageTileProvider : ITileControllerProvider
    {
        private readonly IDashboardController _dashboardController;
        private readonly IServices _services;
        private static readonly string TileTypeId = "Message";

        public MessageTileProvider(IDashboardController dashboardController, IServices services)
        {
            _dashboardController = dashboardController;
            _services = services;
        }

        public string Name => "Message";

        public string TypeId => TileTypeId;

        public bool MatchesId(string id)
        {
            return id == TileTypeId || id.Equals("0FFACE9A-8B68-4DBC-8B42-0255F51368B3", StringComparison.InvariantCultureIgnoreCase);
        }

        public FrameworkElement CreateTile(TileConfiguration tileConfigturation, TileLayoutController layoutController)
        {
            var conduit = new ConfigurationChangeListenerConduit();
            var tileProperties = new TileProperties(tileConfigturation, conduit, _services);
            var viewModel = new MessageTileViewModel(tileConfigturation, _dashboardController, layoutController, _services, tileProperties);
            conduit.SetTarget(viewModel);
            return new MessageTileControl {DataContext = viewModel};
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