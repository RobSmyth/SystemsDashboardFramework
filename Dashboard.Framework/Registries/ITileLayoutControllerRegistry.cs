using System.Windows.Controls;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.XmlTypes;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;


namespace NoeticTools.TeamStatusBoard.Framework.Registries
{
    public interface ITileLayoutControllerRegistry
    {
        int Count { get; }
        ITileLayoutController GetNew(Grid tileGrid, TileConfiguration tileConfiguration, TileLayoutController parent);
        ITileLayoutController[] GetAll();
    }
}