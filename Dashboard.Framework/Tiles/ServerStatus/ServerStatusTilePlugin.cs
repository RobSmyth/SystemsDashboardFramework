using System;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Tiles.Date;


namespace NoeticTools.Dashboard.Framework.Tiles.ServerStatus
{
    public class ServerStatusTilePlugin : IPlugin, ITileControllerProvider
    {
        private readonly IDashboardController _dashboardController;

        public ServerStatusTilePlugin(IDashboardController dashboardController)
        {
            _dashboardController = dashboardController;
        }

        public bool MatchesId(string id)
        {
            return id == ServerStatusTileController.TileTypeId || id.Equals("0FFACE9A-8B68-4DBC-8B42-0255F51368B4", StringComparison.InvariantCultureIgnoreCase);
        }

        public IViewController CreateTileController(TileConfiguration tileConfiguration)
        {
            return new ServerStatusTileController(tileConfiguration);
        }

        public void Register(IServices services)
        {
            services.TileProviderRegistry.Register(this);
        }

        public string Name => "Server status";
    }
}