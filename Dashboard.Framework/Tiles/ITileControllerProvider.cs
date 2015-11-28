using NoeticTools.Dashboard.Framework.Config;


namespace NoeticTools.Dashboard.Framework.Tiles
{
    public interface ITileControllerProvider
    {
        bool MatchesId(string id);
        IViewController CreateTileController(TileConfiguration tileConfiguration);
        string Name { get; }
    }
}