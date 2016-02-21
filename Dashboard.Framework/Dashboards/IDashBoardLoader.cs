using NoeticTools.SystemsDashboard.Framework.Config;


namespace NoeticTools.SystemsDashboard.Framework.Dashboards
{
    public interface IDashBoardLoader
    {
        void Load(DashboardConfiguration configuration);
    }
}