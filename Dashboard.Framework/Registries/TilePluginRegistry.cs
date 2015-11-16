using System.Linq;
using NoeticTools.Dashboard.Framework.Tiles;


namespace NoeticTools.Dashboard.Framework.Registries
{
    public class TilePluginRegistry
    {
        private readonly ITilePlugin[] _plugins;

        public TilePluginRegistry(ITilePlugin[] plugins)
        {
            _plugins = plugins;
        }

        public ITilePlugin Get(string id)
        {
            return _plugins.First(x => x.MatchesId(id));
        }

        public ITilePlugin[] GetAll()
        {
            return _plugins;
        }
    }
}