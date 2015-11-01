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

        ITileViewModel ITileRegistry.GetNew(DashboardTileConfiguration tileConfiguration)
        {
            return _inner.GetNew(tileConfiguration);
        }

        void ITileRegistry.Clear()
        {
            _inner.Clear();
        }

        ITileViewModel[] ITileRegistry.GetAll()
        {
            return _inner.GetAll();
        }
    }
}