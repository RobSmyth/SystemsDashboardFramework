using System;
using System.Collections.Generic;
using System.Linq;


namespace NoeticTools.TeamStatusBoard.Framework.Services.DataServices
{
    public class DataServer : IDataService
    {
        private readonly IDictionary<string, IDataSource> _sources = new Dictionary<string, IDataSource>();

        public string TypeName => "DataServer";
        public string Name { get { return ""; } set {} }

        public void Write<T>(string name, T value)
        {
            throw new NotImplementedException();
        }

        public T Read<T>(string address)
        {
            var parser = new DataSourcePropertyParser(address);
            if (!parser.IsValid)
            {
                return default(T);
            }
            var dataSource = GetDataSource(parser.TypeName);
            return dataSource.Read<T>(parser.PropertyName);
        }

        public IEnumerable<string> GetAllNames()
        {
            return _sources.Keys.OrderBy(x => x).ToArray();
        }

        public void AddListener(IDataChangeListener listener)
        {
            throw new NotImplementedException();
        }

        public bool IsReadOnly(string name)
        {
            throw new NotImplementedException();
        }

        public void Set(string name, object value, PropertiesFlags flags, params string[] tags)
        {
            var parser = new DataSourcePropertyParser(name);
            GetDataSource(parser.TypeName).Set(parser.PropertyName, value, flags, tags);
        }

        public IEnumerable<DataValue> Find(Func<DataValue, bool> predicate)
        {
            throw new InvalidOperationException();
        }

        public IEnumerable<IDataSource> GetAllDataSources()
        {
            return _sources.Values.OrderBy(x => x.TypeName).ToArray();
        }

        public IDataSource Get(string name)
        {
            if (!_sources.ContainsKey(name))
            {
                return new NullDataSource(name);
            }
            return _sources[name];
        }

        public IDataSource GetDataSource(string typeName)
        {
            if (!_sources.ContainsKey(typeName))
            {
                return new NullDataSource(typeName);
            }
            return _sources[typeName];
        }

        public void Register(string name, IDataSource dataSource)
        {
            _sources.Add(name, dataSource);
        }

        void IService.Stop()
        {
        }

        void IService.Start()
        {
        }
    }
}