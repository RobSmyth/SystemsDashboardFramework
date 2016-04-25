namespace NoeticTools.TeamStatusBoard.Framework.Config
{
    public interface IDashboardConfigurations
    {
        string Current { get; set; }
        DashboardConfiguration[] Configurations { get; set; }
        DashboardConfigurationServices Services { get; set; }
    }
}