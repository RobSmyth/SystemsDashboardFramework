using System;
using System.Collections.Generic;
using System.Globalization;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Plugins.DataSources.StaticProperties;
using NoeticTools.SystemsDashboard.Framework.Services.DataServices;
using NoeticTools.SystemsDashboard.Framework.Services.TimeServices;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.DataSources.DashboardData
{
    public class DashboardDataSource : ITimerListener, IDataSource
    {
        private readonly IDataSource _innerDataSource;
        private readonly TimeSpan _tickTime = TimeSpan.FromSeconds(1);
        private readonly DashboardDataConfiguration _configuration;
        private readonly TimerToken _timerToken;

        public DashboardDataSource(IDashboardConfigurationServices configurationServices, IServices services, IDataSource innerDataSource)
        {
            _innerDataSource = innerDataSource;
            _configuration = new DashboardDataConfiguration(configurationServices.GetService("Dashboard"));

            SetParameter("Version", "1.0.0");
            SetParameter("MachineName", Environment.MachineName);
            SetParameter("StartDateTime", DateTime.UtcNow.ToString("u"));

            _timerToken = services.Timer.QueueCallback(_tickTime, this);
        }

        private void SetParameter(string name, string value)
        {
            _configuration.GetParameter(name, string.Empty).Value = value;
            _innerDataSource.Write(name, _configuration.GetParameter(name, string.Empty).Value);
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            _configuration.GetParameter("LocalDateTime", string.Empty).Value = DateTime.Now.ToString(CultureInfo.CurrentCulture);
            _timerToken.Requeue(_tickTime);
        }

        public string Name => "Dashboard";

        public void Write<T>(string name, T value)
        {
            _innerDataSource.Write<T>(name, value);
        }

        public T Read<T>(string name)
        {
            return _innerDataSource.Read<T>(name);
        }

        public IEnumerable<string> GetAllNames()
        {
            return _innerDataSource.GetAllNames();
        }
    }
}
