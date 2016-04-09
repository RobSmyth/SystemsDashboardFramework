using System;
using System.Collections.Generic;
using System.Linq;
using NoeticTools.SystemsDashboard.Framework.Services;
using NoeticTools.SystemsDashboard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.Framework.Services.DataServices
{
    public class DataServer : IDataService
    {
        private readonly IDataRepositoryFactory _repositoryFactory;
        private readonly IDictionary<string, IDataSource>  _sources = new Dictionary<string, IDataSource>();

        public DataServer(IDataRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public string Name => "DataServer";

        public void Write<T>(string name, T value)
        {
            throw new NotImplementedException();
        }

        public T Read<T>(string address)
        {
            var elements = address.Split('.');
            if (elements.Length < 2)
            {
                return default(T);
            }

            var sourceName = elements.First();
            var propertyName = address.Substring(sourceName.Length + 1);
            return GetDataSource(sourceName).Read<T>(propertyName);
        }

        public IDataSource GetDataSource(string name)
        {
            if (!_sources.ContainsKey(name))
            {
                return new NullDataSource(name);
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
