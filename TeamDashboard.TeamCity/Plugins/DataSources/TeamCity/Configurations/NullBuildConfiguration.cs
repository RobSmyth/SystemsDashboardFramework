using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Configurations
{
    public sealed class NullBuildConfiguration : IBuildConfiguration
    {
        public NullBuildConfiguration(string name)
        {
            Name = name;
            Description = "Unknown";
            Id = "";
            Href = "";
            WebUrl = "";
        }

        public string Id { get; }
        public string Name { get; }
        public string Href { get; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; }
        public string WebUrl { get; }
        public Parameters Parameters { get; set; }
        public ArtifactDependencies ArtifactDependencies { get; set; }
        public SnapshotDependencies SnapshotDependencies { get; set; }
        public VcsRootEntries VcsRootEntries { get; set; }
        public BuildSteps Steps { get; set; }
        public AgentRequirements AgentRequirements { get; set; }
        public Properties Settings { get; set; }
        public Build[] GetRunningBuilds() { return new Build[0]; }
        public Build GetLastBuild() { return null; }
        public Build GetLastSuccessfulBuild() { return null; }
        public void Update(BuildConfig tcsConfiguration)
        {
        }
    }
}