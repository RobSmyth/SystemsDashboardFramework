using NoeticTools.Dashboard.Framework.Config;


namespace NoeticTools.Dashboard.Framework.Tiles
{
    public class TileRegistryConduit : ITileRegistry
    {
        private ITileRegistry _inner;

        public void SetTarget(ITileRegistry tileRegistry)
        {
            _inner = tileRegistry;
        }

        IViewController ITileRegistry.GetNew(TileConfiguration tileConfiguration)
        {
            return _inner.GetNew(tileConfiguration);
        }

        void ITileRegistry.Clear()
        {
            _inner.Clear();
        }

        IViewController[] ITileRegistry.GetAll()
        {
            return _inner.GetAll();
        }
    }
}