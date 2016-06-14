using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Configurations
{
    public interface IBuildConfiguration
    {
        string Id { get; }
        string Name { get; }
        string Href { get; }
        string ProjectId { get; set; }
        string ProjectName { get; set; }
        string Description { get; }
        string WebUrl { get; }
        Parameters Parameters { get; set; }
        ArtifactDependencies ArtifactDependencies { get; set; }
        SnapshotDependencies SnapshotDependencies { get; set; }
        VcsRootEntries VcsRootEntries { get; set; }
        BuildSteps Steps { get; set; }
        AgentRequirements AgentRequirements { get; set; }
        Properties Settings { get; set; }
        Build[] GetRunningBuilds();
        Build GetLastBuild();
        Build GetLastSuccessfulBuild();
    }
}