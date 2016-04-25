using System.Windows;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;


namespace NoeticTools.TeamStatusBoard.Framework
{
    public interface ITileControllerProvider
    {
        string Name { get; }
        bool MatchesId(string id);
        FrameworkElement CreateTile(TileConfiguration tile, TileLayoutController layoutController);
        TileConfiguration CreateDefaultConfiguration();
    }
}