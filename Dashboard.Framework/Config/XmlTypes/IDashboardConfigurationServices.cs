namespace NoeticTools.TeamStatusBoard.Framework.Config.XmlTypes
{
    public interface IDashboardConfigurationServices
    {
        DashboardServiceConfiguration[] Services { get; set; }
        DashboardServiceConfiguration GetService(string serviceName);
    }
}