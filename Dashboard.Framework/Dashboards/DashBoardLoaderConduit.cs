using NoeticTools.TeamStatusBoard.Framework.Config.XmlTypes;


namespace NoeticTools.TeamStatusBoard.Framework.Dashboards
{
    public class DashBoardLoaderConduit : IDashBoardLoader
    {
        private IDashBoardLoader _inner;

        public void Load(DashboardConfiguration configuration)
        {
            _inner.Load(configuration);
        }

        public void SetTarget(IDashBoardLoader loader)
        {
            _inner = loader;
        }
    }
}