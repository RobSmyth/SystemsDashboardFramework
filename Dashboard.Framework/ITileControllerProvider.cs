using System.Windows;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Config;


namespace NoeticTools.SystemsDashboard.Framework
{
    public interface ITileControllerProvider
    {
        bool MatchesId(string id);
        FrameworkElement CreateTile(TileConfiguration tile, TileLayoutController layoutController);
        TileConfiguration CreateDefaultConfiguration();
    }
}