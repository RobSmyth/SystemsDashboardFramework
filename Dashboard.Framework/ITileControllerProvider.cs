using System.Windows;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.XmlTypes;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;


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