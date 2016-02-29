using System;
using System.Globalization;


namespace NoeticTools.SystemsDashboard.Framework.Config
{
    public sealed class TileConfigurationConverter
    {
        private readonly IItemConfiguration _inner;
        private readonly IConfigurationChangeListener _listener;

        public TileConfigurationConverter(IItemConfiguration inner, IConfigurationChangeListener listener)
        {
            _inner = inner;
            _listener = listener;
        }

        public DateTime GetDateTime(string name)
        {
            var value = _inner.GetParameter(name, string.Empty).Value;
            if (string.IsNullOrWhiteSpace(value))
            {
                return new DateTime(2000, 1, 1);
            }

            long fileTime;
            if (long.TryParse(value, out fileTime))
            {
                return DateTime.FromFileTimeUtc(fileTime);
            }
            return DateTime.Parse(value, CultureInfo.CurrentCulture);
        }

        public TimeSpan GetTimeSpan(string name)
        {
            var value = _inner.GetParameter(name, string.Empty).Value;
            if (string.IsNullOrWhiteSpace(value))
            {
                return TimeSpan.Zero;
            }

            return TimeSpan.Parse(value, CultureInfo.CurrentCulture);
        }

        public string GetString(string name)
        {
            return _inner.GetParameter(name, "").Value;
        }

        public bool GetBool(string name)
        {
            var value = _inner.GetParameter(name, string.Empty).Value;
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }
            return bool.Parse(value);
        }

        public object GetParameter(string name)
        {
            return GetString(name);
        }

        public void SetParameter(string name, object value)
        {
            SetParameter(name, value.ToString());
        }

        private void SetParameter<T>(string name, T value)
        {
            var textValue = value.ToString();
            if (_inner.GetParameter(name, string.Empty).Value != textValue)
            {
                _inner.GetParameter(name, string.Empty).Value = textValue;
                _listener.OnConfigurationChanged(this);
            }
        }

        public string GetString(string name, string defaultValue)
        {
            var value = GetString(name);
            return string.IsNullOrWhiteSpace(value) ? defaultValue : value;
        }
    }
}