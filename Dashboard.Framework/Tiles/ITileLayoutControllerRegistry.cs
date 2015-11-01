using System.Windows.Controls;


namespace NoeticTools.Dashboard.Framework.Tiles
{
    public interface ITileLayoutControllerRegistry
    {
        ITileLayoutController GetNew(Grid tileGrid);
        ITileLayoutController[] GetAll();
    }
}