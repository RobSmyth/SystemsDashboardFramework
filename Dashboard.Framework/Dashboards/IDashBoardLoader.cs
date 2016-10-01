using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.XmlTypes;


namespace NoeticTools.TeamStatusBoard.Framework.Dashboards
{
    public interface IDashBoardLoader
    {
        void Load(DashboardConfiguration configuration);
    }
}