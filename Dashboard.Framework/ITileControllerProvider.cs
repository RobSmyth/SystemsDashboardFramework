using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Tiles;


namespace NoeticTools.Dashboard.Framework
{
    public interface ITileControllerProvider
    {
        string Name { get; }
        bool MatchesId(string id);
        IViewController CreateTileController(TileConfiguration tileConfiguration, TileLayoutController tileLayoutController);
        TileConfiguration CreateDefaultConfiguration();
    }
}