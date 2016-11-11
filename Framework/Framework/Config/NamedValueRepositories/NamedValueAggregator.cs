using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories
{
    public sealed class NamedValueRepositoryAggregator : INamedValueRepository
    {
        private readonly INamedValueReaderProvider[] _providers;

        public NamedValueRepositoryAggregator(params INamedValueReaderProvider[] providers)
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

        public Color GetColour(string name, string defaultValue)
        {
            return Get(name).GetColour(name, defaultValue);
        }

        public T Get<T>(string name)
        {
            return Get(name).Get<T>(name);
        }

        public IDataValue GetDatum(string name, object defaultValue = null)
        {
            return Get(name).GetDatum(name, defaultValue);
        }

        public IEnumerable<IDataValue> GetDatums(string name)
        {
            return Get(name).GetDatums(name);
        }

        private INamedValueRepository Get(string name)
        {
            return _providers.First(x => x.CanHandle(name)).Get(name);
        }
    }
}