using System.Collections.Generic;
using System.Windows.Controls;
using NoeticTools.TeamStatusBoard.Framework.Config.XmlTypes;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;


namespace NoeticTools.TeamStatusBoard.Framework.Registries
{
    public interface ITileLayoutControllerRegistry
    {
        int Count { get; }
        void GetNew(Grid tileGrid, TileConfiguration tileConfiguration, TileLayoutController parent);
        IEnumerable<ITileLayoutController> GetAll();
    }
}