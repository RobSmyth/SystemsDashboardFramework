using NoeticTools.SystemsDashboard.Framework.Config;


namespace NoeticTools.SystemsDashboard.Framework
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