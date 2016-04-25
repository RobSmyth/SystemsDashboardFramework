using System.Windows;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;


namespace NoeticTools.TeamStatusBoard.Framework
{
    public class NullTileControllerProvider : ITileControllerProvider
    {
        public string Name { get; }

        public bool MatchesId(string id)
        {
            return false;
        }

        public FrameworkElement CreateTile(TileConfiguration tile, TileLayoutController layoutController)
        {
            throw new System.NotImplementedException();
        }

        public TileConfiguration CreateDefaultConfiguration()
        {
            throw new System.NotImplementedException();
        }
    }
}