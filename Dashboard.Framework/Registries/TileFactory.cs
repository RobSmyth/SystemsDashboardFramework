using System.Linq;
using System.Windows;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Config;


namespace NoeticTools.SystemsDashboard.Framework.Registries
{
    public sealed class TileFactory : ITileFactory
    {
        private readonly TileProviderRegistry _tileProviderRegistry;

        public TileFactory(TileProviderRegistry tileProviderRegistry)
        {
            _tileProviderRegistry = tileProviderRegistry;
        }

        public FrameworkElement Create(TileConfiguration tileConfiguration, TileLayoutController tileLayoutController)
        {
            var plugin = _tileProviderRegistry.GetAll().Single(x => x.MatchesId(tileConfiguration.TypeId));
            return plugin.CreateTile(tileConfiguration, tileLayoutController);
        }
    }
}