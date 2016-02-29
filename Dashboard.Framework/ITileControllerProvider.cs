using System.Windows;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Dashboards;


namespace NoeticTools.SystemsDashboard.Framework
{
    public interface ITileControllerProvider
    {
        string Name { get; }
        bool MatchesId(string id);
        FrameworkElement CreateTile(TileConfiguration tile, TileLayoutController layoutController);
        TileConfiguration CreateDefaultConfiguration();
    }
}