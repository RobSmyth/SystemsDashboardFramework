using NoeticTools.SystemsDashboard.Framework.Config;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.DataSources.StaticProperties
{
    public class StaticPropertiesServices
    {
        private readonly StaticPropertiesServicesConfiguration _configuration;

        public StaticPropertiesServices(IDashboardConfigurationServices servicesConfiguration)
        {
            _configuration = new StaticPropertiesServicesConfiguration(servicesConfiguration.GetService("StaticPropertyServices"));
            _configuration.GetParameter("Dashboard.Version", string.Empty).Value = "1.0.0";
        }
    }
}
