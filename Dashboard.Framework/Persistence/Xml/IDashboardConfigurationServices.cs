namespace NoeticTools.TeamStatusBoard.Framework.Persistence.Xml
{
    public interface IDashboardConfigurationServices
    {
        DashboardServiceConfiguration[] Services { get; set; }
        DashboardServiceConfiguration GetService(string serviceName);
    }
}