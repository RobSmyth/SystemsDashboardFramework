using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;


namespace NoeticTools.SystemsDashboard.Framework.Services.DataServices
{
    public class DataService : IService
    {
        private readonly IDataRepositoryFactory _repositoryFactory;
        private readonly IList<IDataSource> _sources = new List<IDataSource>();
        private readonly IDictionary<string, IDataSink>  _sinks = new Dictionary<string, IDataSink>();

        public DataService(IDataRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public IDataSource GetDataSource(string name)
        {
            var dataSource = new DataSource(name);
            _sources.Add(dataSource);
            GetDataSink(name).AddListener(dataSource);
            return dataSource;
        }

        public IDataSink CreateDataSink(string name)
        {
            var sink = _repositoryFactory.Create(name, _sinks.Values.Count(x => x.ShortName.Equals(name, StringComparison.InvariantCulture))+1);
            _sinks.Add(sink.Name, sink);
            return sink;
        }

        private IDataSink GetDataSink(string name)
        {
            if (!_sinks.ContainsKey(name))
            {
                return new NullDataSink();
            }
            return _sinks[name];
        }

        private class NullDataSink : IDataSink
        {
            public string Name => "";
            public string ShortName => "";
            public void AddListener(IChangeListener listener) {}
        }

        void IService.Stop()
        {
        }

        void IService.Start()
        {
        }
    }
}
