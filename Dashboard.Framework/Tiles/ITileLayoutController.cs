using NoeticTools.Dashboard.Framework.Config;


namespace NoeticTools.Dashboard.Framework.Tiles
{
    public interface ITileLayoutController
    {
        void AddTile(TileConfiguration tileConfiguration);
        void Clear();
        void ToggleShowGroupPanelDetailsMode();
    }
}