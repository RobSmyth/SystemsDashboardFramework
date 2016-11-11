using NoeticTools.TeamStatusBoard.Persistence;
using NoeticTools.TeamStatusBoard.Persistence.Xml;


namespace NoeticTools.TeamStatusBoard.Framework.Persistence
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
            _tileLayoutController.Load(configuration.RootTile);
        }
    }
}