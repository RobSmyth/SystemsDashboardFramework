using System.Windows;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Persistence.Xml;


namespace NoeticTools.TeamStatusBoard.Framework.Registries
{
    public interface ITileFactory
    {
        FrameworkElement Create(TileConfiguration tileConfiguration, TileLayoutController tileLayoutController);
    }
}