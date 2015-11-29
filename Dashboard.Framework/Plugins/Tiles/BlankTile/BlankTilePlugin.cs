using System;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Plugins.Tiles.Date;
using NoeticTools.Dashboard.Framework.Tiles;
using NoeticTools.Dashboard.Framework.Time;


namespace NoeticTools.Dashboard.Framework.Plugins.Tiles.BlankTile
{
    public class BlankTilePlugin : IPlugin, ITileControllerProvider
    {
        public string Name => "Blank";

        public void Register(IServices services)
        {
            services.TileProviderRegistry.Register(this);
        }

        public bool MatchesId(string id)
        {
            return id == BlankTileController.TileTypeId;
        }

        public IViewController CreateTileController(TileConfiguration tileConfiguration, TileLayoutController tileLayoutController)
        {
            return new BlankTileController();
        }

        public TileConfiguration CreateDefaultConfiguration()
        {
            return new TileConfiguration
            {
                TypeId = BlankTileController.TileTypeId,
                Id = Guid.NewGuid(),
                Tiles = new TileConfiguration[0]
            };
        }
    }
}