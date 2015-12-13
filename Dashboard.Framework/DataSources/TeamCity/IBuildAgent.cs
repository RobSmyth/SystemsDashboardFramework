namespace NoeticTools.SystemsDashboard.Framework.DataSources.TeamCity
{
    public interface IBuildAgent
    {
        string Name { get; }
        BuildAgentStatus Status { get; }
        bool IsRunning { get; }
    }
}