using System.Windows;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;


namespace NoeticTools.TeamStatusBoard.Framework.Registries
{
    public interface ITileFactory
    {
        FrameworkElement Create(TileConfiguration tileConfiguration, TileLayoutController tileLayoutController);
    }
}