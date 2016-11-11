using System.Windows;
using NoeticTools.TeamStatusBoard.Framework.Config.XmlTypes;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;


namespace NoeticTools.TeamStatusBoard.Framework.Registries
{
    public interface ITileFactory
    {
        FrameworkElement Create(TileConfiguration tileConfiguration, TileLayoutController tileLayoutController);
    }
}