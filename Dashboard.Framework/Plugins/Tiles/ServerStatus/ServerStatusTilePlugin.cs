﻿using System;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Tiles;
using NoeticTools.Dashboard.Framework.Tiles.ServerStatus;


namespace NoeticTools.Dashboard.Framework.Plugins.Tiles.ServerStatus
{
    public class ServerStatusTilePlugin : IPlugin, ITileControllerProvider
    {
        public int Rank => 0;

        public string Name => "Server status (future)";

        public bool MatchesId(string id)
        {
            return id == ServerStatusTileController.TileTypeId || id.Equals("0FFACE9A-8B68-4DBC-8B42-0255F51368B4", StringComparison.InvariantCultureIgnoreCase);
        }

        public IViewController CreateTileController(TileConfiguration tile, TileLayoutController layoutController)
        {
            return new ServerStatusTileController(tile);
        }

        public TileConfiguration CreateDefaultConfiguration()
        {
            throw new NotImplementedException();
        }

        public void Register(IServices services)
        {
            services.TileProviders.Register(this);
        }
    }
}