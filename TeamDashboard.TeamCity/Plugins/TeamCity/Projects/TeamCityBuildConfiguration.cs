using System.Linq;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.TcSharpInterop;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Projects
{
    public sealed class TeamCityBuildConfiguration : IBuildConfiguration
    {
        private readonly BuildConfig _inner;
        private readonly ITcSharpTeamCityClient _teamCityClient;

        public TeamCityBuildConfiguration(BuildConfig inner, ITcSharpTeamCityClient teamCityClient)
        {
            _inner = inner;
            _teamCityClient = teamCityClient;
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

        public string Href
        {
            get { return _inner.Href; }
            set { _inner.Href = value; }
        }

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

        public string Description
        {
            get { return _inner.Description; }
            set { _inner.Description = value; }
        }

        public string WebUrl
        {
            get { return _inner.WebUrl; }
            set { _inner.WebUrl = value; }
        }

        public Project Project
        {
            get { return _inner.Project; }
            set { _inner.Project = value; }
        }

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

        public BuildTriggers Triggers
        {
            get { return _inner.Triggers; }
            set { _inner.Triggers = value; }
        }

        public Properties Settings
        {
            get { return _inner.Settings; }
            set { _inner.Settings = value; }
        }

        public Build[] GetRunningBuilds()
        {
            var builds = _teamCityClient.Builds.ByBuildLocator(BuildLocator.WithDimensions(running: true, branch: "default:any")).Where(x => x.WebUrl.EndsWith(Id)).ToArray();
            foreach (var build in builds)
            {
                build.Status = build.Status == "FAILED" ? "RUNNING FAILED" : "RUNNING";
            }
            return builds;
        }
    }
}