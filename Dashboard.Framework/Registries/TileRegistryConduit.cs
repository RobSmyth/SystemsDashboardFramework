using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Tiles;


namespace NoeticTools.Dashboard.Framework.Registries
{
    public class TileRegistryConduit : ITileControllerRegistry
    {
        private ITileControllerRegistry _inner;

        public void SetTarget(ITileControllerRegistry tileRegistry)
        {
            _inner = tileRegistry;
        }

        IViewController ITileControllerRegistry.GetNew(TileConfiguration tileConfiguration)
        {
            return _inner.GetNew(tileConfiguration);
        }

        void ITileControllerRegistry.Clear()
        {
            _inner.Clear();
        }

        IViewController[] ITileControllerRegistry.GetAll()
        {
            return _inner.GetAll();
        }

        public void Register(IViewControllerProvider provider)
        {
            _inner.Register(provider);
        }
    }
}