using System.Windows;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.XmlTypes;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;


namespace NoeticTools.TeamStatusBoard.Framework
{
    public interface ITileLayoutController
    {
        void Clear();
        void ToggleShowGroupPanelDetailsMode();
        void Load(TileConfiguration tileConfiguration);
        void Replace(TileConfiguration tile, TileConfiguration newTile);
        void InsertTile(TileConfiguration currentTile, TileConfiguration tile, TileInsertAction insertAction);
        void Remove(TileConfiguration tile);
        void SplitTile(TileConfiguration tile, TileConfiguration newTile, TileInsertAction insertAction);
        bool ProvdesLayoutFor(UIElement element);
        void AddTile(TileConfiguration tile);
    }
}