using System.Collections.Generic;
using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Channel;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.TcSharpInterop;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Agents
{
    public sealed class BuildAgentRepository : IBuildAgentRepository
    {
        private readonly IDataSource _dataSource;
        private readonly IChannelConnectionStateBroadcaster _channelStateBroadcaster;
        private readonly IBuildAgentViewModelFactory _buildAgentFactory;
        private readonly IDictionary<string, IBuildAgent> _buildAgents = new Dictionary<string, IBuildAgent>();
        private readonly object _syncRoot = new object();
        private readonly IList<IDataChangeListener> _listeners = new List<IDataChangeListener>();

        public BuildAgentRepository(IDataSource dataSource, IChannelConnectionStateBroadcaster channelStateBroadcaster, IBuildAgentViewModelFactory buildAgentFactory)
        {
            _dataSource = dataSource;
            _channelStateBroadcaster = channelStateBroadcaster;
            _buildAgentFactory = buildAgentFactory;
        }

        public IBuildAgent[] GetAll()
        {
            return _buildAgents.Values.OrderBy(x => x.Name).ToArray();
        }

        public void Add(IBuildAgent buildAgent)
        {
            _buildAgents.Add(buildAgent.Name, buildAgent);
            NotifyValueChanged();
        }

        public IBuildAgent Add(string name)
        {
            var buildAgent = _buildAgentFactory.Create(name, _channelStateBroadcaster, _dataSource);
            Add(buildAgent);
            return buildAgent;
        }

        public IBuildAgent Get(string name)
        {
            lock (_syncRoot)
            {
                var shortName = GetShortName(name);
                if (!Has(shortName))
                {
                    Add(shortName);
                }
                return _buildAgents[shortName];
            }
        }

        public bool Has(string name)
        {
            return _buildAgents.ContainsKey(GetShortName(name));
        }

        public void AddListener(IDataChangeListener listener)
        {
            _listeners.Add(listener);
            listener.OnChanged();
        }

        private string GetShortName(string name)
        {
            var longNamePrefix = $"{_dataSource.TypeName}.Agents.";
            if (name.StartsWith(longNamePrefix))
            {
                return name.Substring(longNamePrefix.Length);
            }
            return name;
        }

        private void NotifyValueChanged()
        {
            var listeners = _listeners.ToArray();
            foreach (var listener in listeners)
            {
                listener.OnChanged();
            }
        }
    }
}