using System.ComponentModel;
using NoeticTools.TeamStatusBoard.Common;


namespace NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Agents
{
    public interface IBuildAgent : INotifyPropertyChanged
    {
        string Name { get; }
        DeviceStatus Status { get; }
        bool IsRunning { get; }
        bool IsOnline { get; set; }
        bool IsAuthorised { get; set; }
        void UpdateProperties();
    }
}