using System.Windows;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Dashboards;


namespace NoeticTools.SystemsDashboard.Framework.Registries
{
    public interface ITileFactory
    {
        FrameworkElement Create(TileConfiguration tileConfiguration, TileLayoutController tileLayoutController);
    }
}