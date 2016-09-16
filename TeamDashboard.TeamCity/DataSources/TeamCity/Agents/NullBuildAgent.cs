using System;
using NoeticTools.TeamStatusBoard.Framework;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Agents
{
    public class NullBuildAgent : NotifyingViewModelBase, IBuildAgent
    {
        public NullBuildAgent(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public BuildAgentStatus Status => BuildAgentStatus.Unknown;
        public bool IsRunning => false;
        public bool IsOnline { get { return false; } set { throw new InvalidOperationException(); } }
        public bool IsAuthorised { get { return false; } set { throw new InvalidOperationException(); } }
    }
}