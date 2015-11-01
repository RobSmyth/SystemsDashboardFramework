using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Tiles;

namespace NoeticTools.Dashboard.Framework
{
    public class DashBoardLoader : IDashBoardLoader
    {
        private readonly ITileLayoutController _tileLayoutController;

        public DashBoardLoader(ITileLayoutController tileLayoutController)
        {
            _tileLayoutController = tileLayoutController;
        }

        public void Load(DashboardConfiguration configuration)
        {
            _tileLayoutController.Clear();

            foreach (var tile in configuration.RootTile.Tiles)
            {
                _tileLayoutController.AddTile(tile);
            }
        }
    }
}