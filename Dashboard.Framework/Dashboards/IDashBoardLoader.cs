using NoeticTools.TeamStatusBoard.Framework.Config;


namespace NoeticTools.TeamStatusBoard.Framework.Dashboards
{
    public interface IDashBoardLoader
    {
        void Load(DashboardConfiguration configuration);
    }
}