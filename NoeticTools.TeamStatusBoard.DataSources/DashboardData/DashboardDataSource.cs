using System;
using System.Collections.Generic;
using System.Globalization;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;


namespace NoeticTools.TeamStatusBoard.DataSources.DashboardData
{
    public class DashboardDataSource : ITimerListener, IDataSource
    {
        private readonly IDataSource _innerDataSource;
        private readonly TimeSpan _tickTime = TimeSpan.FromSeconds(1);
        private readonly DashboardDataConfiguration _configuration;
        private readonly ITimerToken _timerToken;

        public DashboardDataSource(IDashboardConfigurationServices configurationServices, IServices services, IDataSource innerDataSource)
        {
            _innerDataSource = innerDataSource;
            _configuration = new DashboardDataConfiguration(configurationServices.GetService("Dashboard"));

            SetParameter("Version", "1.0.0");
            SetParameter("MachineName", Environment.MachineName);
            SetParameter("StartDateTime", DateTime.UtcNow.ToString("u"));

            _timerToken = services.Timer.QueueCallback(_tickTime, this);
        }

        public string TypeName => "Dashboard";

        public void Write<T>(string name, T value)
        {
            _innerDataSource.Write(name, value);
        }

        public T Read<T>(string name)
        {
            return _innerDataSource.Read<T>(name);
        }

        public IEnumerable<string> GetAllNames()
        {
            return _innerDataSource.GetAllNames();
        }

        public void AddListener(IDataChangeListener listener)
        {
            _innerDataSource.AddListener(listener);
        }

        public bool IsReadOnly(string name)
        {
            return _innerDataSource.IsReadOnly(name);
        }

        public void Set(string name, object value, PropertiesFlags flags, params string[] tags)
        {
            _innerDataSource.Set(name, value, flags, tags);
        }

        public IEnumerable<DataValue> Find(Func<DataValue, bool> predicate)
        {
            return _innerDataSource.Find(predicate);
        }

        public DataValue GetDatum(string name, object defaultValue = null)
        {
            return _innerDataSource.GetDatum(name, defaultValue);
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
    }
}