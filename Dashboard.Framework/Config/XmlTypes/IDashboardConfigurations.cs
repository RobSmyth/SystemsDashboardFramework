namespace NoeticTools.TeamStatusBoard.Framework.Config.XmlTypes
{
    public interface IDashboardConfigurations
    {
        string Current { get; set; }
        DashboardConfiguration[] Configurations { get; set; }
        DashboardConfigurationServices Services { get; set; }
    }
}