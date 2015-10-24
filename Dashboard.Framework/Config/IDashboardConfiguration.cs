using System;
using NoeticTools.Dashboard.Framework.Config;

namespace Dashboard.Config
{
    public interface IDashboardConfiguration
    {
        string Name { get; set; }

        string DisplayName { get; set; }

        DashboardTileConfiguration RootTile { get; set; }
        DashboardTileConfiguration GetTileConfiguration(Guid tileId);
    }
}