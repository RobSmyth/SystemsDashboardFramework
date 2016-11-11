namespace NoeticTools.TeamStatusBoard.Framework.Persistence.Xml
{
    public interface IItemConfiguration
    {
        DashboardConfigValuePair GetParameter(string name, string defaultValue);
    }
}