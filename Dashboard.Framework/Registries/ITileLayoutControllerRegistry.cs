using System.Windows.Controls;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Config;


namespace NoeticTools.SystemsDashboard.Framework.Registries
{
    public interface ITileLayoutControllerRegistry
    {
        int Count { get; }
        ITileLayoutController GetNew(Grid tileGrid, TileConfiguration tileConfiguration, TileLayoutController parent);
        ITileLayoutController[] GetAll();
    }
}