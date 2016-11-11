using System;
using System.Linq;
using log4net;
using NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Projects;
using NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.TcSharpInterop;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;


namespace NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Configurations
{
    public sealed class BuildConfiguration : IBuildConfiguration
    {
        private BuildConfig _inner;
        private readonly IProject _project;
        private readonly ITcSharpTeamCityClient _teamCityClient;
        private readonly ILog _logger;
        private readonly object _syncRoot = new object();

        public BuildConfiguration(BuildConfig inner, IProject project, ITcSharpTeamCityClient teamCityClient)
        {
            _inner = inner;
            _project = project;
            _teamCityClient = teamCityClient;
            _logger = LogManager.GetLogger("Project.BuildConfiguration");
        }

        public string Id => _inner.Id;

        public string Name => _inner.Name;

        public string Href => _inner.Href;

        public string ProjectId
        {
            get { return _inner.ProjectId; }
            set { _inner.ProjectId = value; }
        }

        public string ProjectName
        {
            get { return _inner.ProjectName; }
            set { _inner.ProjectName = value; }
        }

        public string Description => _inner.Description;

        public string WebUrl => _inner.WebUrl;

        public Parameters Parameters
        {
            get { return _inner.Parameters; }
            set { _inner.Parameters = value; }
        }

        public ArtifactDependencies ArtifactDependencies
        {
            get { return _inner.ArtifactDependencies; }
            set { _inner.ArtifactDependencies = value; }
        }

        public SnapshotDependencies SnapshotDependencies
        {
            get { return _inner.SnapshotDependencies; }
            set { _inner.SnapshotDependencies = value; }
        }

        public VcsRootEntries VcsRootEntries
        {
            get { return _inner.VcsRootEntries; }
            set { _inner.VcsRootEntries = value; }
        }

        public BuildSteps Steps
        {
            get { return _inner.Steps; }
            set { _inner.Steps = value; }
        }

        public AgentRequirements AgentRequirements
        {
            get { return _inner.AgentRequirements; }
            set { _inner.AgentRequirements = value; }
        }

        public Properties Settings
        {
            get { return _inner.Settings; }
            set { _inner.Settings = value; }
        }

        public Build GetLastBuild()
        {
            _logger.DebugFormat("Request for last build: {0} / {1}.", _project.Name, Name);

            try
            {
                var builds = _teamCityClient.Builds.ByBuildConfigId(Id);
                return builds.FirstOrDefault(x => x.Status != "UNKNOWN");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Build GetLastSuccessfulBuild()
        {
            _logger.DebugFormat("Request for last successful build: {0} / {1}.", _project.Name, Name);

            lock (_syncRoot)
            {
                try
                {
                    var builds = _teamCityClient.Builds.SuccessfulBuildsByBuildConfigId(Id);
                    var lastBuild = builds.FirstOrDefault();
                    if (lastBuild == null)
                    {
                        _logger.WarnFormat("Could not find a last successful build for: {0} / {1}.", _project.Name, Name);
                    }
                    else
                    {
                        _logger.DebugFormat("Last successful build was run at {2} for: {0} / {1}.", _project.Name, Name, lastBuild.StartDate);
                    }
                    return lastBuild;
                }
                catch (Exception exception)
                {
                    _logger.Error("Exception getting last successful build.", exception);
                    return null;
                }
            }
        }

        public void Update(BuildConfig tcsConfiguration)
        {
            _inner = tcsConfiguration;
        }

        public Build[] GetRunningBuilds()
        {
            try
            {
                var builds = _teamCityClient.Builds.ByBuildLocator(BuildLocator.WithDimensions(running: true, branch: "default:any")).Where(x => x.WebUrl.EndsWith(Id)).ToArray();
                foreach (var build in builds)
                {
                    build.Status = build.Status == "FAILED" ? "RUNNING FAILED" : "RUNNING";
                }
                return builds;
            }
            catch (Exception)
            {
                return new Build[0];
            }
        }
    }
}