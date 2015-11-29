﻿using NoeticTools.Dashboard.Framework.Config;


namespace NoeticTools.Dashboard.Framework.Tiles
{
    public interface ITileLayoutController
    {
        void Clear();
        void ToggleShowGroupPanelDetailsMode();
        void AddTile(TileConfiguration tileConfiguration);
        void Load(TileConfiguration tileConfiguration);
    }
}