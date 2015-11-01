using NoeticTools.Dashboard.Framework;
using NoeticTools.Dashboard.Framework.Config;

namespace NoeticTools.TeamDashboard
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