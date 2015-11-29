using System;
using NoeticTools.Dashboard.Framework.Config.Parameters;


namespace NoeticTools.Dashboard.Framework.Config
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
            return DateTime.FromFileTimeUtc(long.Parse(value));
        }

        public string GetString(string name)
        {
            return _inner.GetParameter(name, "X").Value;
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

        public object GetParameter(string name, ElementType elementType)
        {
            if (elementType == ElementType.DateTime)
            {
                return GetDateTime(name);
            }
            if (elementType == ElementType.Boolean)
            {
                return GetBool(name);
            }
            return GetString(name);
        }

        public void SetParameter(string name, ElementType elementType, object value)
        {
            if (elementType == ElementType.DateTime)
            {
                SetDateTime(name, (DateTime) value);
            }
            else if (elementType == ElementType.Boolean)
            {
                SetParameter(name, (bool) value);
            }
            else if (value != null)
            {
                SetParameter(name, value.ToString());
            }
        }

        private void SetDateTime(string name, DateTime value)
        {
            SetParameter(name, value.ToFileTimeUtc());
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