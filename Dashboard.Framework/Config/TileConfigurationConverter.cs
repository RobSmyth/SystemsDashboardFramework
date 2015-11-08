using System;
using NoeticTools.Dashboard.Framework.Config.Parameters;


namespace NoeticTools.Dashboard.Framework.Config
{
    public class TileConfigurationConverter
    {
        private readonly TileConfiguration _inner;
        private readonly IConfigurationChangeListener _listener;

        public TileConfigurationConverter(TileConfiguration inner, IConfigurationChangeListener listener)
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
            if (parameter.ValueType is DateTime)
            {
                return GetDateTime(parameter.Name).ToShortDateString();
            }
            return GetParameter<string>(parameter.Name);
        }

        public void SetParameterValue(IConfigurationParameter parameter, string text)
        {
            if (parameter.ValueType is DateTime)
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

        public void SetParameter<T>(string name, T value)
        {
            var textValue = value.ToString();
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

        private T GetParameter<T>(string name)
        {
            var value = _inner.GetParameter(name, string.Empty).Value;
            return (T) Convert.ChangeType(value, typeof (T));
        }
    }
}