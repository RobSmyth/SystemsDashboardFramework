using NoeticTools.Dashboard.Framework.Config;


namespace NoeticTools.Dashboard.Framework.Plugins.Tiles
{
    public interface ITileLayoutController
    {
        void Clear();
        void ToggleShowGroupPanelDetailsMode();
        void AddTile(TileConfiguration tile);
        void Load(TileConfiguration tileConfiguration);
    }
}