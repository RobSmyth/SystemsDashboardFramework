using NoeticTools.Dashboard.Framework.Config;


namespace NoeticTools.Dashboard.Framework
{
    public interface ITileLayoutController
    {
        void Clear();
        void ToggleShowGroupPanelDetailsMode();
        void Load(TileConfiguration tileConfiguration);
        void Replace(TileConfiguration tile, TileConfiguration newTile);
        void InsertTile(TileConfiguration tile, TileInsertAction insertAction, TileConfiguration currentTile);
        void Remove(TileConfiguration tile);
    }
}