using System.Windows;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;


namespace NoeticTools.TeamStatusBoard.Framework
{
    public interface ITileControllerProvider
    {
        string Name { get; }
        string TypeId { get; }
        bool MatchesId(string id);
        FrameworkElement CreateTile(TileConfiguration tileConfigturation, TileLayoutController layoutController);
        TileConfiguration CreateDefaultConfiguration();
    }
}