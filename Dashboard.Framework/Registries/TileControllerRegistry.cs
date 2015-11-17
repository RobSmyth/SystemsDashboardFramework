using System.Collections.Generic;
using System.Linq;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Tiles;


namespace NoeticTools.Dashboard.Framework.Registries
{
    public class TileControllerRegistry : ITileControllerRegistry
    {
        private readonly IList<IViewController> _tileControllers = new List<IViewController>();
        private readonly IList<IViewControllerProvider> _providers = new List<IViewControllerProvider>();

        public IViewController GetNew(TileConfiguration tileConfiguration)
        {
            var plugin = _providers.Single(x => x.MatchesId(tileConfiguration.TypeId));
            var tileViewModel = plugin.CreateViewController(tileConfiguration);
            _tileControllers.Add(tileViewModel);
            return tileViewModel;
        }

        public IViewController[] GetAll()
        {
            return _tileControllers.ToArray();
        }

        public void Register(IViewControllerProvider provider)
        {
            _providers.Add(provider);
        }

        public void Clear()
        {
            _tileControllers.Clear();
        }
    }
}