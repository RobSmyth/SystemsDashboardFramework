using NoeticTools.Dashboard.Framework.Config;

namespace NoeticTools.Dashboard.Framework.Tiles
{
    public interface ITileRegistry
    {
        ITileViewModel GetNew(DashboardTileConfiguration tileConfiguration);
        void Clear();
        ITileViewModel[] GetAll();
    }
}