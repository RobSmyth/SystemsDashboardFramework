using System;
using NoeticTools.Dashboard.Framework.Config;


namespace NoeticTools.Dashboard.Framework.Plugins.Tiles.MessageTile
{
    internal sealed class MessageTilePlugin : IPlugin, ITileControllerProvider
    {
        private readonly IDashboardController _dashboardController;
        private readonly IServices _services;

        public MessageTilePlugin(IDashboardController dashboardController, IServices services)
        {
            _dashboardController = dashboardController;
            _services = services;
        }

        public int Rank => 0;

        public string Name => "Message";

        public bool MatchesId(string id)
        {
            return id == MessageTileController.TileTypeId || id.Equals("0FFACE9A-8B68-4DBC-8B42-0255F51368B3", StringComparison.InvariantCultureIgnoreCase);
        }

        public IViewController CreateTileController(TileConfiguration tile, TileLayoutController layoutController)
        {
            return new MessageTileController(tile, _dashboardController, layoutController, _services);
        }

        public TileConfiguration CreateDefaultConfiguration()
        {
            return new TileConfiguration
            {
                TypeId = MessageTileController.TileTypeId,
                Id = Guid.NewGuid(),
                Tiles = new TileConfiguration[0]
            };
        }

        public void Register(IServices services)
        {
            services.TileProviders.Register(this);
        }
    }
}