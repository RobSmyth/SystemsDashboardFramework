using NoeticTools.Dashboard.Framework.Config;

namespace NoeticTools.Dashboard.Framework.Tiles
{
    public interface ITileFactory
    {
        ITileViewModel Create(DashboardTileConfiguration tileConfiguration);
    }
}