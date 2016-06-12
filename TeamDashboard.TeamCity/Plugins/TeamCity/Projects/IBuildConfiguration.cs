using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Projects
{
    public interface IBuildConfiguration
    {
        string Id { get; set; }
        string Name { get; set; }
        string Href { get; set; }
        string ProjectId { get; set; }
        string ProjectName { get; set; }
        string Description { get; set; }
        string WebUrl { get; set; }
        Project Project { get; set; }
        Parameters Parameters { get; set; }
        ArtifactDependencies ArtifactDependencies { get; set; }
        SnapshotDependencies SnapshotDependencies { get; set; }
        VcsRootEntries VcsRootEntries { get; set; }
        BuildSteps Steps { get; set; }
        AgentRequirements AgentRequirements { get; set; }
        BuildTriggers Triggers { get; set; }
        Properties Settings { get; set; }
        Build[] GetRunningBuilds();
    }
}