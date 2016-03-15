using System;
using System.Globalization;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Services.TimeServices;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.DataSources.StaticProperties
{
    public class VizBoardPropertiesService : ITimerListener
    {
        private readonly TimeSpan _tickTime = TimeSpan.FromSeconds(5);
        private readonly VizBoardPropertiesServicesConfiguration _configuration;
        private readonly TimerToken _timerToken;

        public VizBoardPropertiesService(IDashboardConfigurationServices servicesConfiguration, IServices services)
        {
            _configuration = new VizBoardPropertiesServicesConfiguration(servicesConfiguration.GetService("VizBoardProperties"));
            _configuration.GetParameter("VizBoard.Version", string.Empty).Value = "1.0.0";
            _configuration.GetParameter("VizBoard.MachineName", string.Empty).Value = Environment.MachineName;
            _configuration.GetParameter("VizBoard.LocalStartDateTime", string.Empty).Value = DateTime.Now.ToString(CultureInfo.CurrentCulture);

            _timerToken = services.Timer.QueueCallback(_tickTime, this);
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            _configuration.GetParameter("Dashboard.LocalDateTime", string.Empty).Value = DateTime.Now.ToString(CultureInfo.CurrentCulture);
            _timerToken.Requeue(_tickTime);
        }
    }
}
