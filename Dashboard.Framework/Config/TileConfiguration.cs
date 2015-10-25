using System;
using Dashboard.Config;
using Dashboard.Config.Parameters;

namespace NoeticTools.Dashboard.Framework.Config
{
    public class TileConfiguration
    {
        private readonly DashboardTileConfiguration _inner;
        private readonly IConfigurationChangeListener _listener;

        public TileConfiguration(DashboardTileConfiguration inner, IConfigurationChangeListener listener)
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
            return DateTime.FromFileTimeUtc(long.Parse(value));
        }

        public string GetParameterValueText(IConfigurationParameter parameter)
        {
            if (parameter.DefaultValue is DateTime)
            {
                return GetDateTime(parameter.Name).ToShortDateString();
            }
            return GetParameter<string>(parameter.Name);
        }

        public void SetParameterValue(IConfigurationParameter parameter, string text)
        {
            if (parameter.DefaultValue is DateTime)
            {
                SetDateTime(parameter.Name, DateTime.Parse(text));
            }
            else
            {
                SetParameter(parameter.Name, text);
            }
        }

        public void SetDateTime(string name, DateTime value)
        {
            SetParameter(name, value.ToFileTimeUtc());
        }

        public string GetString(string name)
        {
            return _inner.GetParameter(name, "X").Value;
        }

        public void SetString(string name, string value)
        {
            SetParameter(name, value);
        }

        private T GetParameter<T>(string name)
        {
            string value = _inner.GetParameter(name, string.Empty).Value;
            return (T) Convert.ChangeType(value, typeof (T));
        }

        public void SetParameter<T>(string name, T value)
        {
            string textValue = value.ToString();
            if (_inner.GetParameter(name, string.Empty).Value != textValue)
            {
                _inner.GetParameter(name, string.Empty).Value = textValue;
                _listener.OnConfigurationChanged();
            }
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
    }
}