using System.Windows.Controls;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Plugins.Tiles;


namespace NoeticTools.Dashboard.Framework.Registries
{
    public interface ITileLayoutControllerRegistry
    {
        int Count { get; }
        ITileLayoutController GetNew(Grid tileGrid, TileConfiguration tileConfiguration, TileLayoutController parent);
        ITileLayoutController[] GetAll();
    }
}