using System.Collections.Generic;
using System.Linq;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Tiles;


namespace NoeticTools.Dashboard.Framework.Registries
{
    public class TileControllerRegistry : ITileRegistry
    {
        private readonly TilePluginRegistry _plugins;
        private readonly IList<IViewController> _tileControllers = new List<IViewController>();

        public TileControllerRegistry(TilePluginRegistry plugins)
        {
            _plugins = plugins;
        }

        public IViewController GetNew(TileConfiguration tileConfiguration)
        {
            var plugin = _plugins.Get(tileConfiguration.TypeId);
            var tileViewModel = plugin.CreateViewController(tileConfiguration);
            _tileControllers.Add(tileViewModel);
            return tileViewModel;
        }

        public IViewController[] GetAll()
        {
            return _tileControllers.ToArray();
        }

        public void Clear()
        {
            _tileControllers.Clear();
        }
    }
}