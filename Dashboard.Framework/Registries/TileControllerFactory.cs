using System.Linq;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Tiles;


namespace NoeticTools.Dashboard.Framework.Registries
{
    public sealed class TileControllerFactory : ITileControllerFactory
    {
        private readonly TileProviderRegistry _tileProviderRegistry;

        public TileControllerFactory(TileProviderRegistry tileProviderRegistry)
        {
            _tileProviderRegistry = tileProviderRegistry;
        }

        public IViewController Create(TileConfiguration tileConfiguration, TileLayoutController tileLayoutController)
        {
            var plugin = _tileProviderRegistry.GetAll().Single(x => x.MatchesId(tileConfiguration.TypeId));
            var tileViewModel = plugin.CreateTileController(tileConfiguration, tileLayoutController);
            return tileViewModel;
        }
    }
}