namespace NoeticTools.SystemsDashboard.Framework.Config
{
    public interface IDashboardConfigurationServices
    {
        DashboardServiceConfiguration[] Services { get; set; }
        DashboardServiceConfiguration GetService(string serviceName);
    }
}