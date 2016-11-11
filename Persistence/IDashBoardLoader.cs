using NoeticTools.TeamStatusBoard.Persistence.Xml;


namespace NoeticTools.TeamStatusBoard.Persistence
{
    public interface IDashBoardLoader
    {
        void Load(DashboardConfiguration configuration);
    }
}