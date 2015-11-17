using NoeticTools.Dashboard.Framework.Config;


namespace NoeticTools.Dashboard.Framework.Tiles
{
    public interface IViewControllerProvider
    {
        bool MatchesId(string id);
        IViewController CreateViewController(TileConfiguration tileConfiguration);
    }
}