namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Agents
{
    public interface IBuildAgent
    {
        string Name { get; }
        BuildAgentStatus Status { get; }
        bool IsRunning { get; }
    }
}