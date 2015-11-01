using NoeticTools.Dashboard.Framework.Config;

namespace NoeticTools.Dashboard.Framework
{
    public interface IDashBoardLoader
    {
        void Load(DashboardConfiguration configuration);
    }
}