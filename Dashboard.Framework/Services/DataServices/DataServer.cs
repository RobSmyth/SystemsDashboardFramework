using System;
using System.Collections.Generic;
using System.Linq;


namespace NoeticTools.TeamStatusBoard.Framework.Services.DataServices
{
    public class DataServer : IDataService
    {
        private readonly IDictionary<string, IDataSource> _sources = new Dictionary<string, IDataSource>();

        public string TypeName => "DataServer";
        public string Name => "";

        public void Write<T>(string name, T value)
        {
            throw new NotImplementedException();
        }

        public T Read<T>(string address)
        {
            var elements = address.Split('.');
            if (elements.Length < 3)
            {
                return default(T);
            }

            var typeName = elements[0];
            var sourceName = elements[1];
            var propertyName = address.Substring(typeName.Length + sourceName.Length + 2);
            return GetDataSource(typeName, sourceName).Read<T>(propertyName);
        }

        public IEnumerable<string> GetAllNames()
        {
            return _sources.Keys.ToArray();
        }

        public void AddListener(IDataChangeListener listener)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IDataSource> GetAllDataSources()
        {
            return _sources.Values.ToArray();
        }

        public IDataSource GetDataSource(string typeName, string name)
        {
            if (!_sources.ContainsKey(name))
            {
                return new NullDataSource(typeName, name);
            }
            return _sources[name];
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