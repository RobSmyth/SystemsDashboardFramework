using System;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Tiles;
using NoeticTools.Dashboard.Framework.Tiles.ServerStatus;


namespace NoeticTools.Dashboard.Framework.Plugins.Tiles.ServerStatus
{
    public class ServerStatusTilePlugin : IPlugin, ITileControllerProvider
    {
        private readonly IDashboardController _dashboardController;

        public ServerStatusTilePlugin(IDashboardController dashboardController)
        {
            _dashboardController = dashboardController;
        }

        public string Name => "Server status (future)";

        public bool MatchesId(string id)
        {
            return id == ServerStatusTileController.TileTypeId || id.Equals("0FFACE9A-8B68-4DBC-8B42-0255F51368B4", StringComparison.InvariantCultureIgnoreCase);
        }

        public IViewController CreateTileController(TileConfiguration tileConfiguration, TileLayoutController tileLayoutController)
        {
            return new ServerStatusTileController(tileConfiguration);
        }

        public TileConfiguration CreateDefaultConfiguration()
        {
            throw new NotImplementedException();
        }

        public void Register(IServices services)
        {
            services.TileProviderRegistry.Register(this);
        }
    }
}