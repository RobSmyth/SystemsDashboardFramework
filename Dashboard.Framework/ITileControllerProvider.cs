using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Config;


namespace NoeticTools.SystemsDashboard.Framework
{
    public interface ITileControllerProvider
    {
        string Name { get; }
        bool MatchesId(string id);
        IViewController CreateTileController(TileConfiguration tile, TileLayoutController layoutController);
        TileConfiguration CreateDefaultConfiguration();
    }
}