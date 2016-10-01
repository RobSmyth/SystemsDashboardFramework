using System.Linq;
using System.Windows;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.XmlTypes;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;


namespace NoeticTools.TeamStatusBoard.Framework.Registries
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
            tileConfiguration.TypeId = plugin.TypeId;
            return plugin.CreateTile(tileConfiguration, tileLayoutController);
        }
    }
}