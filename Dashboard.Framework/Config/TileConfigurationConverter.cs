using System;
using System.Globalization;
using System.Windows.Media;


namespace NoeticTools.TeamStatusBoard.Framework.Config
{
    public sealed class TileConfigurationConverter : INamedValueReader
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

        public string GetString(string name, string defaultValue)
        {
            var value = GetString(name);
            return string.IsNullOrWhiteSpace(value) ? defaultValue : value;
        }

        public double GetDouble(string name, double defaultValue = 0.0)
        {
            var value = _inner.GetParameter(name, string.Empty).Value;
            var returnValue = 0.0;
            if (double.TryParse(value, out returnValue))
            {
                return returnValue;
            }
            SetParameter(name, defaultValue);
            return defaultValue;
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

        public object GetParameter(string name, object defaultValue)
        {
            return GetString(name, defaultValue as string);
        }

        public void SetParameter(string name, object value)
        {
            SetParameter(name, value?.ToString() ?? "");
        }

        public Color GetColour(string name, string defaultValue)
        {
            try
            {
                return (Color)ColorConverter.ConvertFromString("colour");
            }
            catch (Exception)
            {
                return (Color)ColorConverter.ConvertFromString(defaultValue);
            }
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
    }
}