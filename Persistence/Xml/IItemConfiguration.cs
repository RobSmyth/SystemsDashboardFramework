namespace NoeticTools.TeamStatusBoard.Persistence.Xml
{
    public interface IItemConfiguration
    {
        DashboardConfigValuePair GetParameter(string name, string defaultValue);
    }
}