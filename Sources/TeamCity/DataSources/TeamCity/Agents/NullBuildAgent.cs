using System;
using NoeticTools.TeamStatusBoard.Common;
using NoeticTools.TeamStatusBoard.Common.ViewModels;


namespace NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Agents
{
    public class NullBuildAgent : NotifyingViewModelBase, IBuildAgent
    {
        public NullBuildAgent(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public DeviceStatus Status => DeviceStatus.Unknown;
        public bool IsRunning => false;
        public bool IsOnline { get { return false; } set { throw new InvalidOperationException(); } }
        public bool IsAuthorised { get { return false; } set { throw new InvalidOperationException(); } }

        public void UpdateProperties()
        {
        }
    }
}