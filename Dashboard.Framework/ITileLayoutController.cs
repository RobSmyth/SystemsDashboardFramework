using NoeticTools.Dashboard.Framework.Config;

namespace NoeticTools.Dashboard.Framework
{
    public interface ITileLayoutController
    {
        void AddTile(DashboardTileConfiguration tileConfiguration);
    }
}