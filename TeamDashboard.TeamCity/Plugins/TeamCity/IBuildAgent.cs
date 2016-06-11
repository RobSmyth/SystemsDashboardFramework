namespace NoeticTools.TeamStatusBoard.Framework.Plugins.DataSources.TeamCity
{
    public interface IBuildAgent
    {
        string Name { get; }
        BuildAgentStatus Status { get; }
        bool IsRunning { get; }
    }
}