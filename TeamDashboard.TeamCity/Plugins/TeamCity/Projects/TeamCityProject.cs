using System;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using NoeticTools.TeamStatusBoard.Framework.DataSources.Jira;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.TcSharpInterop;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Projects
{
    public sealed class TeamCityProject : IProject
    {
        private readonly Project _inner;
        private readonly ITcSharpTeamCityClient _teamCityClient;
        private readonly ILog _logger;
        private readonly TimeCachedArray<IBuildConfiguration> _buildsCache;
        private readonly object _syncRoot = new object();

        public TeamCityProject(Project inner, ITcSharpTeamCityClient teamCityClient, IServices services)
        {
            _inner = inner;
            _teamCityClient = teamCityClient;
            _buildsCache = new TimeCachedArray<IBuildConfiguration>(() => _teamCityClient.BuildConfigs.ByProjectId(Id).Select(x => new TeamCityBuildConfiguration(x, teamCityClient)), TimeSpan.FromHours(12), services.Clock);
            _logger = LogManager.GetLogger("Repositories.Projects.Project");
        }

        public Build[] GetRunningBuilds(string buildConfigurationName)
        {
            _logger.DebugFormat("Request for running build: {0} / {1}.", Name, buildConfigurationName);

            try
            {
                var buildConfiguration = GetConfiguration( buildConfigurationName);
                if (buildConfiguration == null)
                {
                    _logger.WarnFormat("Could not find configuration: {0} / {1}.", Name, buildConfigurationName);
                    return new Build[0];
                }
                return buildConfiguration.GetRunningBuilds();
            }
            catch (Exception exception)
            {
                _logger.Error("Exception while getting running build.", exception);
                return new Build[0];
            }
        }

        public IBuildConfiguration[] GetConfigurations()
        {
            _logger.DebugFormat("Request for configurations on project {0}.", Name);
            return _buildsCache.Items;
        }

        public bool Archived
        {
            get { return _inner.Archived; }
            set { _inner.Archived = value; }
        }

        public string Description
        {
            get { return _inner.Description; }
            set { _inner.Description = value; }
        }

        public string Href
        {
            get { return _inner.Href; }
            set { _inner.Href = value; }
        }

        public string Id
        {
            get { return _inner.Id; }
            set { _inner.Id = value; }
        }

        public string Name
        {
            get { return _inner.Name; }
            set { _inner.Name = value; }
        }

        public string WebUrl
        {
            get { return _inner.WebUrl; }
            set { _inner.WebUrl = value; }
        }

        public BuildTypeWrapper BuildTypes
        {
            get { return _inner.BuildTypes; }
            set { _inner.BuildTypes = value; }
        }

        public Parameters Parameters
        {
            get { return _inner.Parameters; }
            set { _inner.Parameters = value; }
        }

        private IBuildConfiguration GetConfiguration(string buildConfigurationName)
        {
            return _buildsCache.Items.SingleOrDefault(x => x.Name.Equals(buildConfigurationName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
