using NoeticTools.SystemsDashboard.Framework.Config;


namespace NoeticTools.SystemsDashboard.Framework
{
    public interface IDashBoardLoader
    {
        void Load(DashboardConfiguration configuration);
    }
}