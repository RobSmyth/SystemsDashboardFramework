using System;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Services.TimeServices;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.DataSources.StaticProperties
{
    public class StaticPropertiesServices : ITimerListener
    {
        private readonly TimeSpan _tickTime = TimeSpan.FromSeconds(5);
        private readonly StaticPropertiesServicesConfiguration _configuration;
        private readonly TimerToken _timerToken;

        public StaticPropertiesServices(IDashboardConfigurationServices servicesConfiguration, IServices services)
        {
            _configuration = new StaticPropertiesServicesConfiguration(servicesConfiguration.GetService("StaticPropertyServices"));
            _configuration.GetParameter("Dashboard.Version", string.Empty).Value = "1.0.0";
            _configuration.GetParameter("Dashboard.MachineName", string.Empty).Value = Environment.MachineName;
            _configuration.GetParameter("Dashboard.StartDateTime", string.Empty).Value = DateTime.Now.ToString();

            _timerToken = services.Timer.QueueCallback(_tickTime, this);
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            _configuration.GetParameter("Dashboard.CurrentDateTime", string.Empty).Value = DateTime.Now.ToString();
            _timerToken.Requeue(_tickTime);
        }
    }
}
