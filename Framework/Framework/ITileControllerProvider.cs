using System.Windows;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Persistence.Xml;


namespace NoeticTools.TeamStatusBoard.Framework
{
    public interface ITileControllerProvider
    {
        string TypeId { get; }
        bool MatchesId(string id);
        FrameworkElement CreateTile(TileConfiguration tileConfigturation, TileLayoutController layoutController);
        TileConfiguration CreateDefaultConfiguration();
    }
}