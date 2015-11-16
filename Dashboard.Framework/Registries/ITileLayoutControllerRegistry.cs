using System.Windows.Controls;
using NoeticTools.Dashboard.Framework.Tiles;


namespace NoeticTools.Dashboard.Framework.Registries
{
    public interface ITileLayoutControllerRegistry
    {
        ITileLayoutController GetNew(Grid tileGrid);
        ITileLayoutController[] GetAll();
        int Count { get; }
    }
}