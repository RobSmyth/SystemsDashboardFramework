using System.Windows.Controls;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Tiles;


namespace NoeticTools.Dashboard.Framework.Registries
{
    public interface ITileLayoutControllerRegistry
    {
        ITileLayoutController GetNew(Grid tileGrid, TileConfiguration tileConfiguration);
        ITileLayoutController[] GetAll();
        int Count { get; }
    }
}