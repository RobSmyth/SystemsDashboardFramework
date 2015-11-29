using System;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Tiles;


namespace NoeticTools.Dashboard.Framework.Plugins.Tiles.Message
{
    public class MessageTilePlugin : IPlugin, ITileControllerProvider
    {
        private readonly IDashboardController _dashboardController;

        public MessageTilePlugin(IDashboardController dashboardController)
        {
            _dashboardController = dashboardController;
        }

        public string Name => "Message";

        public bool MatchesId(string id)
        {
            return id == MessageTileController.TileTypeId || id.Equals("0FFACE9A-8B68-4DBC-8B42-0255F51368B3", StringComparison.InvariantCultureIgnoreCase);
        }

        public IViewController CreateTileController(TileConfiguration tileConfiguration, TileLayoutController tileLayoutController)
        {
            return new MessageTileController(tileConfiguration, _dashboardController, tileLayoutController);
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
            services.TileProviderRegistry.Register(this);
        }
    }
}