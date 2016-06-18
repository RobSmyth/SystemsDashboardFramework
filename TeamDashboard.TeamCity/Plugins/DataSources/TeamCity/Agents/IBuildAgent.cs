using System.ComponentModel;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Agents
{
    public interface IBuildAgent : INotifyPropertyChanged
    {
        string Name { get; }
        BuildAgentStatus Status { get; }
        bool IsRunning { get; }
        bool IsOnline { get; set; }
        bool IsAuthorised { get; set; }
    }
}