namespace NoeticTools.TeamStatusBoard.Persistence.Xml
{
    public interface IDashboardConfigurationServices
    {
        DashboardServiceConfiguration GetService(string serviceName);
    }
}