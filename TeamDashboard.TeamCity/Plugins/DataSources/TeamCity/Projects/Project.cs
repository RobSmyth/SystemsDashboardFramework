using System;
using System.Linq;
using log4net;
using NoeticTools.TeamStatusBoard.Framework.DataSources.Jira;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Channel;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Configurations;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Configurations;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.TcSharpInterop;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Projects
{
    public sealed class Project : IProject, IChannelConnectionStateListener
    {
        private TeamCitySharp.DomainEntities.Project _inner;
        private readonly ITcSharpTeamCityClient _teamCityClient;
        private readonly ILog _logger;
        private readonly TimeCachedArray<IBuildConfiguration> _buildConfigurationsCache;
        private BuildConfigurationRepository _configurations;

        public Project(TeamCitySharp.DomainEntities.Project inner, ITcSharpTeamCityClient teamCityClient, IServices services, IChannelConnectionStateBroadcaster channelStateBroadcaster)
        {
            _inner = inner;
            _teamCityClient = teamCityClient;
            _buildConfigurationsCache = new TimeCachedArray<IBuildConfiguration>(() => _teamCityClient.BuildConfigs.ByProjectId(Id).Select(x => new BuildConfiguration(x, this, teamCityClient)), TimeSpan.FromHours(12), services.Clock);
            _logger = LogManager.GetLogger("Repositories.Projects.Project");
            channelStateBroadcaster.Add(this);
            _configurations = new BuildConfigurationRepository(_teamCityClient, services, channelStateBroadcaster, this);
        }

        public IBuildConfiguration GetConfiguration(string name)
        {
            return Configurations.FirstOrDefault(x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)) ?? new NullBuildConfiguration(name);
        }

        public void Update(TeamCitySharp.DomainEntities.Project project)
        {
            _inner = project;
        }

        // todo - use BuildTypes property instead when inner Project is periodically updated.
        public IBuildConfiguration[] Configurations => _configurations.GetAll();

        public bool Archived => _inner.Archived;

        public string Description => _inner.Description;

        public string Href => _inner.Href;

        public string Id => _inner.Id;

        public string Name => _inner.Name;

        public string WebUrl => _inner.WebUrl;

        public Parameters Parameters => _inner.Parameters;

        void IChannelConnectionStateListener.OnConnected()
        {
            _buildConfigurationsCache.Start();
        }

        void IChannelConnectionStateListener.OnDisconnected()
        {
            _buildConfigurationsCache.Stop();
        }
    }
}
