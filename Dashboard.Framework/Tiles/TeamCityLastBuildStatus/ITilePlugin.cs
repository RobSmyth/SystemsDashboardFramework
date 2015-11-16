using NoeticTools.Dashboard.Framework.Config;


namespace NoeticTools.Dashboard.Framework.Tiles.TeamCityLastBuildStatus
{
    public interface ITilePlugin
    {
        bool MatchesId(string id);
        IViewController CreateViewController(TileConfiguration tileConfiguration);
    }
}