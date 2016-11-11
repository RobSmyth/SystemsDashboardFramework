using NoeticTools.TeamStatusBoard.Framework.Persistence.Xml;


namespace NoeticTools.TeamStatusBoard.Framework.Persistence
{
    public interface IDashBoardLoader
    {
        void Load(DashboardConfiguration configuration);
    }
}