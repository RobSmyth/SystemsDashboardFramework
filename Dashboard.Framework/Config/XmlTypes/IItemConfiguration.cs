namespace NoeticTools.TeamStatusBoard.Framework.Config.XmlTypes
{
    public interface IItemConfiguration
    {
        DashboardConfigValuePair GetParameter(string name, string defaultValue);
    }
}