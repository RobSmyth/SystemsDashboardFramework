using System;
using System.Linq;


namespace NoeticTools.TeamStatusBoard.Framework.Config
{
    public sealed class NamedValueReaderAggregator : INamedValueReader
    {
        private readonly INamedValueReaderProvider[] _providers;

        public NamedValueReaderAggregator(params INamedValueReaderProvider[] providers)
        {
            _providers = providers;
        }

        public DateTime GetDateTime(string name)
        {
            return Get(name).GetDateTime(name);
        }

        public TimeSpan GetTimeSpan(string name)
        {
            return Get(name).GetTimeSpan(name);
        }

        public string GetString(string name)
        {
            return Get(name).GetString(name);
        }

        public string GetString(string name, string defaultValue)
        {
            return Get(name).GetString(name, defaultValue);
        }

        public double GetDouble(string name, double defaultValue = 0)
        {
            return Get(name).GetDouble(name, defaultValue);
        }

        public bool GetBool(string name)
        {
            return Get(name).GetBool(name);
        }

        public object GetParameter(string name)
        {
            return Get(name).GetParameter(name);
        }

        public object GetParameter(string name, object defaultValue)
        {
            return Get(name).GetParameter(name, defaultValue);
        }

        public void SetParameter(string name, object value)
        {
            Get(name).SetParameter(name, value);
        }

        private INamedValueReader Get(string name)
        {
            return _providers.First(x => x.CanHandle(name)).Get(name);
        }
    }
}